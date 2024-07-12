using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsManager : MonoBehaviour
{
    [SerializeField] Image fillImageBack;
    [SerializeField] Image fillImageFront;

    [SerializeField] bool delayFille = true;
    [SerializeField] float fillDelay = 0.5f;
    [SerializeField] float fillSpeed = 0.1f;

    float currentFillAmout;
    protected float targetFillAmout;
    float previousFillAmount;

    float t;

    WaitForSeconds waitForDelayFill;

    Coroutine bufferedFillingCoroutine;

    Canvas canvas;

    private void Awake()
    {
        if (TryGetComponent<Canvas>(out Canvas canvas))
        {
            canvas.worldCamera = Camera.main;
        }
        waitForDelayFill = new WaitForSeconds(fillDelay);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public virtual void Initialize(float currentValue, float maxValue)
    {
        // 调试信息：确保引用已分配
        Debug.Log("Initialize called");
        Debug.Log("fillImageBack: " + (fillImageBack != null ? "Assigned" : "Not Assigned"));
        Debug.Log("fillImageFront: " + (fillImageFront != null ? "Assigned" : "Not Assigned"));

        currentFillAmout = currentValue / maxValue;
        targetFillAmout = currentFillAmout;
        fillImageBack.fillAmount = currentFillAmout;
        fillImageFront.fillAmount = currentFillAmout;

        Debug.Log("Initial Fill Amount: " + currentFillAmout);
    }

    public void UpdateStats(float currentValue, float maxValue)
    {
        targetFillAmout = currentValue / maxValue;
        Debug.Log("UpdateStats called. Target Fill Amount: " + targetFillAmout);

        if (bufferedFillingCoroutine != null)
        {
            StopCoroutine(bufferedFillingCoroutine);
        }

        if (currentFillAmout > targetFillAmout)
        {
            fillImageFront.fillAmount = targetFillAmout;
            bufferedFillingCoroutine = StartCoroutine(BufferedFillingCoroutine(fillImageBack));
            return;
        }

        if (currentFillAmout < targetFillAmout)
        {
            fillImageBack.fillAmount = targetFillAmout;
            bufferedFillingCoroutine = StartCoroutine(BufferedFillingCoroutine(fillImageFront));
        }
    }

    protected virtual IEnumerator BufferedFillingCoroutine(Image image)
    {
        if (delayFille)
        {
            yield return waitForDelayFill;
        }

        previousFillAmount = currentFillAmout;
        t = 0;

        while (t < 1f)
        {
            t += Time.deltaTime * fillSpeed;
            currentFillAmout = Mathf.Lerp(previousFillAmount, targetFillAmout, t);
            image.fillAmount = currentFillAmout;

            Debug.Log("Buffered Filling - Current Fill Amount: " + currentFillAmout);

            yield return null;
        }
    }
}
