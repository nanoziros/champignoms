using UnityEngine;

namespace Gameplay.Entities
{
    public class NutrientNode : MonoBehaviour
    {
        public float radius = 10;
        [SerializeField] private int nutrientPoints = 10;
        private float currentNutrientPoints;
        private Vector3 originalScale;
        
        private void Start()
        {
            originalScale = transform.localScale;
            currentNutrientPoints = nutrientPoints;
        }

        public void SubtractNutrients(float amount)
        {
            currentNutrientPoints -= amount;
            if (currentNutrientPoints <= 0)
            {
                Destroy(gameObject);
            }

            var targetScale = currentNutrientPoints / (float) nutrientPoints;
            transform.localScale = originalScale * targetScale;
        }
    }
}
