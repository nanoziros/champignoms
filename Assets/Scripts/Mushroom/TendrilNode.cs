using System.Collections.Generic;
using UnityEngine;

namespace Mushroom
{
    public class TendrilNode : MonoBehaviour
    {
        [SerializeField] int _lifePoints = 2;
        readonly List<TendrilNode> _childrenTendrilNodes = new List<TendrilNode>();
        
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
        
        public void AddTendril(TendrilNode tendrilPrefab, Vector3 position)
        {
            var newTendril = Instantiate(tendrilPrefab, transform);
            newTendril.transform.position = position;
            _childrenTendrilNodes.Add(newTendril);
        }
    }
}
