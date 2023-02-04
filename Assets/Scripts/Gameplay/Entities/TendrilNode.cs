using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Mushroom
{
    public class TendrilNode : MonoBehaviour
    {
        [SerializeField] int _lifePoints = 2;
        readonly List<TendrilNode> _childrenTendrilNodes = new List<TendrilNode>();
        [SerializeField] private PointerEventLogic _pointerEventLogic;

        private void Awake()
        {
            _pointerEventLogic.SubscribeOnClick(OnClick);
        }

        private void OnClick(PointerEventData pointerEventData)
        {
            GameplayLogic.Instance.SetParentTendrilNode(this);
        }

        public void RemoveLifePoints(int damage)
        {
            _lifePoints -= damage;
            if (_lifePoints <= 0)
            {
                DestroyTendril();
            }
        }

        void DestroyTendril()
        {
            Destroy(gameObject);
        }
        
        public void AddTendrilNode(TendrilNode tendrilPrefab, Vector3 position)
        {
            var newTendril = Instantiate(tendrilPrefab, transform);
            newTendril.transform.position = position;
            newTendril.transform.localScale = Vector3.one;
            _childrenTendrilNodes.Add(newTendril);
        }
    }
}
