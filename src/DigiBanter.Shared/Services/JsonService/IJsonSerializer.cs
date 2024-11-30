public interface IJsonSerializer
{
    // Synchronous methods
    T? Deserialize<T>(string json);
    string Serialize<T>(T obj);
    T DeepCopy<T>(T obj);
}
