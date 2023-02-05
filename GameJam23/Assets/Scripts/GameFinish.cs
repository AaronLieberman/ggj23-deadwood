using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameFinish : MonoBehaviour
{
    public Canvas MainCanvas;
    public Animator animator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            ShowEndScreen();
        }
    }

    void ShowEndScreen()
    {
        //PlayerResources.Instance.Health = 0;
        MainCanvas.enabled = true;
        animator.Play("Fade");
        StartCoroutine(QuitAfterFinish());
    }

    IEnumerator QuitAfterFinish()
    {
        yield return new WaitForSeconds(3f);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
