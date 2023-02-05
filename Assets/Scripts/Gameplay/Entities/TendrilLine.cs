using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace Gameplay.Entities
{
    public class TendrilLine : MonoBehaviour
    {
        [SerializeField] int maxSubdivisions = 20;
        [SerializeField] float maxDistanceSubdivisions = 40;
        [SerializeField] float variancePosition = 10;
        [SerializeField] LineRenderer lineRenderer;
        
        Sequence _growthSequence; 
        Vector3[] _tendrilNodes = new Vector3[0];
        public List<Vector3> TendrilNodes => _tendrilNodes.ToList();
        
        public Sequence GrowNode(TendrilNode originNode, TendrilNode targetNode, float growthSpeed)
        {
            var originPosition = originNode.transform.position;
            var finalPosition = targetNode.transform.position;

            var normalizedDistance = Mathf.Clamp01(Vector3.Distance(originPosition, finalPosition) / maxDistanceSubdivisions);
            var subDivisions = Mathf.CeilToInt(Mathf.Lerp(2, maxSubdivisions, normalizedDistance));

            var targetPositions = new Vector3[subDivisions];
            for (var i = 0; i < subDivisions; i++)
            {
                var normalizedIndex = i / (float) subDivisions;
                var interpolatedPosition = Vector3.Lerp(originPosition, finalPosition, normalizedIndex);

                if (i != subDivisions - 1)
                {
                    var variance = new Vector3(Random.Range(-variancePosition, variancePosition),
                        Random.Range(-variancePosition, variancePosition), 0);
                    targetPositions[i] = interpolatedPosition + variance;
                }
                else
                {
                    targetPositions[i] = finalPosition;
                }
            }
            
            var targetSpeeds = new float[subDivisions];
            for (var i = 1; i < subDivisions; i++)
            {
                var distance = Vector3.Distance(targetPositions[i], targetPositions[i - 1]);
                targetSpeeds[i] = distance / growthSpeed;
            }
            
            _growthSequence = DOTween.Sequence();
            for (var i = 1; i < subDivisions; i++)
            {
                var currentMaxIndex = i;
                _growthSequence.AppendCallback(() =>
                {
                    lineRenderer.positionCount = currentMaxIndex + 1;
                    for (var j = 0; j < currentMaxIndex; j++)
                    {
                        lineRenderer.SetPosition(j, targetPositions[j]);
                    }
                });
                _growthSequence.AppendCallback(() =>
                {
                    lineRenderer.SetPosition(currentMaxIndex, targetPositions[currentMaxIndex-1]);
                });
                _growthSequence.Append(DOTween
                    .To(() => lineRenderer.GetPosition(currentMaxIndex), x => lineRenderer.SetPosition(currentMaxIndex, x), targetPositions[currentMaxIndex],
                        targetSpeeds[currentMaxIndex]).SetEase(Ease.Linear));
                _growthSequence.AppendCallback(() =>
                {
                    _tendrilNodes = targetPositions;
                });
            }
            
            return _growthSequence;
        }
    }
}
