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
    [SerializeField] private Vector3 worldCenter;
    [SerializeField] private Vector3 worldSize = new Vector3(200, 20, 200);
    [SerializeField] private int minYOffset = -5;
    [SerializeField] private int maxYOffset = 0;

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
        InitializeGatherPoints();
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
        GatherPoints[spawningIndex].Initialize(resourceType, Mathf.Abs(Mathf.CeilToInt(position.y - 1) * GatherPointDefaultResourceAmount));
        spawningIndex = (spawningIndex + 1) % GatherPointAmount;
    }
    public Vector3[] GetRandomSpawnPositions()
    {
        Vector3[] spawnPoints = new Vector3[GatherPointAmount];
        if (heightBakeTexture == null) return spawnPoints;

        int texWidth = heightBakeTexture.width;
        int texHeight = heightBakeTexture.height;
        int count = 0;
        int attempts = 0;
        int maxAttempts = GatherPointAmount * 50;

        while (count < GatherPointAmount && attempts < maxAttempts)
        {
            attempts++;
            int u = Random.Range(0, texWidth);
            int v = Random.Range(0, texHeight);

            Color pixel = heightBakeTexture.GetPixel(u, v);

            // R < 0.5f (Height)
            // G > 0 (Valid area)
            if (pixel.r <= 0.55f && pixel.g > 0.01f)
            {
                float x = worldCenter.x + ((float)u / texWidth - 0.5f) * worldSize.x;
                float z = worldCenter.z + ((float)v / texHeight - 0.5f) * worldSize.z;

                // Map R [0, 0.5] to [minYOffset, maxYOffset] as an integer
                float normalizedR = pixel.r / 0.5f;
                int yOffset = Mathf.RoundToInt(Mathf.Lerp(minYOffset, maxYOffset, normalizedR));

                // Final world Y (allowing negative values as per requested mapping)
                float y = worldCenter.y + yOffset;

                // Reject points below the current cycle's RestHeight
                float restHeight = TidesManager.Instance.tideCyclesSO.tideCycles[TidesManager.Instance.currentCycleIndex].RestHeight;
                if (y < restHeight + 1)
                {
                    continue;
                }

                spawnPoints[count] = new Vector3(x, y, z);
                count++;
            }
        }

        return spawnPoints;
    }

    private float lastSpawnTime;
    [SerializeField] private float spawnCooldown = 0.1f;

    private void FixedUpdate()
    {
        if (TidesManager.Instance.currentTide == TidesManager.TideState.Rising)
        {
            for (int i = 0; i < GatherPoints.Length; i++)
            {
                GatherPointBehaviour point = GatherPoints[i];
                if (point == null) continue;

                // If wave is rising and point is now behind the wave front, despawn it
                if (!PlaneProjectionHelper.IsPointInFrontOfPlane(waveTransform.position, waveTransform.forward, point.transform.position))
                {
                    if (point.ResourceType == ResourceType.FOOD)
                        foodPool.Release(point.gameObject);
                    else
                        woodPool.Release(point.gameObject);

                    GatherPoints[i] = null;
                }
            }
            return;
        }

        if (TidesManager.Instance.currentTide == TidesManager.TideState.Lowering)
        {
            if (CurrentSpawningCoordinates == null)
            {
                CurrentSpawningCoordinates = GetRandomSpawnPositions().ToList();
                spawningIndex = 0;
            }

            if (Time.time < lastSpawnTime + spawnCooldown) return;

            Vector3 spawnPos = CurrentSpawningCoordinates.FirstOrDefault(s => PlaneProjectionHelper.IsPointInFrontOfPlane(waveTransform.position, waveTransform.forward, s));

            if (spawnPos != default)
            {
                CurrentSpawningCoordinates.Remove(spawnPos);
                SpawnPoint(spawnPos, (ResourceType)Random.Range(1, 3));
                lastSpawnTime = Time.time;
            }
        }
        else
        {
            if (CurrentSpawningCoordinates != null)
                CurrentSpawningCoordinates = null;
        }
    }
    [Button("Initialize Gather Points")]
    public void InitializeGatherPoints()
    {
        // Generate new coordinates if none exist
        CurrentSpawningCoordinates = GetRandomSpawnPositions().ToList();
        spawningIndex = 0;

        // Reset tracking array to avoid duplicates
        for (int i = 0; i < GatherPoints.Length; i++)
        {
            if (GatherPoints[i] != null)
            {
                if (GatherPoints[i].ResourceType == ResourceType.FOOD)
                    foodPool.Release(GatherPoints[i].gameObject);
                else
                    woodPool.Release(GatherPoints[i].gameObject);

                GatherPoints[i] = null;
            }
        }

        // Spawn all points that are currently "behind" the wave (revealed)
        List<Vector3> toSpawn = CurrentSpawningCoordinates
            .Where(s => PlaneProjectionHelper.IsPointInFrontOfPlane(waveTransform.position, waveTransform.forward, s))
            .ToList();

        foreach (Vector3 pos in toSpawn)
        {
            SpawnPoint(pos, (ResourceType)Random.Range(1, 3));
            CurrentSpawningCoordinates.Remove(pos);
        }
    }

    [Button("Spawn Debug")]
    private void DebugSpawnPoints()
    {
        SpawnPoint(Vector3.zero, ResourceType.WOOD);
    }
}
