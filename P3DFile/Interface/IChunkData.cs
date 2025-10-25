using System.ComponentModel.DataAnnotations;

namespace TestProject.P3DFile.Interface;

public abstract class IChunkData
{
    public abstract string Name { get; }
    public object? Data { get; set; }
    public Type? DataType { get; set; }

    public virtual uint TypeId { get; }
    public uint? HeaderSize { get; set; }
    public uint Length { get; set; }
    public List<IChunkData> Children { get; } = [];

    public uint StartPosition { get; set; }

    public void SetData<T>(T Data)
    {
        this.Data = Data;
        DataType = Data?.GetType();
    }
    public abstract void Deserialize(Stream input);
}