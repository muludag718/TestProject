using TestProject.P3DFile.Helpers;
using TestProject.P3DFile.Interface;

namespace TestProject.P3DFile.Models;

public class UnknowFile(uint Typeid) : IChunkData
{

    public override uint TypeId { get; } = Typeid;
    public override string Name { get; } = "Unknow";

    public override void Deserialize(Stream input)
    {
        this.SetData(input.ReadBytes((int)Length));
    }

    public override string ToString()
    {
        return $"Unknow({TypeId})";
    }
}
