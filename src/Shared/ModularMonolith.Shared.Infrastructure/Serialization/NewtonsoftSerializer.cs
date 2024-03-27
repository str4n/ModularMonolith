using System.Text.Json;
using System.Text.Json.Serialization;
using ModularMonolith.Shared.Abstractions.Serialization;
using Newtonsoft.Json;

namespace ModularMonolith.Shared.Infrastructure.Serialization;

internal sealed class NewtonsoftSerializer : IJsonSerializer
{
    private static readonly JsonSerializerSettings Settings = new()
    {
        //
    };
    
    public string Serialize<T>(T value)
        => JsonConvert.SerializeObject(value);

    public T Deserialize<T>(string value)
        => JsonConvert.DeserializeObject<T>(value);

    public object Deserialize(string value, Type type)
        => JsonConvert.DeserializeObject(value, type);
}