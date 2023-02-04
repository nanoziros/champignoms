using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Mushroom
{
    public class TendrilNode : MonoBehaviour
    {
        [SerializeField] int lifePoints = 2;
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
            lifePoints -= damage;
            if (lifePoints <= 0)
            {
                DestroyTendril();
            }
        }

        public bool GetPathToNode(TendrilNode currentNode, TendrilNode targetNode, ref List<TendrilNode> track)
        {
            if (currentNode == null)
            {
                return false;
            }
            
            if (targetNode == currentNode)
            {
                track.Add(targetNode);
                return true;
            }

            foreach (var tendrilNode in _childrenTendrilNodes)
            {
                if (GetPathToNode(tendrilNode, targetNode, ref track))
                {
                    track.Add(currentNode);
                    return true;
                }
            }
            
            return false;
        }

        public void AddTendrilNode(TendrilNode tendrilPrefab, Vector3 position)
        {
            var newTendril = Instantiate(tendrilPrefab, transform);
            newTendril.transform.position = position;
            newTendril.transform.localScale = Vector3.one;
            _childrenTendrilNodes.Add(newTendril);
            // todo: generate tendril line
        }

        public bool IsTargetPositionUnderTargetDistanceFromThisNodeAndChildren(float minDistance, Vector3 targetPosition)
        {
            return Vector3.Distance(transform.position, targetPosition) <= minDistance ||
                   _childrenTendrilNodes.Any(tendrilNode => tendrilNode.IsTargetPositionUnderTargetDistanceFromThisNodeAndChildren(minDistance, targetPosition));
        }
        
        void DestroyTendril()
        {
            Destroy(gameObject);
        }
    }
}
