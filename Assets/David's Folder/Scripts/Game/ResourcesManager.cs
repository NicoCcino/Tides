using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
namespace Tides.Resources
{

    public class ResourcesManager : Singleton<ResourcesManager>
    {
        public FoodResource FoodResource;
        public WoodResource WoodResource;
        [SerializeField] private int defaultFood = 25;
        [SerializeField] private int defaultWood = 25;

        [SerializeField] public int FoodConsumptionPerSurvivor = 5;
        private void OnEnable()
        {
            FoodResource = new FoodResource(defaultFood);
            WoodResource = new WoodResource(defaultWood);
        }
        public void AddFood(int amount)
        {
            FoodResource.Add(amount);
        }
        public bool TryConsumeFood(int amount)
        {
            if (!(FoodResource as IResource).TryConsume(amount))
            {
                Debug.Log("Not enough Food to consume !");
                return false;
            }
            return true;
        }

        public void AddWood(int amount)
        {
            WoodResource.Add(amount);
        }
        public bool TryConsumeWood(int amount)
        {
            if (!(WoodResource as IResource).TryConsume(amount))
            {
                Debug.Log("Not enough Wood to consume !");
                return false;
            }
            return true;
        }



        #region DEBUG
        [Button("Add 1 FOOD")]
        private void AddFoodDebug()
        {
            AddFood(1);
        }
        [Button("Add 1 WOOD")]
        private void AddWoodDebug()
        {
            AddWood(1);
        }
        [Button("Consume 1 FOOD")]
        private void ConsumeFoodDebug()
        {
            TryConsumeFood(1);
        }
        [Button("Consume 1 WOOD")]
        private void ConsumeWoodDebug()
        {
            TryConsumeWood(1);
        }
        #endregion
    }


}

