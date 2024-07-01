using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Questionnaire))]
public class QuestionnaireEditor : Editor
{
    
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Questionnaire _target = (Questionnaire)target;

        if (GUILayout.Button("Get Questions")){
            _target.GetAllQuestions();
            OnButtonClicked(_target);
        }

        if (GUILayout.Button("Clear All Questions")){
            _target.ClearQuestions();
            OnButtonClicked(_target);
        }

        if (GUILayout.Button("Enter configuration")){
            _target.EnterConfiguration();
            OnButtonClicked(_target);
        }

        if (GUILayout.Button("Exit Configuration")){
            _target.ExitConfiguration();
            OnButtonClicked(_target);
        }

        if (GUILayout.Button("Update Positions from template")){
            _target.UpdateQPosFromTempalte();
            OnButtonClicked(_target);
        }

        if (GUILayout.Button("Update Smoke VFX")){
            _target.UpdateVFXs();
            OnButtonClicked(_target);
        }

        
    }

    void OnButtonClicked(Object o){
        PrefabUtility.RecordPrefabInstancePropertyModifications(o);
    }
}
