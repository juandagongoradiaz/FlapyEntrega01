using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class PipeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject pipePrefab;
    [SerializeField] private int PoolSize = 3;
    [SerializeField] private float timeToSpawnFirstPipe;
    [SerializeField] private float timeToSpawnPipe;

    [SerializeField] private Transform pipeSpawnPosition;
    [SerializeField] private Transform pipeMinSpawnHeight;
    [SerializeField] private Transform pipeMaxSpawnHeight;

    private List<GameObject> pipePool = new List<GameObject>();

    private void Start()
    {
        InitializePool();
        StartCoroutine(SpawnPipes());
    }

    private void InitializePool()
    {
        for (int i = 0; i < PoolSize; i++)
        {
            GameObject pipe = Instantiate(pipePrefab);
            pipe.SetActive(false);
            pipePool.Add(pipe);
        }
    }

    private GameObject GetPooledPipe()
    {
        foreach (GameObject pipe in pipePool)
        {
            if (!pipe.activeInHierarchy)
            {
                return pipe;
            }
        }
        return null;
    }

    private void SpawnPipe()
    {
        GameObject pipe = GetPooledPipe();
        if (pipe != null)
        {
            pipe.transform.position = GetPipePosition();
            pipe.SetActive(true);
            StartCoroutine(ReturnToPool(pipe));
        }
    }

    private Vector3 GetPipePosition()
    {
        return new Vector3(pipeSpawnPosition.position.x, GetPipeHeight(), pipeSpawnPosition.position.z);
    }

    private float GetPipeHeight()
    {
        return Random.Range(pipeMinSpawnHeight.position.y, pipeMaxSpawnHeight.position.y);
    }

    private IEnumerator SpawnPipes()
    {
        yield return new WaitForSeconds(timeToSpawnFirstPipe);

        SpawnPipe();

        WaitForSeconds timeToSpawnPipeWaitForSeconds = new WaitForSeconds(timeToSpawnPipe);

        while (!GameManager.Instance.isGameOver)
        {
            yield return timeToSpawnPipeWaitForSeconds;
            SpawnPipe();
        }
    }

    private IEnumerator ReturnToPool(GameObject pipe)
    {
        yield return new WaitForSeconds(5f);
        pipe.SetActive(false);
    }
}
