using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private float leftEdge;

    private void Awake()
    {
        leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 2f;
    }

    private void Update()
    {
        transform.position += GameManager.Instance.gameSpeed * Time.deltaTime * Vector3.left;

        if (transform.position.x < leftEdge) {
            // 화면 밖으로 나가면 파괴
            Destroy(gameObject);
        }
    }
}
