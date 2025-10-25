
using System.Runtime.Serialization;
using TestProject.P3DFile.Models;

namespace TestProject.P3DFile.Interface;

public abstract class IBaseParse : ISerializable<IBaseParse>
{
    public abstract uint TypeId { get; }

    public uint TotalSize { get; set; }

    public uint StartPosition { get; set; }


    public List<IBaseParse> Children { get; } = [];

    public abstract IBaseParse Deserialize(Stream input);


}
