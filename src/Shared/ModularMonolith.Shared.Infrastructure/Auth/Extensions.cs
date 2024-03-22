using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ModularMonolith.Shared.Abstractions.Auth;
using ModularMonolith.Shared.Abstractions.Storage;
using ModularMonolith.Shared.Infrastructure.Storage;

namespace ModularMonolith.Shared.Infrastructure.Auth;

internal static class Extensions
{
   private const string SectionName = "Auth";

   public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
   {
      services.Configure<AuthOptions>(configuration.GetSection(SectionName));

      var options = configuration.GetOptions<AuthOptions>(SectionName);

      services.AddSingleton<IAuthenticator, Authenticator>();
      services.AddScoped<ITokenStorage, HttpContextTokenStorage>();
      
      services
         .AddAuthentication(opt =>
         {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
         }).AddJwtBearer(opt =>
         {
            opt.Authority = options.Authority;
            opt.Audience = options.Audience;
            opt.MetadataAddress = options.MetadataAddress;
            opt.SaveToken = options.SaveToken;
            opt.RefreshOnIssuerKeyNotFound = options.RefreshOnIssuerKeyNotFound;
            opt.RequireHttpsMetadata = options.RequireHttpsMetadata;
            opt.IncludeErrorDetails = options.IncludeErrorDetails;
            opt.TokenValidationParameters = new TokenValidationParameters
            {
               RequireAudience = options.RequireAudience,
               ValidIssuer = options.ValidIssuer,
               ValidateAudience = options.ValidateAudience,
               ValidateIssuer = options.ValidateIssuer,
               ValidateLifetime = options.ValidateLifetime,
               ValidateTokenReplay = options.ValidateTokenReplay,
               ValidateIssuerSigningKey = options.ValidateIssuerSigningKey,
               SaveSigninToken = options.SaveSigninToken,
               RequireExpirationTime = options.RequireExpirationTime,
               RequireSignedTokens = options.RequireSignedTokens,
               ClockSkew = TimeSpan.Zero,
               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SigningKey))
            };
         });

      return services;
   }
}