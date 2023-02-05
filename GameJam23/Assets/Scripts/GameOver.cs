using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    Canvas _canvas;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
    }

    private void Start()
    {
        _canvas.enabled = false;

        PlayerResources.Instance.Death += ShowGameOver;
    }

    public void ShowGameOver()
    {
        _canvas.enabled = true;
    }
}
