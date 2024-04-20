using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace dotnet_sk_plugins.Plugins.Plugin.Time;

public sealed class TimePlugin
{
    [KernelFunction, Description("获取今天的日期")]
    public static string GetToday()
    {
        return DateTime.Today.ToString("yyyy-M-d dddd");
    }
}