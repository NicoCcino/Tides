using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using WiDiD.UI;


namespace WiDiD.SceneManagement
{
	public interface ISceneManager
	{
#if UNITY_EDITOR
		void UpdateSceneList();
#endif
	}

	[DisallowMultipleComponent]
	public class SceneManager : Singleton<SceneManager>
	{
		[SerializeField]
		private bool m_ShowDebugLog = true;
		[SerializeField]
		CanvasGroupCustom m_LoadingScreenCanvas;

		// There are errors while trying to activate a scene when multiple scenes are loading, we wait for the last scene to load before setting the active scene
		int m_ScenesCurrentlyLoading = 0;
		int m_ScenesCurrentlyUnloading = 0;

		public async void LoadSceneSet(SceneSet set, bool safeLoad = true, bool showLoadingScreen = true, System.Action OnSetLoaded = null)
		{
			if (m_ShowDebugLog) Debug.Log($"Loading {set.Scenes.Count} scenes...");

			if (showLoadingScreen)
			{
				m_LoadingScreenCanvas.Fade(true);
				await UniTask.WaitForSeconds(0.5f);
			}

			m_ScenesCurrentlyLoading = set.Scenes.Count;
			System.Action<AsyncOperation> callback = (ao) => SetSceneActiveCallback(set.ActiveScene, OnSetLoaded);

			if (set.Scenes.Count == 0)
			{
				SetSceneActiveCallback(set.ActiveScene, OnSetLoaded);
				return;
			}

			foreach (var scene in set.Scenes)
			{
				LoadScene(scene, safeLoad, callback);
			}
		}

		public async void UnloadSceneSet(SceneSet set, bool safeUnload = true, bool showLoadingScreen = true, bool destroyAllObjects = false, System.Action OnSetUnloaded = null)
		{
			var scenesToUnload = set.Scenes.FindAll(s => set.ActiveScene == null || s.ScenePath != set.ActiveScene.ScenePath);

			if (m_ShowDebugLog) Debug.Log($"Unloading {scenesToUnload.Count} scenes (skipped active scene: {set.ActiveScene?.ScenePath})...");

			if (showLoadingScreen)
			{
				m_LoadingScreenCanvas.Fade(true);
				await UniTask.WaitForSeconds(0.5f);
			}

			System.Action<AsyncOperation> callback = FadeOffCanvas;
			if (OnSetUnloaded != null)
			{
				m_ScenesCurrentlyUnloading = scenesToUnload.Count;
				callback = (ao) =>
				{
					if (showLoadingScreen) FadeOffCanvas(ao);
					UnloadCallback(OnSetUnloaded);
				};
			}

			if (scenesToUnload.Count == 0)
			{
				if (showLoadingScreen) FadeOffCanvas(null);
				OnSetUnloaded?.Invoke();
				return;
			}

			foreach (var scene in scenesToUnload)
			{
				UnloadScene(scene, safeUnload, destroyAllObjects, callback);
			}
		}
		private void FadeOffCanvas(AsyncOperation _)
		{
			m_LoadingScreenCanvas.Fade(false);
		}
		/// <summary>
		/// Unload the given scene
		/// </summary>
		/// <param name="sceneName"></param>
		/// <param name="destroyAllObjects">Set true to enable UnloadAllEmbeddedSceneObjects option <seealso cref="UnityEngine.SceneManagement.UnloadSceneOptions.UnloadAllEmbeddedSceneObjects"/></param>
		public void UnloadScene(string sceneName, bool safeUnload = true, bool destroyAllObjects = false, System.Action<AsyncOperation> onCompleted = null)
		{
			if (safeUnload)
			{
				if (!IsSceneLoaded(sceneName))
				{
					if (onCompleted != null)
					{
						m_ScenesCurrentlyUnloading--;
						if (m_ShowDebugLog) Debug.Log($"{sceneName} is not loaded, so it's ignored. {m_ScenesCurrentlyUnloading} remaining scenes to unload.");
					}
					else
					{
						if (m_ShowDebugLog) Debug.Log($"{sceneName} is not loaded, so it's ignored.");
					}

					return;
				}
			}

			if (m_ShowDebugLog) Debug.Log($"Unloading {sceneName}... (safeUnload is " + safeUnload + ")");

			var asyncOp = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneName, destroyAllObjects ? UnityEngine.SceneManagement.UnloadSceneOptions.UnloadAllEmbeddedSceneObjects : UnityEngine.SceneManagement.UnloadSceneOptions.None);
			if (asyncOp != null && onCompleted != null)
				asyncOp.completed += onCompleted;
			else if (asyncOp == null && onCompleted != null)
				onCompleted?.Invoke(null);
		}

		/// <summary>
		/// Load a scene
		/// </summary>
		/// <param name="sceneName">Scene name</param>
		/// <param name="safeLoad">Check if scene is already loaded to avoid double</param>
		public void LoadScene(string sceneName, bool safeLoad = true, System.Action<AsyncOperation> onCompleted = null)
		{
			if (safeLoad)
			{
				if (IsSceneLoaded(sceneName))
				{
					if (onCompleted != null)
					{
						onCompleted?.Invoke(null);
						if (m_ShowDebugLog) Debug.Log($"{sceneName} is already loaded, so it's ignored. {m_ScenesCurrentlyLoading} remaining scenes to load.");
					}
					else
					{
						if (m_ShowDebugLog) Debug.Log($"{sceneName} is already loaded, so it's ignored.");
					}

					return;
				}
			}
			// Protect against warning for scenes not included in build
#if !UNITY_EDITOR
			if (Application.CanStreamedLevelBeLoaded(sceneName))
#endif
			{
				if (m_ShowDebugLog) Debug.Log($"Loading {sceneName}... (safeLoad is " + safeLoad + ")");

				var asyncOp = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
				if (onCompleted != null)
					asyncOp.completed += onCompleted;
			}
		}



		private void SetSceneActive(SceneReference activeScene)
		{
			int count = UnityEngine.SceneManagement.SceneManager.sceneCount;
			for (int i = 0; i < count; i++)
			{
				var scene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);

				if (scene.path.Equals(activeScene.ScenePath))
				{
					// Set the first found scene as active
					UnityEngine.SceneManagement.SceneManager.SetActiveScene(scene);

					// Exit
					return;
				}
			}
		}

		private void SetSceneActiveCallback(SceneReference sceneActive, System.Action OnSetLoaded = null)
		{
			m_ScenesCurrentlyLoading--;
			if (m_ShowDebugLog) Debug.Log($"{m_ScenesCurrentlyLoading} remaining scenes");

			if (m_ScenesCurrentlyLoading <= 0)
			{
				if (m_ShowDebugLog) Debug.Log("The last scene of the bunch was loaded");
				if (m_ShowDebugLog && sceneActive != null) Debug.Log("Now setting " + sceneActive + " scene as active ");
				this.ExecuteAtNextFrame(() =>
				{
					if (sceneActive != null)
						SetSceneActive(sceneActive);

					LightProbes.TetrahedralizeAsync();
					FadeOffCanvas(null);

					// Hide VR loading
					OnSetLoaded?.Invoke();
				});
			}
		}
		private void UnloadCallback(System.Action OnSetUnloaded = null)
		{
			m_ScenesCurrentlyUnloading--;
			if (m_ShowDebugLog) Debug.Log($"{m_ScenesCurrentlyUnloading} remaining scenes");

			if (m_ScenesCurrentlyUnloading == 0)
			{
				if (m_ShowDebugLog) Debug.Log("The last scene of the bunch was unloaded");
				this.ExecuteAtNextFrame(() =>
				{
					OnSetUnloaded?.Invoke();
				});
			}
		}

		/// <summary>
		/// Check if the scene is already loaded
		/// </summary>
		/// <returns>True if the scene is loaded</returns>
		private bool IsSceneLoaded(string pSceneName)
		{
			int count = UnityEngine.SceneManagement.SceneManager.sceneCount;
			for (int i = 0; i < count; i++)
			{
				var scene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);
				if (scene.name.Equals(pSceneName) || scene.path.Equals(pSceneName))
					return true;
			}

			return false;
		}

		internal void LoadSceneSet(SceneSet coreSet, bool v1, bool v2, object onCoreSetLoaded)
		{
			throw new NotImplementedException();
		}
	}
}