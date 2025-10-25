namespace TestProject.P3DFile.Interface;

public interface ISerializableAsync<T>
{
    public Task SerializeAsync(Stream output);

    public Task<T> DeserializeAsync(Stream input);
}
