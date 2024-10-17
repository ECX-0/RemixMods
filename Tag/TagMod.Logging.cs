﻿using System;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace TagMod
{
    public partial class TagMod
    {
        private static string TrimCaller(string callerFile) { return (callerFile = callerFile.Substring(Mathf.Max(callerFile.LastIndexOf(Path.DirectorySeparatorChar), callerFile.LastIndexOf(Path.AltDirectorySeparatorChar)) + 1)).Substring(0, callerFile.LastIndexOf('.')); }
        private static string LogTime() { return ((int)(Time.time * 1000)).ToString(); }
        private static string LogDOT() { return DateTime.Now.ToUniversalTime().TimeOfDay.ToString().Substring(0, 8); }
        public static void Debug(object data, [CallerFilePath] string callerFile = "", [CallerMemberName] string callerName = "")
        {
            instance.Logger.LogInfo($"{LogDOT()}|{LogTime()}|{TrimCaller(callerFile)}.{callerName}:{data}");
        }
        public static void DebugMe([CallerFilePath] string callerFile = "", [CallerMemberName] string callerName = "")
        {
            instance.Logger.LogInfo($"{LogDOT()}|{LogTime()}|{TrimCaller(callerFile)}.{callerName}");
        }
        public static void Error(object data, [CallerFilePath] string callerFile = "", [CallerMemberName] string callerName = "")
        {
            instance.Logger.LogError($"{LogDOT()}|{LogTime()}|{TrimCaller(callerFile)}.{callerName}:{data}");
        }
        public static void Stacktrace()
        {
            var stacktrace = Environment.StackTrace;
            stacktrace = stacktrace.Substring(stacktrace.IndexOf('\n') + 1);
            stacktrace = stacktrace.Substring(stacktrace.IndexOf('\n'));
            instance.Logger.LogInfo(stacktrace);
        }
    }
}
