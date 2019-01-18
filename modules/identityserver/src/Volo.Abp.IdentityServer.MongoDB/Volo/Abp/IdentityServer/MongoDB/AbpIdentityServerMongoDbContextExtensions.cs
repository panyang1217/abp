﻿using System;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.IdentityServer.Grants;
using Volo.Abp.IdentityServer.IdentityResources;
using Volo.Abp.MongoDB;

namespace Volo.Abp.IdentityServer.MongoDB
{
    public static class AbpIdentityServerMongoDbContextExtensions
    {
        public static void ConfigureIdentityServer(
            this IMongoModelBuilder builder,
            Action<IdentityServerMongoModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new IdentityServerMongoModelBuilderConfigurationOptions();

            optionsAction?.Invoke(options);

            builder.Entity<ApiResource>(b =>
            {
                b.CollectionName = options.CollectionPrefix + "ApiResources";
            });

            builder.Entity<Client>(b =>
            {
                b.CollectionName = options.CollectionPrefix + "Clients";
            });
            builder.Entity<IdentityResource>(b =>
            {
                b.CollectionName = options.CollectionPrefix + "IdentityResources";
            });

            builder.Entity<PersistedGrant>(b =>
            {
                b.CollectionName = options.CollectionPrefix + "PersistedGrants";
            });
        }
    }
}
