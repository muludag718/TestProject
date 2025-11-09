using Newtonsoft.Json;

using ProtoType2.Tools.RadpToWav;
using System.Collections.Concurrent;
using System.Data;
using System.Text;
using System.Text.Json;
using TestProject.P3DFile;
using TestProject.P3DFile.Interface;
using TestProject.P3DFile.Models;
using Timer = System.Windows.Forms.Timer;

namespace ProtoType2.Tools.UI
{
    public partial class P3DForm : Form
    {

        private readonly HashDatabaseHelper HashDatabase;

        private ConcurrentBag<IChunkData> allSections = [];

        private ConcurrentQueue<string> logQueue = [];

        private Timer? logTimer;

        private CancellationTokenSource cancelTSource = new();

        #region UI Process

        public P3DForm()
        {
            InitializeComponent();
            InitLogger();
            HashDatabase = new("Hash_Files.json");
        }


        private async void Start_Click(object sender, EventArgs e)
        {
            if (TxtValidation())
            {
                await AfterFileProcess(TxtInputFolder.Text);
            }
            else
            {
                MessageBox.Show("Folder Select");
            }
        }
        private void BtnInputFolder_Click(object sender, EventArgs e)
        {
            using FolderBrowserDialog fdg = new();
            if (fdg.ShowDialog() == DialogResult.OK)
            {
                TxtInputFolder.Text = fdg.SelectedPath;
            }
        }

        private void BtnOutputFolder_Click(object sender, EventArgs e)
        {
            using FolderBrowserDialog fdg = new();
            if (fdg.ShowDialog() == DialogResult.OK)
            {
                TxtOutputFolder.Text = fdg.SelectedPath;
            }
        }


        private void BtnStop_Click(object sender, EventArgs e)
        {
            cancelTSource.Cancel();
        }

        private void UpdateProgress(int processed, int total)
        {
            UpdateLabel(processed, total);

            if (PbFile.InvokeRequired)
            {
                PbFile.BeginInvoke(new Action(() => UpdateProgress(processed, total)));
            }
            else
            {
                PbFile.Value = processed;
            }
        }

        private void UpdateLabel(int processed, int total)
        {
            if (LblCounter.InvokeRequired)
            {
                LblCounter.BeginInvoke(new Action(() => UpdateLabel(processed, total)));
            }
            else
            {
                LblCounter.Text = $"{processed}/{total}";
            }
        }


        #endregion
        private async Task AfterFileProcess(string folderPath)
        {
            //md5 hesaplama      //md5 karşilaştirması 
            string[] files = Directory.GetFiles(folderPath, "*.p3d", SearchOption.AllDirectories);
            files = await HashProcess(files);
            try
            {
                await ProcessP3D(files);
            }
            catch (OperationCanceledException)
            {
                Log("Döngü iptal edildi.");
            }




        }
        #region Process P3D

        private async Task ProcessP3D(string[] files)
        {
            var options = new ParallelOptions
            {
                MaxDegreeOfParallelism = 5,
                CancellationToken = cancelTSource.Token
            };
            int totalFiles = files.Length;
            int processedFiles = 0;
            PbFile.Minimum = 0;
            PbFile.Maximum = totalFiles;
            PbFile.Value = 0;


            await Parallel.ForEachAsync(files, options, async (file, cancellationToken) =>
            {
                try
                {
                    var parser = new P3D();
                    parser.Deserialize(file);
                    var sections = parser.Chunks;
                    LogWrite($"{file}-->");
                    string? wavPath = null;
                    foreach (var section in sections)
                    {
                        LogWrite(section.ToString() + "\t");
                        if (section is AudioRadp) SaveWav((AudioRadp)section, file, out wavPath);
                        allSections.Add(section);
                    }
                    Log($" <--{sections.Count}");
                    //SaveHash(file, sections, wavPath);
                }
                catch (Exception e)
                {
                    Log($"{file}, {e.Message}");
                }
                finally
                {
                    Interlocked.Increment(ref processedFiles);
                    UpdateProgress(processedFiles, totalFiles);
                    SaveAudioDialogJson();
                }
            });
        }



        private void SaveAudioDialogJson()
        {
            try
            {

                var newBase = IsValidPath(TxtOutputFolder.Text) ? TxtOutputFolder.Text : Application.StartupPath + "\\Output";
                string subtitlePath = Path.Combine(newBase, "OutputJson", $"subtitle.json");
                Directory.CreateDirectory(Path.Combine(newBase, "OutputJson"));
                var jsonObj = allSections.Select(s =>
                {
                    if (s is AudioSubtitle sub) return sub.ToJsonObject();
                    return null;
                }).Where(o => o != null).ToList();
                var settings = new JsonSerializerSettings
                {
                    StringEscapeHandling = StringEscapeHandling.Default
                };
                string json = JsonConvert.SerializeObject(jsonObj, Formatting.Indented, settings);
                string jsonclean = json.Replace("\u0000", "");
                File.WriteAllText(subtitlePath, jsonclean, Encoding.UTF8);

            }
            catch
            {
            }
        }

        private void SaveWav(AudioRadp audio, string filePath, out string wavPath)
        {
            var relativePath = Path.GetRelativePath(TxtInputFolder.Text, Path.GetDirectoryName(filePath) ?? "");
            string fileName = Path.GetFileNameWithoutExtension(filePath);

            var newBase = IsValidPath(TxtOutputFolder.Text) ? TxtOutputFolder.Text : Application.StartupPath + "\\Output";

            Directory.CreateDirectory(Path.Combine(newBase, relativePath));

            wavPath = Path.Combine(newBase, relativePath, $"{fileName}.wav");

            var samples = audio.ToWavData();

            if (samples.Count > 0)
                samples.WriteToWavFile(wavPath, 48000);
        }
        private void SaveHash(string filePath, List<IChunkData> sections, string? outputPath)
        {
            var hash = FileHasher.CalculateHash_MD5(filePath);
            HashDatabase.SaveMetaToDatabase(filePath, new FileMetaData
            {
                Tag = Path.GetFileNameWithoutExtension(filePath),
                Hash = hash,
                Labels = [.. sections.Select((item) => item.ToString())],
                OutputFile = outputPath ?? ""
            });
            HashDatabase.SaveChanges();
        }

        private JsonSerializerOptions GetOptions()
        {
            return new JsonSerializerOptions { WriteIndented = true };

        }

        #endregion

        #region Validation
        bool TxtValidation()
        {
            return !string.IsNullOrEmpty(TxtInputFolder.Text);
        }
        static bool IsValidPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return false;

            try
            {
                string fullPath = Path.GetFullPath(path); // Geçerli bir tam yol oluşturabiliyorsa
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region Hash Process
        private async Task<string[]> HashProcess(string[] files)
        {
            #region MD File Hash
            Log("Hash hesaplaması yapılıyor");
            var hasher = await FileHasher.CalculateHash_MD5(files);
            Log($"{files.Length} tane dosyanın hash hesaplaması yapıldı");
            #endregion

            #region Hash DataBase 
            Log("Kayıt ve sorgu Başlatıldı");

            var filesList = files.ToList();

            int counter = 0;

            foreach (var item in hasher)
            {
                var filename = HashDatabase.GetMetaDataByHash(item.Value);
                if (filename != null && filename.Any(pair => pair.Key == item.Key))
                {
                    counter++;
                    Log($"{item.Key} {item.Value} record found");
                    filesList.Remove(item.Key);
                }

            }
            return filesList.ToArray();

            #endregion
        }



        #endregion

        #region Logger Process


        private void InitLogger()
        {
            logQueue = new ConcurrentQueue<string>();

            logTimer = new Timer();
            logTimer.Interval = 50; // 50ms aralıklarla queue'yu kontrol eder
            logTimer.Tick += LogTimer_Tick;
            logTimer.Start();
        }
        private void LogTimer_Tick(object? sender, EventArgs e)
        {
            while (logQueue.TryDequeue(out var message))
            {
                logTextBox.AppendText(message);
            }
        }
        public void Log(string message)
        {
            if (string.IsNullOrEmpty(message)) return;
            logQueue.Enqueue(message + Environment.NewLine);
        }

        public void LogWrite(string message)
        {
            if (string.IsNullOrEmpty(message)) return;
            logQueue.Enqueue(message);
        }
        #endregion

    }
}
