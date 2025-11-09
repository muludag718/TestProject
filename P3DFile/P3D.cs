using TestProject.P3DFile.Helpers;
using TestProject.P3DFile.Interface;
using TestProject.P3DFile.Models;

namespace TestProject.P3DFile;

public class P3D
{
    public string FilePath { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public List<uint> TypeId { get; } = [];

    public const uint Magic = 4282659664;

    public const uint Header = 12;


    public uint FileSize { get; set; }

    public List<IChunkData> Chunks { get; } = [];

    public int ChunkLength => Chunks.Count;

    public void Deserialize(string filePath)
    {
        var input = FileController(filePath);
        if (input == null)
        {
            return;
        }
        while (input.Position < FileSize)
        {
            Chunks.Add(Deserilaze(input));
        }
        if (input.Position != FileSize)
        {
            throw new FormatException();
        }
    }
    private IChunkData Deserilaze(Stream input)
    {
        long position = input.Position;
        var type = input.ReadValueU32();
        uint HeaderSize = input.ReadValueU32();
        var LengthData = input.ReadValueU32();

        IChunkData? chunkData = GetIParse(type, input);

        if (chunkData != null)
        {
            chunkData.Deserialize(input);
        }
        else
        {
            chunkData = new UnknowFile(type)
            {
                StartPosition = (uint)position,
                Data = input.ReadBytes((int)(LengthData - 12))
            };

        }


        return chunkData;
    }
    private static IChunkData? GetIParse(uint DataID, Stream input)
    {
        return DataID switch
        {
            4261412864 => AudioFileHelper.GetChuckData(input),
            98818 => new TextBibleDataHeader(),
            98817 => new TextBibleData(),
            _ => null,
        };
    }
    private Stream? FileController(string filePath)
    {
        try
        {

            using var input = File.OpenRead(filePath);

            var magic = input.ReadValueU32();
            if (magic != Magic)
            {
                return null;
            }
            var header = input.ReadValueU32();
            if (header != Header)
            {
                return null;
            }
            var fileTotalSize = input.ReadValueU32();

            var fileName = Path.GetFileName(filePath);


            FilePath = filePath;
            FileSize = fileTotalSize;
            FileName = fileName;


            return input;//position =12
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}

