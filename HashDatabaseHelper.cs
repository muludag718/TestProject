using System.Collections.Concurrent;
using System.Text.Json;

namespace ProtoType2.Tools.UI;

/// <summary>
/// Dosya metadata bilgilerini tutar.
/// </summary>
[Serializable]
public class FileMetaData
{
    public string Tag { get; set; } = string.Empty;
    public string Hash { get; set; } = string.Empty;
    public string OutputFile { get; set; } = string.Empty;
    public List<string> Labels { get; set; } = [];
    public DateTime CreateTime { get; set; } = DateTime.Now;
}

/// <summary>
/// Disk üzerinde JSON tabanlı metadata veritabanı yönetimi.
/// Güvenli, thread-safe ve veri kaybına dayanıklı.
/// </summary>
public class HashDatabaseHelper : IDisposable
{
    private readonly string _databaseFilePath;
    private readonly string _backupFilePath;
    private readonly string _tempFilePath;

    // Disk yazımı ve _isDirty bayrağı için kilit
    private readonly object _saveLock = new object();

    // Asıl veritabanı (dosya yolu -> FileMetaData)
    private readonly ConcurrentDictionary<string, FileMetaData> _hashDatabase;

    private bool _isDirty = false;

    public HashDatabaseHelper(string databaseFilePath)
    {
        databaseFilePath = Application.StartupPath + databaseFilePath;
        _databaseFilePath = databaseFilePath;
        _backupFilePath = databaseFilePath + ".bak";
        _tempFilePath = databaseFilePath + ".tmp";

        _hashDatabase = LoadDatabaseFromFile();
    }

    /// <summary>
    /// Yeni metadata kaydeder veya günceller (thread-safe).
    /// </summary>
    public bool SaveMetaToDatabase(string filePath, FileMetaData meta)
    {
        if (string.IsNullOrEmpty(filePath) || meta == null)
            return false;

        // Aynı veri varsa gereksiz yazma yapma
        if (_hashDatabase.TryGetValue(filePath, out var existingMeta))
        {
            if (existingMeta.Hash == meta.Hash &&
                existingMeta.Tag == meta.Tag &&
                existingMeta.OutputFile == meta.OutputFile &&
                existingMeta.Labels.SequenceEqual(meta.Labels))
            {
                return true;
            }
        }

        _hashDatabase[filePath] = meta;

        lock (_saveLock)
        {
            _isDirty = true;
        }

        return true;
    }

    /// <summary>
    /// Dosya yoluna göre metadata getirir.
    /// </summary>
    public FileMetaData? GetMetaByPath(string filePath)
    {
        _hashDatabase.TryGetValue(filePath, out var meta);
        return meta;
    }

    /// <summary>
    /// Hash değerine göre tüm dosya yollarını getirir.
    /// </summary>
    public List<string> GetPathsByHash(string hashToFind)
    {
        return _hashDatabase
            .Where(pair => pair.Value.Hash.Equals(hashToFind, StringComparison.OrdinalIgnoreCase))
            .Select(pair => pair.Key)
            .ToList();
    }
    public Dictionary<string, FileMetaData> GetMetaDataByHash(string hashToFind)
    {
        return _hashDatabase
            .Where(pair => pair.Value.Hash.Equals(hashToFind, StringComparison.OrdinalIgnoreCase))
            .ToDictionary();
    }
    /// <summary>
    /// Etiket (Label) bazlı arama.
    /// </summary>
    public List<string> GetPathsByLabel(string label)
    {
        return _hashDatabase
            .Where(pair => pair.Value.Labels.Contains(label, StringComparer.OrdinalIgnoreCase))
            .Select(pair => pair.Key)
            .ToList();
    }

    /// <summary>
    /// Bellekteki değişiklikleri güvenli şekilde diske yazar.
    /// </summary>
    public void SaveChanges()
    {
        bool needsToSave;
        lock (_saveLock)
        {
            needsToSave = _isDirty;
            _isDirty = false;
        }

        if (!needsToSave)
            return;

        Console.WriteLine("[Helper] Değişiklik algılandı, diske güvenli şekilde kaydediliyor...");

        try
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string jsonContent = JsonSerializer.Serialize(_hashDatabase, options);

            // 1️⃣ Geçici dosyaya yaz
            File.WriteAllText(_tempFilePath, jsonContent);

            // 2️⃣ Eski dosyanın yedeğini al (.bak)
            if (File.Exists(_databaseFilePath))
            {
                File.Copy(_databaseFilePath, _backupFilePath, true);
            }

            // 3️⃣ Geçici dosyayı atomik olarak asıl dosyayla değiştir
            File.Replace(_tempFilePath, _databaseFilePath, _backupFilePath, ignoreMetadataErrors: true);

            Console.WriteLine("[Helper] Veritabanı başarıyla kaydedildi.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Helper] Hata: Veritabanı kaydedilemedi: {ex.Message}");

            lock (_saveLock)
            {
                _isDirty = true; // sonraki kaydetmede tekrar dene
            }

            try
            {
                if (File.Exists(_tempFilePath))
                    File.Move(_tempFilePath, _tempFilePath + ".error", true);
            }
            catch { }
        }
    }

    public void Dispose()
    {
        SaveChanges();
    }

    /// <summary>
    /// Diskten JSON veritabanını yükler, bozulma varsa yedekten geri döner.
    /// </summary>
    private ConcurrentDictionary<string, FileMetaData> LoadDatabaseFromFile()
    {
        Dictionary<string, FileMetaData>? tempDictionary;

        if (TryLoadFromJson(_databaseFilePath, out tempDictionary))
            return new ConcurrentDictionary<string, FileMetaData>(tempDictionary);

        Console.WriteLine("[Helper] Ana dosya okunamadı, yedekten deneniyor...");

        if (TryLoadFromJson(_backupFilePath, out tempDictionary))
        {
            Console.WriteLine("[Helper] Yedekten geri yükleme başarılı.");
            try
            {
                File.Copy(_backupFilePath, _databaseFilePath, true);
            }
            catch { }
            return new ConcurrentDictionary<string, FileMetaData>(tempDictionary);
        }

        Console.WriteLine("[Helper] Hiçbir kaynak yüklenemedi, boş veritabanı oluşturuluyor.");
        return new ConcurrentDictionary<string, FileMetaData>();
    }

    /// <summary>
    /// JSON dosyasını güvenli biçimde yükler.
    /// </summary>
    private bool TryLoadFromJson(string path, out Dictionary<string, FileMetaData>? result)
    {
        result = null;
        try
        {
            if (!File.Exists(path))
                return false;

            string jsonContent = File.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(jsonContent))
                return false;

            result = JsonSerializer.Deserialize<Dictionary<string, FileMetaData>>(jsonContent);
            return result != null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Helper] Hata ({Path.GetFileName(path)}): {ex.Message}");
            return false;
        }
    }
}
