using System;
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
        [SerializeField] float growthSpeed = 10;

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

            parentNode.AddTendrilNode(tendrilPrefab, targetPosition, growthSpeed);
            var distance = Vector3.Distance(targetPosition, parentNode.transform.position);
            StartCoroutine(UpdateMass(-newTendrilCost, distance / growthSpeed));
            StartCoroutine(SpawnTendrilCooldown());
            _tendrilNodes = GetAllTendrilNodes();
            return true;
        }
        
        public IEnumerator UpdateMass(int mass, float totalTime)
        {
            var target = Mathf.Max(0, availableMass + mass);
            var currentTime = 0.0f;
            //Debug.Log("current mass: " + availableMass + ", target: " + target);
            var tick = totalTime / Math.Abs(mass);
            var sign = Math.Sign(mass);
            var diff = Math.Abs(mass);
            while (diff > 0)
            {
                currentTime += Time.deltaTime;
                if (currentTime >= tick)
                {
                    availableMass += sign;
                    diff--;
                    currentTime -= tick;
                }

                yield return new WaitForEndOfFrame();
            }
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
                Debug.Log($"Not enough mass to spawn tendril, ready to DIE? Cost ({newTendrilCost})");
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
