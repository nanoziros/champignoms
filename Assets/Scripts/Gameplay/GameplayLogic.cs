using System;
using System.Collections;
using System.Collections.Generic;
using Mushroom;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameplayLogic : MonoBehaviour
{
    public PointerEventLogic _foregroundPointerEvent;
    public MushroomController _mushroomController;
    public TendrilNode _tendrilNodePrefab;
    private TendrilNode _selectedParentTendril;
    public Transform _tendrilParent;

    public static GameplayLogic Instance { get; private set; }
    
    private void Awake()
    {
        Instance = this;
        _foregroundPointerEvent.SubscribeOnClick(OnForegroundClick);
    }

    public void SetParentTendrilNode(TendrilNode tendrilNode)
    {
        _selectedParentTendril = tendrilNode;
    }

    private void OnForegroundClick(PointerEventData pointerEventData)
    {
        // check if its possible to generate a tendril node
        if (_selectedParentTendril == null)
        {
            Debug.LogWarning("No parent tendril, no new tendril node");
            return;
        }

        // get the event data, translate it to coords
        var worldPoint = Camera.main.ScreenToWorldPoint(pointerEventData.position);
        worldPoint.z = 0;
        _selectedParentTendril.AddTendrilNode(_tendrilNodePrefab, worldPoint);
        _selectedParentTendril = null;
    }
}
