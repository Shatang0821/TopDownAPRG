using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public float moveSpeed = 2f; // 移动速度
    public float moveDistance = 1f; // 上下移动的最大距离
    private Vector3 startPos; // 地刺的初始位置

    void Start()
    {
        // 记录地刺的初始位置
        startPos = transform.position;
    }

    void Update()
    {
        // 使用正弦波计算新的 y 位置，并限制向上移动的距离
        float offset = Mathf.Sin(Time.time * moveSpeed) * moveDistance;
        float newY = startPos.y + Mathf.Clamp(offset, -moveDistance, 0.5f * moveDistance); // 限制向上移动范围

        // 更新地刺的 y 坐标
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}
