using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Gameplay.Entities
{
    public class TendrilNode : MonoBehaviour
    {
        [SerializeField] private TendrilLine tendrilLinePrefab;
        [SerializeField] private PointerEventLogic pointerEventLogic;
        [SerializeField] int lifePoints = 2;

        private readonly Dictionary<TendrilNode, TendrilLine> _childrenTendrilNodes =
            new Dictionary<TendrilNode, TendrilLine>();
        
        private void Awake()
        {
            pointerEventLogic.SubscribeOnClick(OnClick);
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
                if (GetPathToNode(tendrilNode.Key, targetNode, ref track))
                {
                    track.Add(currentNode);
                    return true;
                }
            }
            return false;
        }

        public void AddTendrilNode(TendrilNode tendrilPrefab, Vector3 position)
        {
            var newTendrilNode = Instantiate(tendrilPrefab, transform, true);
            newTendrilNode.transform.position = position;
            newTendrilNode.transform.localScale = Vector3.one;


            var newTendrilLine = Instantiate(tendrilLinePrefab, transform, true);
            newTendrilLine.transform.position = position;
            newTendrilLine.transform.localScale = Vector3.one;

            newTendrilLine.Initialize(this, newTendrilNode);
            _childrenTendrilNodes.Add(newTendrilNode, newTendrilLine);
        }

        public bool IsTargetPositionUnderTargetDistanceFromThisNodeAndChildren(float minDistance, Vector3 targetPosition)
        {
            return Vector3.Distance(transform.position, targetPosition) <= minDistance ||
                   _childrenTendrilNodes.Any(
                       tendrilNode => tendrilNode.Key.IsTargetPositionUnderTargetDistanceFromThisNodeAndChildren(minDistance, targetPosition));
        }
        
        void DestroyTendril()
        {
            Destroy(gameObject);
        }
    }
}
