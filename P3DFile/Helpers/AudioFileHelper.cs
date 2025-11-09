using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.P3DFile.Interface;
using TestProject.P3DFile.Models;

namespace TestProject.P3DFile.Helpers
{
    public class AudioFileHelper
    {

        public static IChunkData GetChuckData(Stream input)
        {

            long position = input.Position;

            var type = input.ReadValueU32();
            uint HeaderLength = input.ReadValueU32();
            var LengthData = input.ReadValueU32();
            _ = input.ReadValueU32();
            var labelLength = input.ReadValueU32();
            var Label = input.ReadString((int)labelLength + 1);
            return Label switch
            {
                "AudioDialogueSubtitle\0" => new AudioSubtitle(),
                "AudioFile\0" => new AudioRadp(),
                _ => new UnknowFile(type),
            };
        }
    }
}
