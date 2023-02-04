using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mushroom
{
    public class TendrilNode : MonoBehaviour
    {
        [SerializeField] int lifePoints = 2;
        readonly List<TendrilNode> _childrenTendrilNodes = new List<TendrilNode>();
        
        public void RemoveLifePoints(int damage)
        {
            lifePoints -= damage;
            if (lifePoints <= 0)
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
            _childrenTendrilNodes.Add(newTendril);
        }

        public bool IsTargetPositionUnderTargetDistanceFromThisNodeAndChildren(float minDistance, Vector3 targetPosition)
        {
            return Vector3.Distance(transform.position, targetPosition) <= minDistance ||
                   _childrenTendrilNodes.Any(tendrilNode => tendrilNode.IsTargetPositionUnderTargetDistanceFromThisNodeAndChildren(minDistance, targetPosition));
        }
    }
}
