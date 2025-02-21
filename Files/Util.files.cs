﻿using System;
using System.IO;
using UnityEngine;

public static partial class Util
{
    public static readonly string
        app_path = Directory.GetCurrentDirectory(),
        home_path = Path.Combine(app_path, "Home");

    public static DirectoryInfo HOME_DIR => home_path.GetDir(true);
    public static DirectoryInfo STREAM_DIR => Application.streamingAssetsPath.GetDir(true);

    //----------------------------------------------------------------------------------------------------------

    public static long SecondsSinceLastWriteTime(this FileInfo file) => (DateTime.UtcNow - file.LastWriteTimeUtc).Ticks / TimeSpan.TicksPerSecond;

    public static DirectoryInfo GetDir(this string path, in bool force = true)
    {
        DirectoryInfo dir = new(path);
        if (force && !dir.Exists)
        {
            dir.Create();
            Debug.Log($"Created directory at: \"{dir.FullName}\"".ToSubLog());
        }
        return dir;
    }

    [Obsolete] public static string TypeToExtension_OLD(this Type type) => type.FullName.Replace(".", string.Empty).Replace('+', '_');
    public static string TypeToExtension(this Type type) => "." + type.FullName.Replace(".", string.Empty).Replace('+', '_');
    public static string TypeToPath(this Type type) => type.FullName.Replace('.', Path.PathSeparator).Replace('+', '_');
}