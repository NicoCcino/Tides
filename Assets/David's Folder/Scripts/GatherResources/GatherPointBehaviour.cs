using System;
using UnityEngine;
namespace Tides.Resources
{
    public class GatherPointBehaviour : MonoBehaviour
    {
        [SerializeField] public ResourceType ResourceType;
        [SerializeField] private AnimationCurve scaleCurve;
        [SerializeField] private Transform affectedTransform;
        public IResource Resource;
        private int baseAmount;
        public bool HasRemainingResources => Resource.GetAmount() > 0;

        /// <summary>
        /// Only for debug purpose
        /// </summary>
        private void OnEnable()
        {
            Initialize(ResourceType, 5);
        }
        private void OnDisable()
        {
            Resource.OnAmountChanged -= OnResourceAmountChanged;
        }
        public void Initialize(ResourceType resourceType, int amount)
        {
            this.ResourceType = resourceType;

            switch (resourceType)
            {
                case ResourceType.FOOD:
                    Resource = new FoodResource(amount);
                    break;
                case ResourceType.WOOD:
                    Resource = new WoodResource(amount);
                    break;
                default:
                    Resource = new FoodResource(amount);
                    break;
            }
            baseAmount = amount;
            Resource.OnAmountChanged += OnResourceAmountChanged;
        }

        private void OnResourceAmountChanged(int newAmount)
        {
            float scale = scaleCurve.Evaluate(newAmount / baseAmount);
            transform.localScale = new Vector3(scale, scale, scale);
        }
    }
    [System.Serializable]
    public enum ResourceType
    {
        NONE,
        FOOD,
        WOOD
    }
}