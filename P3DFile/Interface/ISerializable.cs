namespace TestProject.P3DFile.Interface;

public interface ISerializable<T>
{


    public T Deserialize(Stream input);
}
