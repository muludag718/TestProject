
using TestProject.P3DFile.Helpers;
using TestProject.P3DFile.Interface;

namespace TestProject.P3DFile.Models;

public class TextBibleData : IChunkData
{
    public override string Name => "TextBibleData";
    public override uint TypeId => 98817;

    public string RawData { get; set; } = string.Empty;



    public override void Deserialize(Stream input)
    {
        var type = input.ReadValueU32();
        uint _ = input.ReadValueU32();
        var LengthData = input.ReadValueU32();

        var Unknown1 = input.ReadStringAlignedU8();
        var Unknown2 = input.ReadValueU32();
        uint num = input.ReadValueU32();
        var Data = new byte[num];
        input.Read(Data, 0, Data.Length);
        RawData = System.Text.Encoding.UTF8.GetString(Data);

        this.SetData(Data);

    }
}
