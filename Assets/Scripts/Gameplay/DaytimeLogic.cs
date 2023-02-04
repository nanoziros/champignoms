using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DaytimeLogic : MonoBehaviour
{
    [SerializeField] private Image _daytimeImageLayer;
    [SerializeField] private float _timeFactor = 86400.0f / 60 / 2;

    private const float _secondsPerDay = 86400.0f;
    [SerializeField] private Color _dayColor;
    [SerializeField] private Color _nightColor;
    [SerializeField] private TextMeshProUGUI _daytimeLabel;
    
    private float _currentTimeInSeconds = 0.0f;
    private bool _gameplayRunning = false;
    private DateTime _currentDateTime;
    
    void Start()
    {
        _currentTimeInSeconds = 0.0f;
        _daytimeImageLayer.color = _dayColor;
        _currentDateTime = new DateTime();
        _currentTimeInSeconds = _secondsPerDay * 0.5f;
        StartGameplay();
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
        _daytimeLabel.text = _currentDateTime.AddSeconds(_currentTimeInSeconds).ToShortTimeString();
    }
}
