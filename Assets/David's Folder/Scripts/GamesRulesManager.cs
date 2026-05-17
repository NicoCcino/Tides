using System.Linq;
using Tides.Resources;
using UnityEngine;

public class GamesRulesManager : Singleton<GamesRulesManager>
{
    [SerializeField] private BuildableBehaviour boatBuildable;
    private void FixedUpdate()
    {
        if (SurvivorsController.Instance.Survivors.Count == 0 && ResourcesManager.Instance.FoodResource.GetAmount() < 5)
        {
            Debug.Log("GAME OVER. No more survivors are not enough to create a new one");
            GameStateManager.Instance.ChangeState(EGameState.GameOver);
        }
        if (CampsController.Instance.Camps.Count <= 0)
        {
            Debug.Log("GAME OVER. No more camps and stocks.");
            GameStateManager.Instance.ChangeState(EGameState.GameOver);
        }
        if (TidesManager.Instance.currentCycleIndex >= TidesManager.Instance.tideCyclesSO.tideCycles.Count())
        {
            Debug.Log("GAME OVER. The tide was too high, you took too long to build the boat.");
            GameStateManager.Instance.ChangeState(EGameState.GameOver);
        }

        if (boatBuildable.BuildingProgress >= 1.0f)
        {
            Debug.Log("SUCCESS. The boat is constructed");
            GameStateManager.Instance.ChangeState(EGameState.Victory);
        }
    }
}
