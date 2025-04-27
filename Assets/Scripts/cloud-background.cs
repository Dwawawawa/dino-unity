using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    [System.Serializable]
    public struct CloudData
    {
        public GameObject prefab;
        [Range(0f, 1f)]
        public float spawnChance;
        public float minHeight;
        public float maxHeight;
        public float minSpeed;
        public float maxSpeed;
    }

    public CloudData[] clouds;
    public float minSpawnRate = 2f;
    public float maxSpawnRate = 5f;
    public float despawnX = -20f;  // 화면 왼쪽 너머 구름 제거 지점

    private void OnEnable()
    {
        Invoke(nameof(SpawnCloud), Random.Range(minSpawnRate, maxSpawnRate));
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void SpawnCloud()
    {
        float spawnChance = Random.value;
        
        for (int i = 0; i < clouds.Length; i++)
        {
            CloudData cloudData = clouds[i];
            
            if (spawnChance < cloudData.spawnChance)
            {
                // 구름 생성
                GameObject cloud = Instantiate(cloudData.prefab);
                
                // 위치 설정
                float height = Random.Range(cloudData.minHeight, cloudData.maxHeight);
                cloud.transform.position = transform.position + new Vector3(0, height, 0);
                
                // 이동 속도 설정
                float speed = Random.Range(cloudData.minSpeed, cloudData.maxSpeed);
                Cloud cloudComponent = cloud.AddComponent<Cloud>();
                cloudComponent.moveSpeed = speed;
                cloudComponent.despawnX = despawnX;
                
                break;
            }
            
            spawnChance -= cloudData.spawnChance;
        }

        // 다음 구름 생성 시간 설정
        Invoke(nameof(SpawnCloud), Random.Range(minSpawnRate, maxSpawnRate));
    }
}
