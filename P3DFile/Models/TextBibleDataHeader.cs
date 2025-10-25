using TestProject.P3DFile.Helpers;
using TestProject.P3DFile.Interface;

namespace TestProject.P3DFile.Models;

public class TextBibleDataHeader : IChunkData
{
    public override uint TypeId => 98818;
    public override string Name => "TextBibleDataHeader";

    public string Language { get; set; } = string.Empty;
    public int Version { get; set; }
    public List<string> Keys { get; } = [];


    public override void Deserialize(Stream input)//input position 12
    {
        var type = input.ReadValueU32();
        uint HeaderSize = input.ReadValueU32();
        var LengthData = input.ReadValueU32();

        Language = input.ReadStringAlignedU8();
        Version = (int)input.ReadValueU32();
        var Count = (int)input.ReadValueU32();

        for (var i = 0; i < Count; i++)
        {
            var inputvalue = input.ReadStringAlignedU8();
            Keys.Add(inputvalue);
        }
        var StringStarts = new List<uint>();
        for (var i = 0; i < Count; i++)
        {
            var inputvalue = input.ReadValueU32();
            StringStarts.Add(inputvalue);
        }
        var StringStops = new List<uint>();
        for (var i = 0; i < Count; i++)
        {
            var inputvalue = input.ReadValueU32();
            StringStops.Add(inputvalue);
        }

    }
}
