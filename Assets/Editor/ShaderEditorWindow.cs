using UnityEngine;
using UnityEditor;

public class ShaderEditorWindow : EditorWindow
{
    private Material targetMat;
    private Vector2 scroll;

    [MenuItem("Tools/Shader Editor")]
    public static void OpenWindow()
    {
        GetWindow<ShaderEditorWindow>("Shader Editor");
    }

    private void OnGUI()
    {
        EditorGUILayout.Space();
        targetMat = (Material)EditorGUILayout.ObjectField("Target Material", targetMat, typeof(Material), false);
        if (targetMat == null) return;

        Shader shader = targetMat.shader;
        int propCount = ShaderUtil.GetPropertyCount(shader);

        // 스크롤 영역
        scroll = EditorGUILayout.BeginScrollView(scroll);
        for (int i = 0; i < propCount; i++)
        {
            string propName = ShaderUtil.GetPropertyName(shader, i);
            string display = ShaderUtil.GetPropertyDescription(shader, i);
            ShaderUtil.ShaderPropertyType type = ShaderUtil.GetPropertyType(shader, i);

            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField(display, EditorStyles.boldLabel);

            switch (type)
            {
                case ShaderUtil.ShaderPropertyType.Color:
                    {
                        Color c = targetMat.GetColor(propName);
                        Color nc = EditorGUILayout.ColorField(c);
                        if (nc != c) targetMat.SetColor(propName, nc);
                        break;
                    }
                case ShaderUtil.ShaderPropertyType.Vector:
                    {
                        Vector4 v = targetMat.GetVector(propName);
                        Vector4 nv = EditorGUILayout.Vector4Field("", v);
                        if (nv != v) targetMat.SetVector(propName, nv);
                        break;
                    }
                case ShaderUtil.ShaderPropertyType.Float:
                case ShaderUtil.ShaderPropertyType.Range:
                    {
                        float f = targetMat.GetFloat(propName);
                        if (type == ShaderUtil.ShaderPropertyType.Range)
                        {
                            float min = ShaderUtil.GetRangeLimits(shader, i, 1);
                            float max = ShaderUtil.GetRangeLimits(shader, i, 2);
                            float nf = EditorGUILayout.Slider(f, min, max);
                            if (!Mathf.Approximately(nf, f)) targetMat.SetFloat(propName, nf);
                        }
                        else
                        {
                            float nf = EditorGUILayout.FloatField(f);
                            if (!Mathf.Approximately(nf, f)) targetMat.SetFloat(propName, nf);
                        }
                        break;
                    }
                case ShaderUtil.ShaderPropertyType.TexEnv:
                    {
                        Texture t = targetMat.GetTexture(propName);
                        Texture nt = (Texture)EditorGUILayout.ObjectField(t, typeof(Texture), false);
                        if (nt != t) targetMat.SetTexture(propName, nt);
                        // 옵션: UV 스케일/오프셋 편집
                        break;
                    }
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(4);
        }
        EditorGUILayout.EndScrollView();
    }
}
