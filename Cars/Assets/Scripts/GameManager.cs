using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;   
    private void Awake()
    {
        if (instance && instance != this)
            Destroy(instance.gameObject);

        instance = this;

        DontDestroyOnLoad(this);

    }

    public Animator transitionAnimatorController;

    public void LoadScene(string name)
    {
        instance.StartCoroutine(LoadSceneWithTransition(name));
    }

    [Space]
    public int currentLevel;

    public void LoadLevel()
    {
        instance.StartCoroutine
            (LoadSceneWithTransition("Level_" + instance.currentLevel.ToString()));
    }

    IEnumerator LoadSceneWithTransition(string name)
    {
        instance.transitionAnimatorController.SetBool("On", true);

        yield return new WaitForSeconds(.8f);

        // Start loading the scene
        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync(name, LoadSceneMode.Single);
        // Wait until the level finish loading
        while (!asyncLoadLevel.isDone)
            yield return null;

        //yield return new WaitForSeconds(.5f);
        instance.transitionAnimatorController.SetBool("On", false);

        // Wait a frame so every Awake and Start method is called
        yield return new WaitForEndOfFrame();


    }
}
