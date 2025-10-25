using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;

namespace ProtoType2.Tools.UI;

public class FileHasher
{
    public static async Task<ConcurrentDictionary<string, string>> CalculateHash_MD5(string[] files, int? MaxDegreeOfParallelism = null)
    {
        var options = new ParallelOptions
        {
            MaxDegreeOfParallelism = MaxDegreeOfParallelism ?? 5 // Paralellik sınırı
        };
        ConcurrentDictionary<string, string> md5list = [];
        await Task.Run(() => Parallel.ForEach(files, file =>
        {
            md5list.TryAdd(file, CalculateHash_MD5(file));
        }));
        return md5list;
    }
    public static string CalculateHash_MD5(string filePath)
    {
        try
        {
            using var md5 = MD5.Create();
            using var stream = File.OpenRead(filePath);
            byte[] hashBytes = md5.ComputeHash(stream);
            return ConvertBytesToHexString(hashBytes);
        }
        catch (FileNotFoundException)
        {
            return $"Hata: '{filePath}' dosyası bulunamadı.";
        }
        catch (UnauthorizedAccessException)
        {
            return $"Hata: '{filePath}' dosyasını okuma izni yok.";
        }
        catch (Exception ex)
        {
            return $"Bilinmeyen bir hata oluştu: {ex.Message}";
        }
    }

    private static string ConvertBytesToHexString(byte[] bytes)
    {
        StringBuilder builder = new();
        for (int i = 0; i < bytes.Length; i++)
        {
            builder.Append(bytes[i].ToString("x2"));
        }
        return builder.ToString();
    }
}
