using TestProject.P3DFile.Helpers;
using TestProject.P3DFile.Interface;

namespace TestProject.P3DFile.Models;

public class AudioFileHelper
{
    public static IChunkData Deserialize(Stream input)
    {
        long position = input.Position;

        var type = input.ReadValueU32();
        uint HeaderLength = input.ReadValueU32();
        var LengthData = input.ReadValueU32();
        _ = input.ReadValueU32();
        var labelLength = input.ReadValueU32();
        var Label = input.ReadString((int)labelLength + 1);

        if (Label == "AudioDialogueSubtitle\0")
        {
            var section = new AudioSubtitle();
            section.StartPosition = (uint)position;
            section.Length = LengthData;
            section.Deserialize(input);
            return section;

        }
        else if (Label == "AudioFile\0")
        {
            var section = new AudioRadp();
            section.StartPosition = (uint)position;
            section.Length = LengthData;
            section.Deserialize(input);
            return section;

        }
        else
        {
            var section = new UnknowFile(type);
            section.StartPosition = (uint)position;
            section.Length = LengthData;
            section.Deserialize(input);
            return section;

        }
    }
}

