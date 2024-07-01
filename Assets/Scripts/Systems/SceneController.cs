using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Scene Controller",menuName = "Scene Data/Scene Controller")]
public class SceneController : ScriptableObject
{
    public void SwitchSceneTo(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadSceneAsync(){
        
    }

    public IEnumerator LoadSceneAsyncCoro(string sceneName){

        yield return null;
    }
}
