using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Content.Scripts.Editor
{
    [CustomEditor(typeof(SaveDataSO))]
    public class PlayerDataObjectEditor : UnityEditor.Editor
    {
        private SaveDataSO so;
        
        private void OnEnable()
        {
            so = target as SaveDataSO;
            so.Load();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (EditorUtility.IsDirty(so))
            {
                if (GUILayout.Button("Save"))
                {
                    so.Save();
                    AssetDatabase.SaveAssetIfDirty(so);
                }
            }

            if (GUILayout.Button("Reset"))
            {
                JsonUtility.FromJsonOverwrite(JsonUtility.ToJson(CreateInstance<SaveDataSO>()), so);
                EditorUtility.SetDirty(so);
            }
            if (GUILayout.Button("Delete"))
            {
                JsonUtility.FromJsonOverwrite(JsonUtility.ToJson(CreateInstance<SaveDataSO>()), so);
                so.Delete();
                EditorUtility.SetDirty(so);
            }
        }
    }
}
