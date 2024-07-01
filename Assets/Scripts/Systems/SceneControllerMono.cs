using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneControllerMono : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] string mainMenuSceneName;
    [SerializeField] string mainGameSceneName;
    [SerializeField] DialogueUpdateManager dialogueUpdateManager;
    [SerializeField] GameObject[] loadingScreenObjects;
    [SerializeField] GameObject[] mainCanvasObjects;
    [SerializeField] Image progressFill;
    AsyncOperation operation;

    public IEnumerator LoadGame()
    {
        for (int i = 0; i < loadingScreenObjects.Length; i++)
        {
            loadingScreenObjects[i].SetActive(true);
        }

        for (int i = 0; i < mainCanvasObjects.Length; i++)
        {
            mainCanvasObjects[i].SetActive(false);
        }
        progressFill.fillAmount = 0f;
        operation = SceneManager.LoadSceneAsync(mainGameSceneName);
        operation.allowSceneActivation = false;
        bool loaded = false;
        while (!loaded)
        {
            progressFill.fillAmount = Mathf.Clamp01(operation.progress / 0.9f);
            if (operation.progress >= 0.9f){
                loaded = true;
            }
            yield return new WaitForEndOfFrame();
        }

        yield return StartCoroutine(dialogueUpdateManager.UpdateFiles());
        /*
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(mainGameSceneName));
        SceneManager.UnloadSceneAsync(currentScene);
        */
    }

    public IEnumerator ActivateScene()
    {
        operation.allowSceneActivation = true;
        yield return new WaitForEndOfFrame();
    }

}
