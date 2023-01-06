using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace InexperiencedDeveloper.Utils.Editor
{
    [CustomEditor(typeof(LayerAdder))]
    public class LayerAdderEditor : UnityEditor.Editor
    {
        private SerializedObject m_TagManager => new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        private SerializedProperty m_Layers => m_TagManager.FindProperty("layers");

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            GUI.backgroundColor = Color.green;
            if(GUILayout.Button("Add Required Layers"))
            {
                IDLayers[] layers = System.Enum.GetValues(typeof(IDLayers)) as IDLayers[];
                foreach (IDLayers l in layers)
                {
                    CreateLayer(l.ToString());
                }
            }
        }

        private Dictionary<string, int> GetAllLayers()
        {
            int layerSize = m_Layers.arraySize;
            Dictionary<string, int> layerDict = new Dictionary<string, int>();
            for(int i = 0; i < layerSize; i++)
            {
                SerializedProperty l = m_Layers.GetArrayElementAtIndex(i);
                string layerName = l.stringValue;
                if (!string.IsNullOrEmpty(layerName))
                    layerDict.Add(layerName, i);
            }
            return layerDict;
        }

        private void CreateLayer(string name)
        {
            bool success = false;
            Dictionary<string, int> dict = GetAllLayers();
            if (dict.ContainsKey(name))
            {
                Debug.LogWarning($"Layer ({name}) already exists.");
                return;
            }
            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty layers = tagManager.FindProperty("layers");
            for (int i = 0; i < 31; i++)
            {
                SerializedProperty l = layers.GetArrayElementAtIndex(i);
                if (string.IsNullOrEmpty(l.stringValue) && i >= 8)
                {
                    l.stringValue = name;
                    success = tagManager.ApplyModifiedProperties(); //Saving changes
                    Debug.Log($"Successfully created layer: {name} at index {i} {success}");
                    break;
                }
            }
            if(!success)
                Debug.LogError($"Could not create layer {name}");
        }

        //void CreateLayer(string name)
        //{
        //    bool Success = false;
        //    Dictionary<string, int> dic = GetAllLayers();

        //    if (!dic.ContainsKey(name))
        //    {
        //        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        //        SerializedProperty layers = tagManager.FindProperty("layers");

        //        for (int i = 0; i < 31; i++)
        //        {
        //            SerializedProperty element = layers.GetArrayElementAtIndex(i);
        //            if (string.IsNullOrEmpty(element.stringValue) && i >= 8)
        //            {
        //                element.stringValue = name;

        //                tagManager.ApplyModifiedProperties(); //save changes
        //                Success = true;
        //                Debug.Log(i.ToString() + " layer created: " + name);
        //                break;
        //            }
        //        }

        //        if (!Success)
        //        {
        //            Debug.Log("could not create layer");
        //        }
        //    }
        //    else
        //    {
        //        Debug.Log("layer already exists: " + name);
        //    }
        //}
    }
}

