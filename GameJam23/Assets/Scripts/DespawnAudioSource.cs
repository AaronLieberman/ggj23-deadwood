using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnAudioSource : MonoBehaviour
{
    AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (_audioSource.isPlaying)
            return;

        Destroy(gameObject);
    }
}
