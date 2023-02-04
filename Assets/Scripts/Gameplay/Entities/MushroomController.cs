using System.Collections;
using System.Collections.Generic;
using Gameplay.Entities;
using UnityEngine;

namespace Mushroom
{
    public class MushroomController : MonoBehaviour
    {
        [SerializeField] TendrilNode tendrilPrefab;
        [SerializeField] TendrilNode originNode;
        [SerializeField] int availableMass;
        [SerializeField] int newTendrilMassCost = 10;
        [SerializeField] float spawnTendrilNodeCooldown = 1;
        [SerializeField] float minimumSpawnTendrilDistance = 1;
        [SerializeField] float maximumSpawnTendrilDistance = 10;
        [SerializeField] float maxSpawningGroundHeight = 0;

        List<TendrilNode> _tendrilNodes;
        bool _inSpawnTendrilCooldown;
        public int AvailableMass => availableMass;

        public bool TryAddTendrilNode(TendrilNode parentNode, Vector3 targetPosition)
        {
            var newTendrilCost = GetDistanceNormalizedNewTendrilMassCost(parentNode.transform.position, targetPosition);
            if (IsInvalidIsValidNewTendrilSpawn(targetPosition, newTendrilCost))
            {
                return false;
            }

            parentNode.AddTendrilNode(tendrilPrefab, targetPosition);
            
            UpdateMass(-newTendrilCost);
            StartCoroutine(SpawnTendrilCooldown());
            _tendrilNodes = GetAllTendrilNodes();
            return true;
        }
        
        public void UpdateMass(int mass)
        {
            availableMass = Mathf.Max(0, availableMass + mass);
        }
        
        public List<TendrilNode> GetAllTendrilNodes()
        {
            return originNode.GetAllTendrilNodes();
        }

        public List<TendrilNode> GetPathToNode(TendrilNode targetNode)
        {
            var path = new List<TendrilNode>();
            originNode.GetPathToNode(originNode, targetNode, ref path);
            return path;
        }
        
        bool IsInvalidIsValidNewTendrilSpawn(Vector3 targetPosition, int newTendrilCost)
        {
            if (availableMass < newTendrilCost)
            {
                Debug.Log($"Not enough mass to spawn tendril. Cost ({newTendrilCost})");
                return true;
            }
            if (targetPosition.y >= maxSpawningGroundHeight)
            {
                Debug.Log($"Trying to spawn the tendrils at ({targetPosition.y}), which is above the min height ({maxSpawningGroundHeight})");
                return true;
            }
            if (_inSpawnTendrilCooldown)
            {
                Debug.Log("Tendril spawn in cooldown");
                return true;
            }
            if (originNode.IsTargetPositionUnderTargetDistanceFromThisNodeAndChildren(minimumSpawnTendrilDistance,
                targetPosition))
            {
                Debug.Log("Tried to spawn tendril too close to parent node");
                return true;
            }
            return false;
        }

        int GetDistanceNormalizedNewTendrilMassCost(Vector3 parentNodePosition, Vector3 targetPosition)
        {
            var newTendrilDistance = Vector3.Distance(parentNodePosition, targetPosition);
            var normalizedDistance = (newTendrilDistance - minimumSpawnTendrilDistance) / (maximumSpawnTendrilDistance - minimumSpawnTendrilDistance);
            return Mathf.CeilToInt(normalizedDistance * newTendrilMassCost);
        }

        IEnumerator SpawnTendrilCooldown()
        {
            _inSpawnTendrilCooldown = true;
            yield return new WaitForSeconds(spawnTendrilNodeCooldown);
            _inSpawnTendrilCooldown = false;
        }
    }
}
