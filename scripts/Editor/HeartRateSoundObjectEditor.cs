#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HeartRateSoundObject))]
public class HeartRateSoundObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        HeartRateSoundObject script = (HeartRateSoundObject)target;

        EditorGUILayout.LabelField("Menu toggles", EditorStyles.boldLabel);
        script.toggleOnOff = EditorGUILayout.Toggle("Sound On/off (+1 synced bits)", script.toggleOnOff);

        EditorGUILayout.Space();
        if (GUILayout.Button("Get Sound Object"))
        {

            string selectedPrefabName = "heartbeat/Heartbeat";
            
            if (script.toggleOnOff) {
                selectedPrefabName += "_Toggle";
            }

            GameObject selectedPrefab = (GameObject)Resources.Load(selectedPrefabName);

            MonoBehaviour thisComponent = (MonoBehaviour)target;
            GameObject instance = Instantiate(selectedPrefab, thisComponent.gameObject.transform);
            Selection.activeGameObject = instance;
            instance.name = "Heartbeat Sound";

        }

        EditorUtility.SetDirty(script);
    }
}

#endif