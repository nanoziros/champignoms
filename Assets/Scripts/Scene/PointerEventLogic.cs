using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerEventLogic : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private float _currentDuration = 0.0f;
    private bool _isPointerDown = false;
    private Action<PointerEventData> _onClick;

    public void SubscribeOnClick(Action<PointerEventData> onClick)
    {
        _onClick = onClick;
    }

    void Update()
    {
        if (_isPointerDown)
        {
            _currentDuration += Time.deltaTime;
        }
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        _currentDuration = 0.0f;
        _isPointerDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isPointerDown = false;
        _onClick?.Invoke(eventData);
    }
}
