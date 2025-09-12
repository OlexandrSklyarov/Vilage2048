using System.Diagnostics;
using UnityEngine;

namespace Assets.App.Code.Runtime.Util
{
    public static class DebugLog
    {
        [Conditional("UNITY_EDITOR")]
        public static void Print(object obj) => UnityEngine.Debug.Log(obj);
        
        [Conditional("UNITY_EDITOR")]
        public static void Print(string msg) => UnityEngine.Debug.Log(msg);

        [Conditional("UNITY_EDITOR")]
        public static void PrintWarning(string msg) => UnityEngine.Debug.LogWarning(msg);

        [Conditional("UNITY_EDITOR")]
        public static void PrintError(string msg) => UnityEngine.Debug.LogError(msg);

        [Conditional("UNITY_EDITOR")]
        public static void PrintColor(string msg, Color color)
        {
            Print($"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{msg}</color>");
        }

        [Conditional("UNITY_EDITOR")]
        public static void PrintGreen(string msg) => PrintColor(msg, Color.green);

        [Conditional("UNITY_EDITOR")]
        public static void PrintCyan(string msg) => PrintColor(msg, Color.cyan);

        [Conditional("UNITY_EDITOR")]
        public static void PrintMagenta(string msg) => PrintColor(msg, Color.magenta);

        [Conditional("UNITY_EDITOR")]
        public static void PrintOrange(string msg) => PrintColor(msg, Color.orange);

        [Conditional("UNITY_EDITOR")]
        public static void PrintBlue(string msg) => PrintColor(msg, Color.blue);

        [Conditional("UNITY_EDITOR")]
        public static void PrintViolet(string msg) => PrintColor(msg, Color.violet);
    }
}