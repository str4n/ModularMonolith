namespace ModularMonolith.Shared.Infrastructure.Modules.Serialization;

internal interface IModuleSerializer
{
    byte[] Serialize<T>(T value);
    T Deserialize<T>(byte[] value);
    object Deserialize(byte[] value, Type type);
}