using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialStep : MonoBehaviour
{
    public Button btn;

    public float timer;

    private void Start()
    {
        StartCoroutine(WaitThanInvoke());
    }

    IEnumerator WaitThanInvoke()
    {
        yield return new WaitForSecondsRealtime(timer);

        btn.onClick.Invoke();
    }
}
