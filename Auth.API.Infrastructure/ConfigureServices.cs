using Auth.API.Abstractions.Interfaces;
using Auth.API.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.API.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<IVerificationTokenService, VerificationTokenService>();
            services.AddScoped<IEmailVerificationService, EmailVerificationService>();
            services.AddScoped<IPasswordResetService, PasswordResetService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IAvatarService, AvatarService>();
            services.AddScoped<IImageService, ImgurService>();

            return services;
        }
    }
}
