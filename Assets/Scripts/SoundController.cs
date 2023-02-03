using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField]
    private float _fadeOutTime = 1.5f;

    private AudioSource _audioMusicSource;
    private float _defaultVolume = 0.0f;

    public bool IsEnabled { get => _audioMusicSource.enabled; set => _audioMusicSource.enabled = value; }

    void Start()
    {
        _audioMusicSource = GetComponent<AudioSource>();
        _defaultVolume = _audioMusicSource.volume;
    }    

    public void Play(AudioClip clip)
    {
        StopCoroutine(FadeOut());
        _audioMusicSource.volume = _defaultVolume;
        _audioMusicSource.clip = clip;
        _audioMusicSource.Play();
    }

    public void Stop(bool fadeOut = true)
    {
        if (fadeOut)
        {
            StartCoroutine(FadeOut());
            return;
        }
        _audioMusicSource.Stop();
    }

    public IEnumerator FadeOut()
    {
        while (_audioMusicSource.volume > 0)
        {
            _audioMusicSource.volume -= _defaultVolume * Time.unscaledDeltaTime / _fadeOutTime;

            yield return null;
        }

        _audioMusicSource.Stop();
        _audioMusicSource.volume = _defaultVolume;
    }
}
