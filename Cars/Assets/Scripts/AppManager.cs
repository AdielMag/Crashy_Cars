using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppManager : MonoBehaviour
{
    #region Singelton
    static public AppManager instance;
    private void Awake()
    {
        instance = this;

        DontDestroyOnLoad(this);
    }
    #endregion

    public Animator transitionAnimatorController;

    private void Start()
    {
        transitionAnimatorController = GetComponentInChildren<Animator>();
    }

    public void LoadScene(string name)
    {
        StartCoroutine(LoadSceneWithTransition(name));
    }

    IEnumerator LoadSceneWithTransition(string name)
    {
        transitionAnimatorController.SetBool("On", true);

        yield return new WaitForSeconds(1);

        // Start loading the scene
        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync(name, LoadSceneMode.Single);
        // Wait until the level finish loading
        while (!asyncLoadLevel.isDone)
            yield return null;

        yield return new WaitForSeconds(.5f);
        transitionAnimatorController.SetBool("On", false);

        // Wait a frame so every Awake and Start method is called
        yield return new WaitForEndOfFrame();


    }
}
