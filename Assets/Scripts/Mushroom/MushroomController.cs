using UnityEngine;

namespace Mushroom
{
    public class MushroomController : MonoBehaviour
    {
        [SerializeField] Transform _tendrilNodeParent; 
        [SerializeField] TendrilNode _originNode;
        [SerializeField] int _availableMass; 
        
        public void UpdateMass(int mass)
        {
            _availableMass = Mathf.Max(0, _availableMass + mass);
        }

        public void AddTendril(TendrilNode originNode, Vector3 position)
        {
            var newTendril = Instantiate(_originNode, _tendrilNodeParent);
            newTendril.transform.position = position;
            newTendril.transform.SetParent(_tendrilNodeParent);
            originNode.AddTendril(originNode);
        }
    }
}
