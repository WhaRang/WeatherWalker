using UnityEngine;

public class Background : MonoBehaviour
{
    private static readonly string FADE_IN_ANIMATION_NAME = "BackgroundFadeInAnimation";
    private static readonly string FADE_OUT_ANIMATION_NAME = "BackgroundFadeOutAnimation";

    [SerializeField] private Animator animator;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private bool filledFromStart;

    public RectTransform RectTransform => rectTransform;

    private int fadeInAnimationHash;
    private int fadeOutAnimationHash;

    private Vector3 startPos;

    private void Awake()
    {
        fadeInAnimationHash = Animator.StringToHash(FADE_IN_ANIMATION_NAME);
        fadeOutAnimationHash = Animator.StringToHash(FADE_OUT_ANIMATION_NAME);

        if (filledFromStart)
            canvasGroup.alpha = 1.0f;
    }

    private void Start()
    {
        startPos = transform.position;
    }

    public void FadeIn()
    {
        animator.Play(fadeInAnimationHash);
    }

    public void FadeOut()
    {
        animator.Play(fadeOutAnimationHash);
    }

    public void ResetToStartPos()
    {
        transform.position = startPos;
    }
}
