using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionnaireLibre : Minigame
{
    public QuestionLibre questionlibre;
    public override void StartMiniGame()
    {
        questionlibre.StartQuestionLibre();
    }


}
