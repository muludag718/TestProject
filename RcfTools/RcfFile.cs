using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.P3DFile.Helpers;

namespace TestProject.RcfTools;

public class RcfFile
{
    private string FilePath { get; init; }

    public RcfFile(string filePath)
    {
        FilePath = filePath;
    }
    public void Deserilaze(string OutputPath)
    {

    }

    public byte MajorVersion;

    public byte MinorVersion;

    public uint EntryCount;

    public readonly List<Entry> Entries = new List<Entry>();

    public readonly Dictionary<uint, Entry> EntriesDict = new Dictionary<uint, Entry>();

    public readonly List<Metadata> Metadatas = new List<Metadata>();

    public CementFile()
    {
    }

    public CementFile(Stream input)
    {
        Deserialize(input);
    }

  

    public int EstimateEntryTableSize()
    {
        return Entries.Sum((Entry e) => Entry.ByteSize);
    }

    public int EstimateMetadataTableSize()
    {
        return 8 + Metadatas.Sum((Metadata e) => e.ByteSize);
    }


    public void Deserialize(Stream input)
    {
        if (input.ReadString(24, trailingNull: true, Encoding.ASCII) != "ATG CORE CEMENT LIBRARY")
        {
            throw new FormatException("Not a Cement file");
        }
        input.ReadBytes(8);
        MajorVersion = input.ReadValueU8();
        MinorVersion = input.ReadValueU8();
        var endian = input.ReadValueB8();
        input.ReadValueU8();
        uint num = input.ReadValueU32();
        uint size = input.ReadValueU32();
        uint num2 = input.ReadValueU32();
        uint size2 = input.ReadValueU32();
        input.ReadValueU32();
        EntryCount = input.ReadValueU32();
        input.Seek(num, SeekOrigin.Begin);
        using (MemoryStream memoryStream = input.ReadToMemoryStream((int)size))
        {
            Entries.Clear();
            for (int i = 0; i < EntryCount; i++)
            {
                Entry entry = new Entry();
                entry.Deserialize(memoryStream);
                Entries.Add(entry);
                EntriesDict.Add(entry.Hash, entry);
            }
            if (memoryStream.Position != memoryStream.Length)
            {
                throw new FormatException();
            }
        }
        input.Seek(num2, SeekOrigin.Begin);
        using (MemoryStream memoryStream2 = input.ReadToMemoryStream((int)size2))
        {
            memoryStream2.ReadValueU32();
            memoryStream2.ReadValueU32();
            Metadatas.Clear();
            for (int j = 0; j < EntryCount; j++)
            {
                Metadata metadata = new Metadata();
                metadata.Deserialize(memoryStream2);
                Metadatas.Add(metadata);
            }
        }
        foreach (Entry entry2 in Entries)
        {
            input.Seek(entry2.Offset, SeekOrigin.Begin);
            entry2.Data = input.ReadBytes((int)entry2.Size);
        }
    }

    public void Pack(string path)
    {
        string[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
        for (int i = 0; i < files.Length; i++)
        {
            files[i] = files[i].Substring(path.Length + 1, files[i].Length - path.Length - 1);
        }
        EntryCount = (uint)files.Length;
        for (int j = 0; j < EntryCount; j++)
        {
            Entry entry = new Entry
            {
                Hash = Utils.RCFStringHash(files[j])
            };
            using (FileStream fileStream = File.OpenRead(Path.Combine(path, files[j])))
            {
                using MemoryStream memoryStream = new MemoryStream();
                fileStream.CopyTo(memoryStream);
                entry.Data = memoryStream.ToArray();
                entry.Size = (uint)entry.Data.Length;
            }
            Entries.Add(entry);
            Metadata item = new Metadata
            {
                Date = DateTime.Now.GetUnixEpoch(),
                Name = files[j]
            };
            Metadatas.Add(item);
        }
    }

    public void Unpack(string path)
    {
        for (int i = 0; i < EntryCount; i++)
        {
            uint key = Utils.RCFStringHash(Metadatas[i].Name.Trim(default(char)));
            Entry entry = EntriesDict[key];
            string path2 = Metadatas[i].Name.Trim(default(char));
            string directoryName = Path.GetDirectoryName(path2);
            using FileStream stream = File.Create(Path.Combine(Directory.CreateDirectory(Path.Combine(path, directoryName)).FullName, Path.GetFileName(path2)));
            stream.WriteBytes(entry.Data);
        }
    }
}
public class Entry
{
    public uint Hash;

    public uint Offset;

    public uint Size;

    public byte[] Data;

    public static int ByteSize => 12;

    public void Serialize(Stream output, Endian endian)
    {
        output.WriteValueU32(Hash, endian);
        output.WriteValueU32(Offset, endian);
        output.WriteValueU32(Size, endian);
    }

    public void Deserialize(Stream input, Endian endian)
    {
        Hash = input.ReadValueU32(endian);
        Offset = input.ReadValueU32(endian);
        Size = input.ReadValueU32(endian);
    }
}