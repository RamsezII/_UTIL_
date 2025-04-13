﻿using System.IO;
using System.Linq;

partial class Util
{
    public static string AutoPath(this string path, in string working_dir)
    {
        bool rooted = Path.IsPathRooted(path);
        string full_path = path;

        if (!rooted)
        {
            full_path = Path.GetFullPath(Path.Combine(working_dir, full_path));

            if (full_path.StartsWith(working_dir))
                full_path = Path.GetRelativePath(working_dir, full_path);
        }

        full_path = full_path.Replace("\\", "/");

        return full_path;
    }

    public static string SafeRootedPath(this string path, in string working_dir)
    {
        try
        {
            bool rooted = Path.IsPathRooted(path);
            string full_path = path;

            if (!rooted)
                full_path = Path.GetFullPath(Path.Combine(working_dir, full_path));

            return full_path.Replace("\\", "/");
        }
        catch
        {
            return path;
        }
    }

    public static bool IsProperlyQuoted(this string input)
    {
        if (string.IsNullOrEmpty(input) || input.Length < 2)
            return false;

        char first = input[0];
        char last = input[^1];

        if ((first == '\'' || first == '"') && first == last)
        {
            string inner = input[1..^1];
            return !inner.Contains(first); // pas de conflit à l'intérieur
        }

        return false;
    }

    public static string QuoteStringSafely(this string input)
    {
        bool containsSingle = input.Contains('\'');
        bool containsDouble = input.Contains('\"');

        if (!containsSingle)
            return $"'{input}'";
        else if (!containsDouble)
            return $"\"{input}\"";
        else
            return $"\"{input.Replace("\"", "\\\"")}\""; // échappement JSON-like
    }

    public static string QuotePathIfNeeded(this string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return "\"\"";

        // Ne rien faire si déjà bien quoté
        if ((path.StartsWith("\"") && path.EndsWith("\"")) ||
            (path.StartsWith("'") && path.EndsWith("'")))
            return path;

        // Si le chemin contient des espaces ou caractères spéciaux, on quote
        if (path.Any(c => char.IsWhiteSpace(c) || c == ';' || c == '&' || c == '|'))
        {
            // Choix intelligent de guillemet selon contenu
            if (path.Contains('"') && !path.Contains('\''))
                return $"'{path}'"; // utiliser ' si " est dedans
            else
                return $"\"{path.Replace("\"", "\\\"")}\""; // échapper les " si nécessaire
        }

        return path;
    }
}