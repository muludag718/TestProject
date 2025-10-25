using System.Collections.Concurrent;
using TestProject.P3DFile;
using TestProject.P3DFile.Helpers;
using TestProject.P3DFile.Interface;

namespace TestProject
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private string? p3DFile = null;
        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using OpenFileDialog fileDialog = new();
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                p3DFile = fileDialog.FileName;
            }
            Parse();
        }

        void Parse()
        {
            if (p3DFile == null) return;
            var p3d = new P3DParser(p3DFile);
            var result = p3d.Deserialize();
            treeView1.Nodes.Clear();
            createTreeViewNodes(result);
        }
        private void createTreeViewNodes(List<IBaseParse> bases)
        {
            foreach (var item in bases)
            {
                TreeNode node = new TreeNode(item.ToString());
                foreach (var item2 in item.Children)
                {
                    node.Nodes.Add(item2.ToString());
                }
                treeView1.Nodes.Add(node);
            }
        }
        string? InputFolder = null;
        private void openFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using FolderBrowserDialog fdg = new();
            if (fdg.ShowDialog() == DialogResult.OK)
            {
                InputFolder = fdg.SelectedPath;
            }
            ParseAll();
        }
        ConcurrentDictionary<uint, string> Uints = [];
        async void ParseAll()
        {
            if (InputFolder == null) return;
            string[] files = Directory.GetFiles(InputFolder, "*.p3d", SearchOption.AllDirectories);
            var options = new ParallelOptions
            {
                MaxDegreeOfParallelism = 5,
            };
            int totalFiles = files.Length;
            int processedFiles = 0;
            PbFile.Minimum = 0;
            PbFile.Maximum = totalFiles;
            PbFile.Value = 0;
            await Task.Run(() =>
            {
                Parallel.ForEach(files, file =>
                {
                    try
                    {
                        ParalelParse(file);
                    }
                    finally
                    {
                        Interlocked.Increment(ref processedFiles);
                        UpdateProgress(processedFiles, totalFiles);

                    }
                });
            });
            foreach (var item in Uints)
            {
                richTextBox1.Text += item.Key.ToString() + " " + item.Value + "\n";
            }
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
        void ParalelParse(string file)
        {
            using var input = File.OpenRead(file);

            var magic = input.ReadValueU32();
            if (magic != 4282659664/*1345537279*/)
            {
                return;
            }
            var header = input.ReadValueU32();
            if (header != 12)
            {
                return;
            }
            var fileTotalSize = input.ReadValueU32();

            var type = input.ReadValueU32();
            uint HeaderSize = input.ReadValueU32();
            var lenghtData = input.ReadValueU32();
            Uints.TryAdd(type, file);
        }
    }
}
