using UnityEngine;

public class DoornimationOnce : MonoBehaviour
{
    private Animator anim; // 声明 Animator 组件
    private int animTriggerHash; // 声明动画触发器的哈希值
    private bool hasPlayed; // 声明一个标志位，表示动画是否已经播放

    private void Start()
    {
        anim = GetComponent<Animator>(); // 获取 Animator 组件
        animTriggerHash = Animator.StringToHash("Door Animation"); // 将动画名称转换为哈希值
        anim.SetTrigger(animTriggerHash); // 触发动画播放
        hasPlayed = false; // 初始化标志位为 false
    }

    private void Update()
    {
        // 检查动画是否已经播放完毕且动画状态为 "Door Animation"
        if (!hasPlayed && anim.GetCurrentAnimatorStateInfo(0).IsName("Door Animation") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            anim.enabled = false; // 禁用 Animator 组件
            hasPlayed = true; // 将标志位设置为 true，表示动画已经播放
        }
    }
}
