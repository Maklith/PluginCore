#region

using System;
using System.Collections.Generic;

#endregion

namespace PluginCore;

public interface IPlugin
{
    public void OnEnabled(IServiceProvider serviceProvider, Dictionary<string, IServiceProvider> dependencyServiceProviders);
    public void OnDisabled();

    public static abstract IServiceProvider GetServiceProvider();
}