
using ProtoType2.Tools.RadpToWav;
using TestProject.P3DFile.Helpers;
using TestProject.P3DFile.Interface;

namespace TestProject.P3DFile.Models;

public class AudioRadp : IChunkData
{
    public override string Name => "AudioFile\0";
    public string LabelKey { get; private set; } = string.Empty;

    public byte[] RawData { get; set; } = [];
    public override uint TypeId => 4261412864u;

    public override void Deserialize(Stream input)
    {
        var unk2 = input.ReadValueU32();
        var labelSize = input.ReadValueU32();
        var labelAudioTitle = input.ReadString((int)labelSize + 1);

        //Audio key kısmı KEY:5\4\54e394b1
        labelSize = input.ReadValueU32();
        LabelKey = input.ReadString((int)labelSize + 1);

        input.ReadValueU32(); // Padding 4 byte

        labelSize = input.ReadValueU32();
        input.ReadString((int)labelSize + 1); // radp kelimesi

        labelSize = input.ReadValueU32();
        input.ReadString((int)labelSize + 1); //Dialogue kelimesi

        RawData = new byte[Length];
        input.Read(RawData, 0, (int)Length);

        this.SetData(RawData);
    }

    public List<short> ToWavData()
    {
        if (RawData.Length <= 0) return [];
        return RadpAudioFile.ToWav(RawData);
    }

}
