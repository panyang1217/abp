﻿using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.IdentityServer.Clients
{
    public interface IClientRepository : IBasicRepository<Client, Guid>
    {
        Task<Client> FindByCliendIdAsync(
            [NotNull] string clientId,
            bool includeDetails = true,
            CancellationToken cancellationToken = default
        );
    }
}