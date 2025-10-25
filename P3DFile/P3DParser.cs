using System.IO;
using TestProject.P3DFile.Helpers;
using TestProject.P3DFile.Interface;
using TestProject.P3DFile.Models;

namespace TestProject.P3DFile;

public class P3DParser(string Path)
{
    public void Serialize()
    {

    }

    //Read 12 btye 
    public List<IBaseParse> Deserialize()
    {
        var result = new List<IBaseParse>();
        try
        {

            using var input = File.OpenRead(Path);

            var magic = input.ReadValueU32();
            if (magic != 4282659664/*1345537279*/)
            {
                return [];
            }
            var header = input.ReadValueU32();
            if (header != 12)
            {
                return [];
            }
            var fileTotalSize = input.ReadValueU32();
            while (input.Position < fileTotalSize)
            {
                result.Add(DeserializeParse(input));
            }
            if (input.Position != fileTotalSize)
            {
                throw new FormatException();
            }
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        return result;
    }

    private static IBaseParse DeserializeParse(Stream input, IBaseParse? parent = null)
    {
        long position = input.Position;
        var type = input.ReadValueU32();
        uint HeaderSize = input.ReadValueU32();
        var lenghtData = input.ReadValueU32();
        input.Position = position;
        IBaseParse baseParse = CreateIParse(input, parent);
        if (input.Position != position + HeaderSize)
        {
            throw new FormatException();
        }
        long len = position + lenghtData;
        while (input.Position < len)
        {
            var parseData = DeserializeParse(input, baseParse);
            baseParse.Children.Add(parseData);
        }
        if (input.Position != len)
        {
            throw new FormatException();
        }
        return baseParse;

    }
    private static IBaseParse CreateIParse(Stream input, IBaseParse? parent = null)
    {
        long position = input.Position;
        var type = input.ReadValueU32();
        uint _ = input.ReadValueU32();
        var LengthData = input.ReadValueU32();

        input.Position = position;
        IBaseParse? baseParse = GetIParse(type);
        if (baseParse != null)
        {
            baseParse.StartPosition = (uint)input.Position;
            baseParse.TotalSize = LengthData;
            baseParse.Deserialize(input);
        }
        else
        {
            baseParse = new UnknowFile(type)
            {
                StartPosition = (uint)position,
                TotalSize = LengthData,
                Data = input.ReadBytes((int)(LengthData - 12))
            };
        }

        return baseParse;
    }
    private static IBaseParse? GetIParse(uint DataID)
    {
        switch (DataID)
        {
            case 4261412864: return new AudioFile();
            case 98818: return new TextBibleDataHeader();
            case 98817: return new TextBibleData();
            default: return null;
        }
    }

}
