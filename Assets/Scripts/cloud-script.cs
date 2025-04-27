using UnityEngine;

public class Cloud : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float despawnX = -20f;

    private void Update()
    {
        // 구름을 왼쪽으로 이동
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        
        // 화면 밖으로 벗어나면 제거
        if (transform.position.x < despawnX)
        {
            Destroy(gameObject);
        }
    }
}
