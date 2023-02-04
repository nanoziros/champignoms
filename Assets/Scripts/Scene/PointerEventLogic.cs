using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerEventLogic : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private float _currentDuration = 0.0f;
    private bool _isPointerDown = false;
    
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
        Debug.Log("OnPointerUp");
        _isPointerDown = false;
    }
}
