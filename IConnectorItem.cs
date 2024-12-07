using System;
using System.Collections.Generic;
using PluginCore;

namespace Core.SDKs.CustomScenario;

public interface IConnectorItem
{
    bool SelfInputAble { get; set; }
    string TypeName { get; set; }
    string Title { get; set; }

    List<string>? Interfaces { get; set; }
    bool isPluginInputConnector { get; set; }

    INodeInputConnector PluginInputConnector { get; set; }

    /// <inheritdoc cref="ConnectorItem._inputObject"/>
    object? InputObject { get; set; }

    /// <inheritdoc cref="ConnectorItem._isConnected"/>
    bool IsConnected { get; set; }

    /// <inheritdoc cref="ConnectorItem._isNotUsed"/>
    bool IsNotUsed { get; set; }

    /// <inheritdoc cref="ConnectorItem._isOut"/>
    bool IsOut { get; set; }

    /// <inheritdoc cref="ConnectorItem._isSelf"/>
    bool IsSelf { get; set; }
}