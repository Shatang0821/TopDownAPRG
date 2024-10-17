    using System.Collections;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    // 该类实现了按钮的悬停效果，继承自 MonoBehaviour 并实现了多种接口以处理鼠标事件
    public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        private Button button; // 存储与此效果关联的按钮组件
        private Vector3 originalScale; // 存储按钮的原始缩放值
        private Coroutine scaleCoroutine; // 存储当前的缩放协程，用于处理按钮缩放效果

        // Start 方法在脚本初始化时调用
        void Start()
        {
            button = GetComponent<Button>(); // 获取与此 GameObject 关联的 Button 组件
            originalScale = transform.localScale; // 记录按钮的初始缩放值
        }

        // 设置按钮的原始缩放值
        public void SetOriginalScale(Vector3 scale)
        {
            originalScale = scale; // 更新原始缩放值
            button = GetComponent<Button>();  // 确保按钮已初始化
        }

        // 当鼠标进入按钮区域时调用
        public void OnPointerEnter(PointerEventData eventData)
        {
            // 确保按钮不为空并且可以交互
            if (button == null) button = GetComponent<Button>();
            if (button.interactable)
            {
                ScaleButton(originalScale * 1.8f); // 放大按钮
            }
        }

        // 当鼠标离开按钮区域时调用
        public void OnPointerExit(PointerEventData eventData)
        {
            // 确保按钮不为空并且可以交互
            if (button == null) button = GetComponent<Button>();
            if (button.interactable)
            {
                ScaleButton(originalScale); // 恢复按钮到原始大小
            }
        }

        // 当按钮被按下时调用
        public void OnPointerDown(PointerEventData eventData)
        {
            // 确保按钮不为空并且可以交互
            if (button == null) button = GetComponent<Button>();
            if (button.interactable)
            {
                ScaleButton(originalScale * 0.5f); // 缩小按钮
            }
        }

        // 当按钮释放时调用
        public void OnPointerUp(PointerEventData eventData)
        {
            // 确保按钮不为空并且可以交互
            if (button == null) button = GetComponent<Button>();
            if (button.interactable)
            {
                // 使用协程延迟恢复到原始大小
                StartCoroutine(DelayedScale(originalScale, 0.2f)); // 恢复到原始大小，延迟0.2秒
            }
        }

        // 当按钮被选中时调用（通过键盘导航等）
        public void OnSelect(BaseEventData eventData)
        {
            // 确保按钮不为空并且可以交互
            if (button == null) button = GetComponent<Button>();
            if (button.interactable)
            {
                ScaleButton(originalScale * 1.8f); // 放大按钮
            }
        }

        // 当按钮失去选中状态时调用
        public void OnDeselect(BaseEventData eventData)
        {
            // 确保按钮不为空并且可以交互
            if (button == null) button = GetComponent<Button>();
            ScaleButton(originalScale); // 恢复按钮到原始大小
        }

        // 缩放按钮到目标大小
        private void ScaleButton(Vector3 targetScale)
        {
            // 如果存在缩放协程，停止它
            if (scaleCoroutine != null)
            {
                StopCoroutine(scaleCoroutine);
            }
            // 启动新的缩放协程
            scaleCoroutine = StartCoroutine(ScaleTo(targetScale, 0.2f)); // 将按钮缩放到目标大小，持续时间0.2秒
        }

        // 协程：将按钮缩放到目标大小
        private IEnumerator ScaleTo(Vector3 targetScale, float duration)
        {
            Vector3 startScale = transform.localScale; // 获取当前的缩放值
            float time = 0f; // 初始化时间计数器

            // 在持续时间内逐渐缩放
            while (time < duration)
            {
                transform.localScale = Vector3.Lerp(startScale, targetScale, time / duration); // 线性插值计算缩放值
                time += Time.deltaTime; // 增加时间
                yield return null; // 等待下一帧
            }

            transform.localScale = targetScale; // 确保最终缩放到目标大小
        }

        // 协程：延迟缩放到目标大小
        private IEnumerator DelayedScale(Vector3 targetScale, float delay)
        {
            yield return new WaitForSeconds(delay); // 等待指定延迟时间
            ScaleButton(targetScale); // 调用缩放方法
        }
    }
