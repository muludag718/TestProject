using Newtonsoft.Json;
using TestProject.P3DFile.Helpers;
using TestProject.P3DFile.Interface;

namespace TestProject.P3DFile.Models;

public class AudioSubtitle : IChunkData
{
    public override string Name => "AudioDialogueSubtitle";

    public override uint TypeId => 4261412864;

    public Dictionary<string, string> SubTitles = [];

    public override void Deserialize(Stream input)
    {
        _ = input.ReadValueU32();//Unknow
        var labelLength = input.ReadValueU32();
        var labelAudioTitle = input.ReadString((int)labelLength + 1);

        input.Position += 19L;

        var unk2 = input.ReadValueU32();
        var unk3 = input.ReadValueU32();
        var labelSize = input.ReadValueU32();
        input.ReadString((int)labelSize + 1); // LabelGerman

        var subLen = input.ReadValueU32();
        var German = input.ReadString((int)subLen + 1, encoding: System.Text.Encoding.UTF8);

        input.Position += 9L;

        labelSize = input.ReadValueU32();
        input.ReadString((int)labelSize + 1); // LabelEnglish

        subLen = input.ReadValueU32();
        var English = input.ReadString((int)subLen + 1, encoding: System.Text.Encoding.UTF8);

        input.ReadBytes(14); //Padding

        SubTitles.Add("AudioTitle", labelAudioTitle);
        SubTitles.Add("German", German);
        SubTitles.Add("English", English);

        this.SetData(SubTitles);

    }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(SubTitles);
    }

    public object ToJsonObject() => SubTitles;

}
