using System.Collections.Generic;
using System.Linq;
using David.Utils;
using NaughtyAttributes;
using Tides.Resources;
using UnityEngine;
using UnityEngine.Pool;

public class GatherPointSpawner : MonoBehaviour
{
    [SerializeField] public int GatherPointDefaultResourceAmount = 5;
    [SerializeField] public int GatherPointAmount = 50;
    [SerializeField] private GameObject foodGatherPointBehaviourPrefab;
    [SerializeField] private GameObject woodGatherPointBehaviourPrefab;
    [SerializeField] private Texture2D heightBakeTexture;
    [SerializeField] private Transform waveTransform;
    private GatherPointBehaviour[] GatherPoints;
    private List<Vector3> CurrentSpawningCoordinates;
    private int spawningIndex;

    private IObjectPool<GameObject> foodPool;
    private IObjectPool<GameObject> woodPool;

    private void Awake()
    {
        foodPool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(foodGatherPointBehaviourPrefab),
            actionOnGet: (obj) => obj.gameObject.SetActive(true),
            actionOnRelease: (obj) => obj.gameObject.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj.gameObject),
            collectionCheck: false,
            defaultCapacity: GatherPointAmount / 2,
            maxSize: GatherPointAmount
        );
        woodPool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(woodGatherPointBehaviourPrefab),
            actionOnGet: (obj) => obj.gameObject.SetActive(true),
            actionOnRelease: (obj) => obj.gameObject.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj.gameObject),
            collectionCheck: false,
            defaultCapacity: GatherPointAmount / 2,
            maxSize: GatherPointAmount
        );
    }

    private void OnEnable()
    {
        GatherPoints = new GatherPointBehaviour[GatherPointAmount];
    }

    public void SpawnPoint(Vector3 position, ResourceType resourceType)
    {
        IObjectPool<GameObject> objectPool;
        if (resourceType == ResourceType.FOOD)
            objectPool = foodPool;
        else
            objectPool = woodPool;

        GameObject gatherPoint = objectPool.Get();
        gatherPoint.transform.position = position;

        GatherPointBehaviour existingPoint = GatherPoints[spawningIndex];
        if (existingPoint != null)
        {
            if (existingPoint.ResourceType == ResourceType.FOOD)
            {
                foodPool.Release(GatherPoints[spawningIndex].gameObject);
            }
            else
            {
                woodPool.Release(GatherPoints[spawningIndex].gameObject);
            }
        }

        GatherPoints[spawningIndex] = gatherPoint.GetComponent<GatherPointBehaviour>();
        GatherPoints[spawningIndex].Initialize(resourceType, Mathf.Abs(Mathf.CeilToInt(position.y) * GatherPointDefaultResourceAmount));
        spawningIndex = (spawningIndex + 1) % GatherPointAmount;
    }
    public Vector3[] GetRandomSpawnPositions()
    {
        Vector3[] spawnPoints = new Vector3[GatherPointAmount];
        //TODO : Implement random positions generation
        return spawnPoints;
    }

    private void FixedUpdate()
    {
        if (TidesManager.Instance.currentTide == TidesManager.TideState.Rising)
        {
            foreach (GatherPointBehaviour gatherPointBehaviour in GatherPoints)
            {
                if (!PlaneProjectionHelper.IsPointInFrontOfPlane(waveTransform.transform.position, waveTransform.forward, gatherPointBehaviour.transform.position))
                {
                    if (gatherPointBehaviour.ResourceType == ResourceType.FOOD)
                        foodPool.Release(gatherPointBehaviour.gameObject);
                    else
                        woodPool.Release(gatherPointBehaviour.gameObject);
                }
            }
            return;
        }

        if (TidesManager.Instance.currentTide == TidesManager.TideState.Lowering)
        {
            if (CurrentSpawningCoordinates == null)
                CurrentSpawningCoordinates = GetRandomSpawnPositions().ToList();

            Vector3 point = CurrentSpawningCoordinates.FirstOrDefault(s => PlaneProjectionHelper.IsPointInFrontOfPlane(waveTransform.transform.position, waveTransform.forward, s));
            if (point == null)
            {
                return;
            }
            CurrentSpawningCoordinates.Remove(point);
            SpawnPoint(point, (ResourceType)Random.Range(1, 3));
        }
        else
        {
            if (CurrentSpawningCoordinates != null)
                CurrentSpawningCoordinates = null;
            spawningIndex = 0;
        }
    }
    [Button("Spawn Debug")]
    private void DebugSpawnPoint()
    {
        SpawnPoint(Vector3.zero, ResourceType.WOOD);
    }
}
