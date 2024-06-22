﻿using UnityEngine;

public static partial class Util
{
    public static string GetPath(this Transform transform) => GetPath(transform, transform.root);
    public static string GetPath(this Transform transform, in Transform root)
    {
        string res = transform.name;

        while (transform.parent && transform.parent != root)
        {
            transform = transform.parent;
            res = transform.name + "/" + res;
        }

        return res;
    }

    public static void NormalizeChildrenScales(this Transform transform)
    {
        foreach (Transform t in transform.GetComponentsInChildren<Transform>(true))
            t.localScale = Vector3.one;
    }

    public static void CleanAll(this Transform transform)
    {
        for (int i = 0; i < transform.childCount; ++i)
            Destroy(transform.GetChild(i).gameObject);
    }

    public static void Clean(this Transform transform, in string path)
    {
        Transform T = transform.Find(path);
        if (T != null)
            Destroy(T.gameObject);
    }

    public static Transform ForceFindTransform(this string path)
    {
        string[] splits = path.Split('/');
        Transform root;

        GameObject go = GameObject.Find(splits[0]);
        if (go == null)
            root = new GameObject(splits[0]).transform;
        else
            root = go.transform;

        if (splits.Length == 1)
            return root;

        splits = splits[1..];
        return ForceFind(root, splits);
    }

    public static Transform ForceFind(this Transform root, params string[] splits)
    {
        Transform tfm = root.Find(splits[0]);

        if (tfm == null)
        {
            tfm = new GameObject(splits[0]).transform;
            tfm.SetParent(root, false);
        }

        if (splits.Length == 1)
            return tfm;
        else
            return ForceFind(tfm, splits[1..]);
    }

    public static void DestroyAllByType<ComponentType>(this GameObject gameObject) where ComponentType : Component
    {
        foreach (ComponentType component in gameObject.GetComponentsInChildren<ComponentType>(true))
            Object.Destroy(component.gameObject);
    }
}