using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private Button button;
    private Vector3 originalScale;
    private Coroutine scaleCoroutine;

    void Start()
    {
        button = GetComponent<Button>();
        originalScale = transform.localScale;
    }

    public void SetOriginalScale(Vector3 scale)
    {
        originalScale = scale;
        button = GetComponent<Button>();  // Ensure button is initialized
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (button == null) button = GetComponent<Button>();  // Ensure button is not null
        if (button.interactable)
        {
            ScaleButton(originalScale * 1.8f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (button == null) button = GetComponent<Button>();  // Ensure button is not null
        if (button.interactable)
        {
            ScaleButton(originalScale);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (button == null) button = GetComponent<Button>();  // Ensure button is not null
        if (button.interactable)
        {
            ScaleButton(originalScale * 0.5f);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (button == null) button = GetComponent<Button>();  // Ensure button is not null
        if (button.interactable)
        {
            // 使用协程延迟恢复到原始大小
            StartCoroutine(DelayedScale(originalScale, 0.2f));
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (button == null) button = GetComponent<Button>();  // Ensure button is not null
        if (button.interactable)
        {
            ScaleButton(originalScale * 1.8f);
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (button == null) button = GetComponent<Button>();  // Ensure button is not null
        ScaleButton(originalScale);
    }

    private void ScaleButton(Vector3 targetScale)
    {
        if (scaleCoroutine != null)
        {
            StopCoroutine(scaleCoroutine);
        }
        scaleCoroutine = StartCoroutine(ScaleTo(targetScale, 0.2f));
    }

    private IEnumerator ScaleTo(Vector3 targetScale, float duration)
    {
        Vector3 startScale = transform.localScale;
        float time = 0f;

        while (time < duration)
        {
            transform.localScale = Vector3.Lerp(startScale, targetScale, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
    }

    private IEnumerator DelayedScale(Vector3 targetScale, float delay)
    {
        yield return new WaitForSeconds(delay);
        ScaleButton(targetScale);
    }
}
