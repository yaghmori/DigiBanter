using Newtonsoft.Json;

public class NewtonsoftJsonSerializer : IJsonSerializer
{
    private readonly JsonSerializerSettings _options;

    public NewtonsoftJsonSerializer(JsonSerializerSettings options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    // Synchronous serialization
    public string Serialize<T>(T obj)
    {
        if (obj == null) throw new ArgumentNullException(nameof(obj));

        try
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj, _options);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Serialization failed.", ex);
        }
    }

    // Synchronous deserialization with special handling for JsonPatchDocument
    public T? Deserialize<T>(string json)
    {
        try
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json, _options);

        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Deserialization failed.", ex);
        }
    }
    public T DeepCopy<T>(T obj)
    {
        var json = JsonConvert.SerializeObject(obj);
        return JsonConvert.DeserializeObject<T>(json);
    }

}
