using UnityEngine;

namespace Gameplay.Entities
{
    public class TendrilLine : MonoBehaviour
    {
        [SerializeField] LineRenderer lineRenderer;
        
        public void Initialize(TendrilNode originNode, TendrilNode targetNode)
        {
            lineRenderer.SetPosition(0, originNode.transform.position);
            lineRenderer.SetPosition(1, targetNode.transform.position);
        }
    }
}
