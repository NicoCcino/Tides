using UnityEngine;

public class BaseManager : Singleton<BaseManager>
{
    [SerializeField] private FoodResource foodResource;
    [SerializeField] private WoodResource woodResource;

    public void AddFood(int amount)
    {
        foodResource.Add(amount);
    }
    public void ConsumeFood(int amount)
    {
        (foodResource as IResource).TryConsume(amount);
    }

    public void AddWood(int amount)
    {
        woodResource.Add(amount);
    }
    public void ConsumeWood(int amount)
    {
        (woodResource as IResource).TryConsume(amount);
    }

}
