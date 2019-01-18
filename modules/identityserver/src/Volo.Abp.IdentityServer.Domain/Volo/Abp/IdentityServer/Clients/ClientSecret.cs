﻿using System;
using IdentityServer4;
using JetBrains.Annotations;

namespace Volo.Abp.IdentityServer.Clients
{
    public class ClientSecret : Secret
    {
        public virtual Guid ClientId { get; protected set; }

        protected ClientSecret()
        {

        }

        protected internal ClientSecret(
            Guid clientId,
            [NotNull] string value,
            DateTime? expiration = null,
            string type = IdentityServerConstants.SecretTypes.SharedSecret,
            string description = null
            ) : base(
                  value, 
                  expiration, 
                  type, 
                  description)
        {
            ClientId = clientId;
        }

        public override object[] GetKeys()
        {
            return new object[] { ClientId, Type, Value };
        }
    }
}