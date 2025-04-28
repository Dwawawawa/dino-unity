using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Spawner : MonoBehaviour
{
    [System.Serializable]
    public struct SpawnableObject
    {
        public GameObject prefab;
        [Range(0f, 1f)]
        public float spawnChance;
    }

    public SpawnableObject[] objects;
    public float minSpawnRate = 1f;
    public float maxSpawnRate = 2f;

    private float addRate = 0f;


    private void OnEnable()
    {
        
        Invoke(nameof(Spawn), Random.Range(minSpawnRate, maxSpawnRate));
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void Spawn()
    {
        float spawnChance = Random.value;
        int selectedIndex = -1;

        for (int i = 0; i <= 6; i++)
        {
            SpawnableObject obj = objects[i];
            if (spawnChance < obj.spawnChance)
            {
                SpawnObstacle(obj, i);
                selectedIndex = i;
                break;
            }

            spawnChance -= obj.spawnChance;
        }


        for (int i = 7; i <= 7 && i < objects.Length; i++)
        {
            SpawnableObject obj = objects[i];
            if (spawnChance < obj.spawnChance)
            {
                SpawnObstacle(obj, i);
                selectedIndex = i;
                break;
            }
        }

        //게임 속도에 맞게 스폰하기
        if (GameManager.Instance.Score < 25 && addRate != 0)
        {
            addRate = 0f;
            Debug.Log(addRate);
        }
        else {
            addRate -= Time.deltaTime;
            Debug.Log(addRate);
        }
        Invoke(nameof(Spawn), Random.Range(minSpawnRate + addRate, maxSpawnRate + addRate));
    }

    private void SpawnObstacle(SpawnableObject spawnableObj, int index)
    {
        // 위치 결정
        Vector3 spawnPosition = transform.position;
        int result = UnityEngine.Random.Range(0, 2);
        int result1 = UnityEngine.Random.Range(0, 2);

        if (index == 6)
        {
            if (result == 0)
                spawnPosition += new Vector3(0, 0.4f, 0);
            else
                spawnPosition += new Vector3(0, 0.8f, 0);
        }

        else if (index == 7)
        {
            if (result1 == 0)
                spawnPosition += new Vector3(0, 1.4f, 0);
            else if (result1 == 1)
                spawnPosition += new Vector3(0, 1.6f, 0);
        }

        // 오브젝트 직접 생성
        GameObject obstacle = Instantiate(spawnableObj.prefab);
        obstacle.transform.position = spawnPosition;
    }
}
