﻿using System;
using Marvin.Modules;

namespace Marvin.Runtime.UserManagement.ClientConfigStore
{
    /// <summary>
    /// Interface for the user config store. 
    /// </summary>
    public interface IClientConfigStore : IConfiguredPlugin<ClientConfigStoreConfigBase>, IDisposable
    {
        /// <summary>
        /// Get a config model from a specific library.
        /// </summary>
        ClientConfigModel GetConfiguration(string clientId, string typeName);

        /// <summary>
        /// Save a user configuration model.
        /// </summary>
        bool SaveConfiguration(string clientId, ClientConfigModel configModel);
    }
}