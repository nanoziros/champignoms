using UnityEngine;

namespace Mushroom
{
    public class MushroomController : MonoBehaviour
    {
        [SerializeField] TendrilNode _originNode;
        [SerializeField] int _availableMass; 
        
        public void UpdateMass(int mass)
        {
            _availableMass = Mathf.Max(0, _availableMass + mass);
        }

        public void AddTendrilNode(TendrilNode parentNode, Vector3 position)
        {
            parentNode.AddTendrilNode(_originNode, position);
        }
    }
}
