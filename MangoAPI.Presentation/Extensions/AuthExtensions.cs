﻿using System.Text;
using MangoAPI.Domain.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace MangoAPI.Presentation.Extensions;

public static class AuthExtensions
{
    public static IServiceCollection AddAppAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.DefaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .Build();
        });
        return services;
    }

    public static IServiceCollection AddAppAuthentication(this IServiceCollection services)
    {
        var tokenKey = EnvironmentConstants.MangoJwtSignKey;
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, configure =>
            {
                configure.RequireHttpsMetadata = false;
                configure.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = EnvironmentConstants.MangoJwtIssuer,
                    ValidateIssuer = true,
                    ValidAudience = EnvironmentConstants.MangoJwtAudience,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = signingKey,
                    ValidateIssuerSigningKey = true,
                };
            });

        return services;
    }
}