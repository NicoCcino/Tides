using UnityEngine;
using WiDiD.SceneManagement;

/// <summary>
/// Button that restarts the game by switching to the Playing state and reloading all associated scenes except the active one.
/// </summary>
public class RestartButton : ASimpleButton
{
    protected override void OnClickCallback()
    {
        if (GameStateManager.Instance == null)
        {
            Debug.LogError("GameStateManager instance not found!");
            return;
        }

        // 1. Switch the game state back to Playing.
        // If we are already in Playing state, ChangeState will handle exiting and re-entering the state.
        GameStateManager.Instance.ChangeState(EGameState.Playing);

        // 2. Reload all scenes in the Playing state's SceneSet, except for the Active Scene.
        // We use the StateDictionary to retrieve the PlayingState object and its SceneSet.
        if (GameStateManager.Instance.StateDictionary.TryGetValue(EGameState.Playing, out AGameState playingState))
        {
            SceneSet set = playingState.SceneSet;
            if (set != null)
            {
                foreach (var scene in set.Scenes)
                {
                    // Skip the active scene as requested (this often contains managers or UI that should persist).
                    if (scene.ScenePath == set.ActiveScene.ScenePath)
                        continue;

                    // Manually trigger an unload and then a forced load to effectively "reload" the scene.
                    // safeLoad is set to false to ensure the scene loads even if the manager thinks it's already there.
                    SceneManager.Instance.UnloadScene(scene.ScenePath, true);
                    SceneManager.Instance.LoadScene(scene.ScenePath, false);
                }
            }
            else
            {
                Debug.LogWarning("PlayingState does not have a valid SceneSet assigned.");
            }
        }
    }
}
