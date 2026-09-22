#region

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace PluginCore;

public interface IPlugin
{
    public void OnEnabled(IServiceProvider serviceProvider, Dictionary<string, IServiceProvider> dependencyServiceProviders);
    public void OnDisabled();

    public ValueTask OnDisabledAsync(CancellationToken cancellationToken = default)
    {
        OnDisabled();
        return ValueTask.CompletedTask;
    }

    public static abstract IServiceProvider GetServiceProvider();
}