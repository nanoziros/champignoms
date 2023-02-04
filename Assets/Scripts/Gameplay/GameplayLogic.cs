using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay.Entities;
using Mushroom;
using TMPro;
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
    [SerializeField] private TextMeshProUGUI _mushMassLabel;
    [SerializeField] private GameObject _gameOverPanel;

    public static GameplayLogic Instance { get; private set; }
    private int _maxMushroomMass = 0;
    
    private void Awake()
    {
        Instance = this;
        _foregroundPointerEvent.SubscribeOnClick(OnForegroundClick);
        _maxMushroomMass = _mushroomController.AvailableMass;
        _gameOverPanel.SetActive(false);
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
        
        var tendrilAdded = _mushroomController.TryAddTendrilNode(_selectedParentTendril, worldPoint);
        if (!tendrilAdded)
        {
            return;
        }
        _selectedParentTendril = null;
    }

    private void Update()
    {
        var isMushAlive = _mushroomController.AvailableMass > 0;
        if (isMushAlive == false)
        {
            // end game
            _gameOverPanel.SetActive(true);
            return;
        }

        _mushMassLabel.text = "Mushroom Mass : " + _mushroomController.AvailableMass + "/" + _maxMushroomMass;
    }
}
