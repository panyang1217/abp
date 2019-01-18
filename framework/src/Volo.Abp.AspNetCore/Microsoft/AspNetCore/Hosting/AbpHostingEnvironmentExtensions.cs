<<<<<<< HEAD
﻿using Microsoft.Extensions.Configuration;

namespace Microsoft.AspNetCore.Hosting
{
    public static class AbpHostingEnvironmentExtensions
    {
        public static IConfigurationRoot BuildConfiguration(this IHostingEnvironment env, AbpAspNetCoreConfigurationOptions options = null)
        {
            options = options ?? new AbpAspNetCoreConfigurationOptions();

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile(options.FileName + ".json", optional: true, reloadOnChange: true)
                .AddJsonFile($"{options.FileName}.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                if (options.UserSecretsId != null)
                {
                    builder.AddUserSecrets(options.UserSecretsId);
                }
                else if (options.UserSecretsAssembly != null)
                {
                    builder.AddUserSecrets(options.UserSecretsAssembly, true);
                }
            }

            return builder.Build();
        }
    }
}
=======
﻿using System;
using Microsoft.Extensions.Configuration;

namespace Microsoft.AspNetCore.Hosting
{
    public static class AbpHostingEnvironmentExtensions
    {
        public static IConfigurationRoot BuildConfiguration(
            this IHostingEnvironment env,
            ConfigurationBuilderOptions options = null)
        {
            options = options ?? new ConfigurationBuilderOptions();

            if (options.BasePath.IsNullOrEmpty())
            {
                options.BasePath = env.ContentRootPath;
            }

            if (options.EnvironmentName.IsNullOrEmpty())
            {
                options.EnvironmentName = env.EnvironmentName;
            }

            return ConfigurationHelper.BuildConfiguration(options);
        }
    }
}
>>>>>>> upstream/master
