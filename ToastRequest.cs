using System;
using System.Collections.Generic;
using Avalonia.Controls.Notifications;

namespace PluginCore;

public sealed class ToastRequest
{
    public required string Header { get; init; }

    public required string Text { get; init; }

    public NotificationType NotificationType { get; init; } = NotificationType.Information;

    public TimeSpan? AutoCloseDelay { get; init; } = TimeSpan.FromSeconds(3);

    public bool ShowCloseButton { get; init; } = true;

    public bool ShowProgressBar { get; init; }

    public bool IsProgressIndeterminate { get; init; }

    /// <summary>
    /// Progress in the range of 0-100. Null means no fixed progress.
    /// </summary>
    public double? ProgressValue { get; init; }

    public IReadOnlyList<string> SelectionOptions { get; init; } = Array.Empty<string>();

    public string? SelectedOption { get; init; }

    public string? SelectionConfirmText { get; init; }

    public Action<string>? SelectionConfirmed { get; init; }

    public IReadOnlyList<ToastAction> Actions { get; init; } = Array.Empty<ToastAction>();

    public Action? ClickCallback { get; init; }

    public Action? CloseAction { get; init; }

    public bool CloseOnClick { get; init; } = true;
}

public sealed class ToastAction
{
    public required string Text { get; init; }

    public Action? Callback { get; init; }

    public bool? IsClose { get; init; }

    public bool CloseOnClick { get; init; } = true;

    public bool ShouldCloseOnClick => IsClose ?? CloseOnClick;

    public bool IsPrimary { get; init; }
}

public interface IToastProgressHandle
{
    public void Update(double? progress = null, string? text = null, string? header = null,
        bool? isIndeterminate = null);

    public void Complete(string? text = null, string? header = null, TimeSpan? autoCloseDelay = null);

    public void Fail(string? text = null, string? header = null, TimeSpan? autoCloseDelay = null);

    public void Close();
}
