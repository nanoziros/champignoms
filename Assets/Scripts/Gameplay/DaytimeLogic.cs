using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DaytimeLogic : MonoBehaviour
{
    [SerializeField] private Image _daytimeImageLayer;
    [SerializeField] private float _timeFactor = 86400.0f / 60 / 2;

    private const float _secondsPerDay = 86400.0f;
    [SerializeField] private Color _dayColor;
    [SerializeField] private Color _nightColor;
    [SerializeField] private TextMeshProUGUI _daytimeLabel;
    [SerializeField] private TextMeshProUGUI _hourLabel;
    [SerializeField] private AudioClip _dayAudioClip;
    [SerializeField] private AudioClip _nightAudioClip;
    [SerializeField] private AudioClip _nightSecondAudioClip;

    [SerializeField] private int _minimumTemperatureForMushroom = 8;
    [SerializeField] private float _nightDamageFactor = 5.0f;
    [SerializeField] private List<int> _nightTemperatures = new List<int>();

    private float _currentTimeInSeconds = 0.0f;
    private bool _gameplayRunning = false;
    private DateTime _currentDateTime;
    private bool _isDaylight;
    private float _nextTimeCheckInSeconds = 0.0f;
    private int _nightAndDayIndex = 0;
    
    void Start()
    {
        _currentTimeInSeconds = 0.0f;
        _daytimeImageLayer.color = _dayColor;
        SoundController.Instance.Play(_dayAudioClip);
        _currentDateTime = new DateTime();
        _currentTimeInSeconds = _secondsPerDay * 0.5f;
        _nextTimeCheckInSeconds = _currentTimeInSeconds + _secondsPerDay * (7.0f / 24);
        _isDaylight = true;
        _daytimeLabel.text = GetDaytimeLabelData();
        GameplayLogic.Instance.ChangeDaytime(_isDaylight, 0);
        StartGameplay();
    }

    private string GetDaytimeLabelData()
    {
        var ret = _isDaylight ? "Day " : "Night ";
        return ret + (_nightAndDayIndex + 1);
    }

    public void StartGameplay()
    {
        _gameplayRunning = true;
    }

    public void PauseGameplay()
    {
        _gameplayRunning = false;
    }

    private void Update()
    {
        if (_gameplayRunning == false)
        {
            return;
        }

        _currentTimeInSeconds += Time.deltaTime * _timeFactor;
        _hourLabel.text = _currentDateTime.AddSeconds(_currentTimeInSeconds).ToShortTimeString();
        if (!(_nextTimeCheckInSeconds <= _currentTimeInSeconds)) return;
       
        _nextTimeCheckInSeconds += _secondsPerDay * 0.5f;
        _isDaylight = _isDaylight == false;
        _daytimeLabel.text = GetDaytimeLabelData();
        _daytimeImageLayer.DOColor(_isDaylight ? _dayColor : _nightColor, 2.0f);
        var nightClip = Random.Range(0.0f, 1.0f) > 0.5f ? _nightSecondAudioClip : _nightAudioClip;
        SoundController.Instance.Stop(true, _isDaylight ? _dayAudioClip : nightClip);
        if (_isDaylight == false)
        {
            _nightAndDayIndex++;
        }
        GameplayLogic.Instance.ChangeDaytime(_isDaylight, 
            (_minimumTemperatureForMushroom - _nightTemperatures[Math.Min(_nightAndDayIndex, _nightTemperatures.Count-1)])*_nightDamageFactor);
        
    }
}
