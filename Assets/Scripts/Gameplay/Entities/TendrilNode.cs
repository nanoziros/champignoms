using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Gameplay.Entities
{
    public class TendrilNode : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private PolygonCollider2D spriteCollider;
        [SerializeField] private TendrilLine tendrilLinePrefab;
        [SerializeField] private PointerEventLogic pointerEventLogic;
        [SerializeField] int lifePoints = 2;
        
        readonly Dictionary<TendrilNode, TendrilLine> _childrenTendrilNodes =
            new Dictionary<TendrilNode, TendrilLine>();
        
        
        private void Awake()
        {
            pointerEventLogic.SubscribeOnClick(OnClick);
        }

        private void OnClick(PointerEventData pointerEventData)
        {
            GameplayLogic.Instance.SetParentTendrilNode(this);
        }

        public void SetDelayedActivation(Tween delayTween)
        {
            spriteRenderer.enabled = false;
            spriteCollider.enabled = false;
            delayTween.OnComplete(() =>
            {
                spriteRenderer.enabled = true;
                spriteCollider.enabled = true; 
            });
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

            var growthTween = newTendrilLine.GrowNode(this, newTendrilNode);
             newTendrilNode.SetDelayedActivation(growthTween);
             
            _childrenTendrilNodes.Add(newTendrilNode, newTendrilLine);
        }

        public bool IsTargetPositionUnderTargetDistanceFromThisNodeAndChildren(float minDistance, Vector3 targetPosition)
        {
            var distance = Vector3.Distance(transform.position, targetPosition);
            if (distance <= minDistance)
            {
                Debug.Log($"invalid distance: {distance}");
            }
            return distance <= minDistance ||
                   _childrenTendrilNodes.Any(
                       tendrilNode => tendrilNode.Key.IsTargetPositionUnderTargetDistanceFromThisNodeAndChildren(minDistance, targetPosition));
        }
        
        void DestroyTendril()
        {
            //TODO: we might need to kill all ongoing transitions on this and childobjects here
            Destroy(gameObject);
        }
    }
}
