using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay.Entities;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Mushroom
{
    public class MushroomController : MonoBehaviour
    {
        [SerializeField] TendrilNode tendrilPrefab;
        [SerializeField] TendrilNode originNode;
        [SerializeField] float availableMass;
        [SerializeField] int newTendrilMassCost = 10;
        [SerializeField] float spawnTendrilNodeCooldown = 1;
        [SerializeField] float minimumSpawnTendrilDistance = 1;
        [SerializeField] float maximumSpawnTendrilDistance = 10;
        [SerializeField] float maxSpawningGroundHeight = 0;
        [SerializeField] float growthSpeed = 10;
        [SerializeField] float absorbStrength = 10;
        [SerializeField] float absorbRadius = 10;

        [SerializeField] private PointerEventLogic _poisonButtonPointerEventLogic;
        
        List<TendrilNode> _tendrilNodes = new List<TendrilNode>();
        bool _inSpawnTendrilCooldown;
        public int AvailableMass => Mathf.RoundToInt(availableMass);

        public void Initialize()
        {
            _poisonButtonPointerEventLogic.SubscribeOnClick(OnClick);
        }
        
        void OnClick(PointerEventData pointerEventData)
        {
            
        }

        public void UpdateMushroomNodes(List<NutrientNode> nutrientNodes, float deltaTime)
        {
            var absorbedNutrients = 0f;
            foreach (var tendrilNode in _tendrilNodes)
            {
                if (!tendrilNode.IsEnabled)
                {
                    continue;
                }
                absorbedNutrients += tendrilNode.TryGetNutrientsFromSurrounding(nutrientNodes, absorbStrength, absorbRadius, deltaTime);
            }

            availableMass += absorbedNutrients;
        }

        public bool TryAddTendrilNode(TendrilNode parentNode, Vector3 targetPosition)
        {
            var newTendrilCost = GetDistanceNormalizedNewTendrilMassCost(parentNode.transform.position, targetPosition);
            if (IsInvalidIsValidNewTendrilSpawn(targetPosition, newTendrilCost))
            {
                return false;
            }

            parentNode.AddTendrilNode(tendrilPrefab, targetPosition, growthSpeed);
            var distance = Vector3.Distance(targetPosition, parentNode.transform.position);
            StartCoroutine(UpdateMassOnDuration(-newTendrilCost, distance / growthSpeed));
            StartCoroutine(SpawnTendrilCooldown());
            _tendrilNodes = GetAllTendrilNodes();
            return true;
        }
        
        public IEnumerator UpdateMassOnDuration(int mass, float totalTime)
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

        private Coroutine _nightDamageRoutine;
        public void EnableNightActions(bool isNight, int nightDamage)
        {
            _poisonButtonPointerEventLogic.gameObject.SetActive(isNight);
            if (isNight)
            {
                _nightDamageRoutine = StartCoroutine(NightTemperatureDamageRoutine((nightDamage)));
                Debug.Log("Night Damage starts");
                return;
            }

            if (_nightDamageRoutine == null) return;
            
            StopCoroutine(_nightDamageRoutine);
            Debug.Log("Night Damage stopped");
        }

        private IEnumerator NightTemperatureDamageRoutine(int nightDamage)
        {
            while (true)
            {
                UpdateMassInstantly(-nightDamage);
                yield return new WaitForSeconds(1);
            }
        }

        private void UpdateMassInstantly(int mass)
        {
            availableMass = Mathf.Max(0, availableMass + mass);
        }

    }
}
