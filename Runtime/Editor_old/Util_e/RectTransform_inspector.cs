﻿#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace _EDITOR_
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(RectTransform))]
    public class RectTransform_inspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            RectTransform rT = (RectTransform)target;
            Transform T = rT;

            if (GUILayout.Button(nameof(Util_e.FillParent)))
                rT.FillParent();
            if (GUILayout.Button(nameof(Util.GetPath)))
                Debug.Log(rT.GetPath(true));
            if (GUILayout.Button(nameof(Util_e.LogTypes)))
                rT.LogTypes();
        }
    }
}
#endif