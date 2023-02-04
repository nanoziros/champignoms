using DG.Tweening;
using UnityEngine;

namespace Gameplay.Entities
{
    public class TendrilLine : MonoBehaviour
    {
        [SerializeField] LineRenderer lineRenderer;
        [SerializeField] float growthSpeed = 1;
        Tween _growthAnimation;
        
        public Tween GrowNode(TendrilNode originNode, TendrilNode targetNode)
        {
            var originPosition = originNode.transform.position;
            var targetPosition = targetNode.transform.position;
            
            lineRenderer.SetPosition(0, originPosition);
            lineRenderer.SetPosition(1, originPosition);

            _growthAnimation = DOTween
                .To(() => lineRenderer.GetPosition(1), x => lineRenderer.SetPosition(1, x), targetPosition, growthSpeed)
                .SetEase(Ease.Linear).SetSpeedBased().Play();
            
            return _growthAnimation;
        }
    }
}
