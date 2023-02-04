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
    public static SoundController Instance { get; private set; }

    void Awake()
    {
        Instance = this;
        _audioMusicSource = GetComponent<AudioSource>();
        _defaultVolume = _audioMusicSource.volume;
    }    

    public void Play(AudioClip clip)
    {
        StopCoroutine(FadeOut(null));
        _audioMusicSource.volume = _defaultVolume;
        _audioMusicSource.clip = clip;
        _audioMusicSource.Play();
    }

    public void Stop(bool fadeOut = true, AudioClip nextTrack = null)
    {
        if (fadeOut)
        {
            StartCoroutine(FadeOut(nextTrack));
            return;
        }
        _audioMusicSource.Stop();
    }

    private IEnumerator FadeOut(AudioClip nextTrack)
    {
        while (_audioMusicSource.volume > 0)
        {
            _audioMusicSource.volume -= _defaultVolume * Time.unscaledDeltaTime / _fadeOutTime;

            yield return null;
        }

        _audioMusicSource.Stop();
        _audioMusicSource.volume = _defaultVolume;
        if (nextTrack != null)
        {
            _audioMusicSource.clip = nextTrack;
            _audioMusicSource.Play();
        }
    }
}
