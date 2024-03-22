using ModularMonolith.Shared.Abstractions.Auth;

namespace ModularMonolith.Shared.Abstractions.Storage;

public interface ITokenStorage
{
    void Set(JsonWebToken token);
    JsonWebToken Get();
}