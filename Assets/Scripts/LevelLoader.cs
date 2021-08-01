/***
 * Asynchronically Load new scene by its number and play Fade animation.
 ***/
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public int sceneNum;

    public void changeScene()
    {
        StartCoroutine(LoadSceneAsync());
    }
    IEnumerator LoadSceneAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneNum);
        transition.Play("Fade_Start");
        while (!operation.isDone)
        {
            Debug.Log(operation.progress);

            yield return null;
        }
    }
}
