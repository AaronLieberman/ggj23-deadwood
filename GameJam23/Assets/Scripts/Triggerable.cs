using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Triggerable : MonoBehaviour
{
    private bool _summoning = false;
    private float _timer = 0f;

    public float TimerPercentage => _timer / TimeToActivate;

    [Range(0f,20f)]
    public float TimeToActivate = 1f;

    [Range(0f,5f)]
    public float TimerDecayPerSecond = 0.1f;

    public GameObject DespawnOnActivate;

    public Transform BarAnchorObject;
    public Texture2D FilledTexture;
    public Texture2D UnfilledTexture;

    public UnityEvent Activate;

    private void Update()
    {
        if (_summoning)
        {
            _timer += Time.deltaTime;
            if (_timer >= TimeToActivate)
            {
                FinishSummoning();
            }
        }
        else if (!_summoning && _timer >= 0f)
        {
            _timer -= TimerDecayPerSecond * Time.deltaTime;
            if (_timer <= 0f)
            {
                _timer = 0f;
            }
        }
    }

    public void StartSummoning()
    {
        _summoning = true;
    }

    public void CancelSummoning()
    {
        _summoning = false;
    }

    public void FinishSummoning()
    {
        _summoning = false;
        Activate?.Invoke();

        if (DespawnOnActivate)
        {
            Destroy(DespawnOnActivate);
        }
    }

    private void OnGUI()
    {
        if (!_summoning && _timer <= 0f)
            return;

        Rect BarRect = new Rect(0,0,32,32);
        BarRect.width = 128f;
        BarRect.height = 48f;
        BarRect.center = Camera.main.WorldToScreenPoint(BarAnchorObject.transform.position);
        BarRect.y = Camera.main.scaledPixelHeight - BarRect.y;

        Rect BGRect = BarRect;
        BGRect.xMin++;
        BGRect.xMax--;
        BGRect.yMin++;
        BGRect.yMax--;
        GUI.DrawTexture(BGRect, UnfilledTexture);

        Rect FillRect = BarRect;
        FillRect.width = TimerPercentage * FillRect.width;
        FillRect.xMin = BarRect.xMin;

        GUI.DrawTexture(FillRect, FilledTexture);
    }
}
