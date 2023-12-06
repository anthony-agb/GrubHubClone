using GrubHubClone.Common.Authentication.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;

namespace GrubHubClone.Common.Authentication;

public static class AuthenticationDependencyInjection
{
    public static IServiceCollection AddAuth0JwtAuthentication(this IServiceCollection services, Action<AuthenticationConfiguration> configure)
    {
        AuthenticationConfiguration configuration = new AuthenticationConfiguration();
        configure(configuration);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            //options.Authority = "https://dev-qepl2m1rxmm3zt3v.us.auth0.com/";
            //options.Audience = "https://api.luxnex.net";
            options.Authority = configuration.Authority;
            options.Audience = configuration.Audience;
        });

        return services;
    }
}
