using NaughtyAttributes;
using UnityEngine;
namespace Tides.Resources
{

    public class ResourcesManager : Singleton<ResourcesManager>
    {
        public FoodResource FoodResource;
        public WoodResource WoodResource;

        private void OnEnable()
        {
            FoodResource = new FoodResource(25);
            WoodResource = new WoodResource(10);
        }
        public void AddFood(int amount)
        {
            FoodResource.Add(amount);
        }
        public void ConsumeFood(int amount)
        {
            if (!(FoodResource as IResource).TryConsume(amount))
            {
                Debug.Log("Not enough Food to consume !");
            }
        }

        public void AddWood(int amount)
        {
            WoodResource.Add(amount);
        }
        public void ConsumeWood(int amount)
        {
            if (!(WoodResource as IResource).TryConsume(amount))
            {
                Debug.Log("Not enough Wood to consume !");
            }
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
            ConsumeFood(1);
        }
        [Button("Consume 1 WOOD")]
        private void ConsumeWoodDebug()
        {
            ConsumeWood(1);
        }
        #endregion
    }


}

