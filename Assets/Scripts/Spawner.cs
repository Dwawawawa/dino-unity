using UnityEngine;

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
        

        for (int i = 0 ; i < 6; i++ ) // SpawnableObject obj in objects
        {
            SpawnableObject obj = objects[i];
            if (spawnChance < obj.spawnChance)
            {
                GameObject obstacle = Instantiate(obj.prefab);

                int result = UnityEngine.Random.Range(0, 2);

                if (i == 6 && result == 0)
                {
                    obstacle.transform.position += transform.position + new Vector3(0, 0.8f, 0);
                }
                else {
                    obstacle.transform.position += transform.position;
                }
                selectedIndex = i;
                break;
            }

            spawnChance -= obj.spawnChance;
        }

        for (int i = 7; i <= 7; i++) // SpawnableObject obj in objects
        {
            SpawnableObject obj = objects[i];
            if (spawnChance < obj.spawnChance)
            {
                GameObject obstacle = Instantiate(obj.prefab);

                int result = UnityEngine.Random.Range(0, 2);

                if (i==7 && result == 0)
                {
                    obstacle.transform.position += transform.position + new Vector3(0, 0.8f, 0);
                }
                else if (i==7 && result == 1)
                {
                    obstacle.transform.position += transform.position + new Vector3(0, 1.4f, 0);
                }

                selectedIndex = i;
                break;
            }
        }

        Invoke(nameof(Spawn), Random.Range(minSpawnRate, maxSpawnRate));
    }

}
