using UnityEngine;
namespace Tides.Resources
{
    public class GatherPointBehaviour : MonoBehaviour
    {
        [SerializeField] public ResourceType ResourceType;
        public IResource Resource;
        public bool HasRemainingResources => Resource.GetAmount() > 0;

        /// <summary>
        /// Only for debug purpose
        /// </summary>
        private void OnEnable()
        {
            Initialize(ResourceType, 5);
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