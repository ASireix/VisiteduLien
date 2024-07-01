using UnityEditor;
using UnityEngine;


public class ScriptableObjectIdAttribute : PropertyAttribute
{
#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ScriptableObjectIdAttribute))]
public class ScriptableObjectIdDrawer: PropertyDrawer{
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false;

            Object owner = property.serializedObject.targetObject;
            string unityManagedGuid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(owner));

            if (property.stringValue != unityManagedGuid){
                property.stringValue = unityManagedGuid;
            }
            EditorGUI.PropertyField(position,property,label,true);
            GUI.enabled = true;
        }
    }
#endif
}
