using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Gameplay.Entities;
using Mushroom;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameplayLogic : MonoBehaviour
{
    public PointerEventLogic _foregroundPointerEvent;
    public MushroomController _mushroomController;
    public TendrilNode _tendrilNodePrefab;
    private TendrilNode _selectedParentTendril;
    public Transform _tendrilParent;
    [SerializeField] private TextMeshProUGUI _mushMassLabel;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private DaytimeLogic _daytimeLogic;
    [SerializeField] private TextMeshProUGUI _mushDamageLabel;

    private List<NutrientNode> _nutrientNodes = new List<NutrientNode>();
    
    public static GameplayLogic Instance { get; private set; }
    private int _maxMushroomMass = 0;
    private Vector3 _damageLabelPosition;
    private bool _gameplayRunning = false;
    
    private void Awake()
    {
        Instance = this;
        _foregroundPointerEvent.SubscribeOnClick(OnForegroundClick);
        _mushroomController.Initialize();
        _maxMushroomMass = _mushroomController.AvailableMass;
        _gameOverPanel.SetActive(false);
        _mushDamageLabel.gameObject.SetActive(false);
        _damageLabelPosition = _mushDamageLabel.transform.position;
        _gameplayRunning = true;

        _nutrientNodes = GetComponentsInChildren<NutrientNode>().ToList();
        SetParentTendrilNode(_mushroomController.OriginTendril);
    }
    
    public void SetParentTendrilNode(TendrilNode tendrilNode)
    {
        if(tendrilNode == _selectedParentTendril) return;
        if (SoundController.Instance)
        {
            SoundController.Instance.PlaySfx();
        }
        if(_selectedParentTendril != null) {
            _selectedParentTendril.ToggleNode(false);
        }
        _selectedParentTendril = tendrilNode;
        _selectedParentTendril.ToggleNode(true);
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
        SoundController.Instance.PlaySfx();
        _selectedParentTendril.ToggleNode(false);
        _selectedParentTendril = null;
    }

    private void Update()
    {
        if (_gameplayRunning == false) return;
        var isMushAlive = _mushroomController.AvailableMass > 0;
        if (isMushAlive == false)
        {
            // end game
            _daytimeLogic.PauseGameplay();
            _mushroomController.GameOverLogic();
            _mushMassLabel.text = "YOU DIED";
            _gameOverPanel.SetActive(true);
            _gameplayRunning = false;
            return;
        }

        _mushroomController.UpdateMushroomNodes(_nutrientNodes, Time.deltaTime);
        _mushMassLabel.text = "Mushroom Mass : " + _mushroomController.AvailableMass + "/" + _maxMushroomMass;
    }

    public void ChangeDaytime(bool isDaylight, float nightDamage)
    {
        _mushroomController.EnableNightActions(isDaylight == false, (int)nightDamage);
    }
    
    public void TriggerDamageFeedback(int damage)
    {
        _mushDamageLabel.text = damage.ToString();
        _mushDamageLabel.color = Color.blue;
        _mushDamageLabel.transform.position = _damageLabelPosition;
        _mushDamageLabel.gameObject.SetActive(true);
        var sequence = DOTween.Sequence();
        sequence.Append(_mushDamageLabel.transform.DOLocalMoveY(-100, 0.5f));
        sequence.Join(_mushDamageLabel.transform.DOScale(1.5f, 0.25f));
        sequence.Append(_mushDamageLabel.transform.DOScale(0.5f, 0.25f));
        sequence.Join(_mushDamageLabel.DOFade(0, 0.25f));
        sequence.OnComplete(() => _mushDamageLabel.gameObject.SetActive(false)).Play();
    }
}