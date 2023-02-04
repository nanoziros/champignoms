using UnityEngine;

namespace Gameplay.Entities
{
    public class NutrientNode : MonoBehaviour
    {
        [SerializeField] private int nutrientPoints = 10;
        private float currentNutrientPoints;
        
        private void Start()
        {
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
            transform.localScale = Vector3.one * targetScale;
        }
    }
}
