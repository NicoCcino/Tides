using UnityEngine;

public class FoodResource : IResource
{
    protected int foodAmount;

    public FoodResource(int amount)
    {
        foodAmount = amount;
    }

    public void Add(int addedAmount)
    {
        foodAmount += addedAmount;
    }

    public int GetAmount()
    {
        return foodAmount;
    }

    void IResource.SetAmount(int amount)
    {
        foodAmount = amount;
    }
}
