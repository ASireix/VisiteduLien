using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Q Style", menuName = "Questionnaire/Style")]
public class QuestionnaireStyle : ScriptableObject
{
    public Color correctColor;
    public Color incorrectColor;
    public Color selectedColor;
    public Color unselectedColor;
}
