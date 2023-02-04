using System.Collections;
using UnityEngine;

namespace Mushroom
{
    public class MushroomController : MonoBehaviour
    {
        [SerializeField] TendrilNode _originNode;
        [SerializeField] int _availableMass;
        [SerializeField] float _spawnTendrilNodeCooldown = 1;
        
        bool _inSpawnTendrilCooldown = false;
        
        public void UpdateMass(int mass)
        {
            _availableMass = Mathf.Max(0, _availableMass + mass);
        }

        public void AddTendrilNode(TendrilNode parentNode, Vector3 position)
        {
            if (_inSpawnTendrilCooldown)
            {
                return;
            }
            parentNode.AddTendrilNode(_originNode, position);
            StartCoroutine(SpawnTendrilCooldown());
        }

        IEnumerator SpawnTendrilCooldown()
        {
            _inSpawnTendrilCooldown = true;
            yield return new WaitForSeconds(_spawnTendrilNodeCooldown);
            _inSpawnTendrilCooldown = false;
        }
    }
}
