using System.Collections.Generic;
using UnityEngine;

public class TimedClickSpawner : MonoBehaviour
{
    [SerializeField] private List<RectTransform> timedClickPositionsGrid;
    [SerializeField] private GameObject timedClickPrefab;
    [SerializeField] private float minSpawnTime;
    [SerializeField] private float maxSpawnTime;
    [SerializeField] private int minTimedClicksAmount;
    [SerializeField] private int maxTimedClicksAmount;

    private float spawnTime = 0.0f;
    private float counter = 0.0f;

    private List<int> spawnIndexList = new List<int>();
    private List<TimedClick> spawnedTimedClicksList = new List<TimedClick>();

    private void Start()
    {
        CalculateSpawnTime();
        RefillSpawnIndexSet();
    }

    public void UpdateSpawner()
    {
        counter += Time.deltaTime;
        if (counter >= spawnTime)
        {
            SpawnTimedClicks();
            CalculateSpawnTime();
            counter = 0.0f;
        }

        UpdateSpawnedTimedClicks();
    } 

    public void DestroyAllClicksFadeOut()
    {
        spawnedTimedClicksList.RemoveAll(x => x == null);

        foreach (TimedClick t in spawnedTimedClicksList)
            t.FadeOutDestroy();
    }

    private void UpdateSpawnedTimedClicks()
    {
        spawnedTimedClicksList.RemoveAll(x => x == null);

        foreach (TimedClick t in spawnedTimedClicksList)
            t.UpdateClick();
    }

    private void CalculateSpawnTime()
    {
        spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
    }

    private void SpawnTimedClicks()
    {
        int currClicksAmount = Random.Range(minTimedClicksAmount, maxTimedClicksAmount + 1);
        for (int i = 0; i < currClicksAmount; i++)
        {
            int randomIndex = spawnIndexList[Random.Range(0, spawnIndexList.Count)];
            spawnIndexList.Remove(randomIndex);

            GameObject spawnedTimedClick = Instantiate(timedClickPrefab);
            spawnedTimedClick.transform.SetParent(timedClickPositionsGrid[randomIndex]);
            spawnedTimedClick.transform.localScale = timedClickPositionsGrid[randomIndex].localScale;
            spawnedTimedClick.transform.position = timedClickPositionsGrid[randomIndex].transform.position;

            spawnedTimedClicksList.Add(spawnedTimedClick.GetComponent<TimedClick>());
        }

        RefillSpawnIndexSet();
    }

    private void RefillSpawnIndexSet()
    {
        spawnIndexList = new List<int>();
        for (int i = 0; i < timedClickPositionsGrid.Count; i++)
            spawnIndexList.Add(i);
    }
}
