using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    [SerializeField] private List<Background> backgrounds;
    [SerializeField] private float backgroundMovingSpeed = 0.0f;

    [SerializeField] private float rectTransformPosX = 0.0f;
    [SerializeField] private float backgroundEndPosX = 0.0f;

    private int currBackgroundIndex = 0;
    private int prevBackgroundIndex = 0;

    private void Start()
    {
        CalculateBackgroundEndPosX();
    }

    public void UpdateController()
    {
        UpdateBackgroundPos();
        CheckBackgroundPos();
    }

    private void UpdateBackgroundPos()
    {
        UpdateCurrentBackgroundPos();
        UpdatePreviousBackgroundPos();
    }

    private void UpdateCurrentBackgroundPos()
    {
        Vector3 startPos = backgrounds[currBackgroundIndex].transform.position;
        startPos.x -= Time.deltaTime * backgroundMovingSpeed;

        backgrounds[currBackgroundIndex].transform.position = startPos;
    }

    private void UpdatePreviousBackgroundPos()
    {
        Vector3 startPos = backgrounds[prevBackgroundIndex].transform.position;
        startPos.x -= Time.deltaTime * backgroundMovingSpeed;

        backgrounds[prevBackgroundIndex].transform.position = startPos;
    }

    private void CheckBackgroundPos()
    {
        rectTransformPosX = backgrounds[currBackgroundIndex].RectTransform.anchoredPosition.x;

        if (backgrounds[currBackgroundIndex].RectTransform.anchoredPosition.x <= backgroundEndPosX)
        {
            backgrounds[currBackgroundIndex].FadeOut();

            prevBackgroundIndex = currBackgroundIndex;
            currBackgroundIndex++;

            if (currBackgroundIndex >= backgrounds.Count)
                currBackgroundIndex = 0;

            backgrounds[currBackgroundIndex].ResetToStartPos();
            backgrounds[currBackgroundIndex].FadeIn();

            CalculateBackgroundEndPosX();
        }
    }

    private void CalculateBackgroundEndPosX()
    {
        backgroundEndPosX = backgrounds[currBackgroundIndex].RectTransform.anchoredPosition.x
            - (backgrounds[currBackgroundIndex].RectTransform.rect.width - Screen.width);
    }
}
