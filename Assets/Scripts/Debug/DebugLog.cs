using System.Diagnostics;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

public static class DebugEditor
{
    // ReSharper disable Unity.PerformanceAnalysis
    [Conditional("UNITY_EDITOR")]
    public static void Log(object message, Object context = null)
    {
        Debug.Log(message, context);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    [Conditional("UNITY_EDITOR")]
    public static void LogWarning(object message, Object context = null)
    {
        Debug.LogWarning(message, context);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    [Conditional("UNITY_EDITOR")]
    public static void LogError(object message, Object context = null)
    {
        Debug.LogError(message, context);
    }
}