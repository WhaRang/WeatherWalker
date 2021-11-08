using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    private static readonly float DELTA_POS_X_PART = 0.025f;

    [SerializeField] private MusicGenre musicGenre;
    [SerializeField] private List<Background> backgrounds;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private float backgroundMovingSpeed = 0.0f;

    public MusicGenre MusicGenre => musicGenre;

    private float backgroundEndPosX = 0.0f;
    private float deltaEndPosX = 0.0f;
    private int currBackgroundIndex = 0;
    private int prevBackgroundIndex = 0;

    private void Start()
    {
        CalculateBackgroundEndPosX();
        deltaEndPosX = DELTA_POS_X_PART * backgrounds[currBackgroundIndex].RectTransform.rect.width;
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
        if (backgrounds[currBackgroundIndex].RectTransform.anchoredPosition.x - deltaEndPosX <= backgroundEndPosX)
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
            - (backgrounds[currBackgroundIndex].RectTransform.rect.width - rectTransform.rect.width);
    }
}
