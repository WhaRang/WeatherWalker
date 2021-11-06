using UnityEngine;

public class Background : MonoBehaviour
{
    private static readonly string FADE_IN_ANIMATION_NAME = "BackgroundFadeInAnimation";
    private static readonly string FADE_OUT_ANIMATION_NAME = "BackgroundFadeOutAnimation";

    [SerializeField] private Animator animator;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private bool filledFromStart;

    private int fadeInAnimationHash;
    private int fadeOutAnimationHash;

    private void Awake()
    {
        fadeInAnimationHash = Animator.StringToHash(FADE_IN_ANIMATION_NAME);
        fadeOutAnimationHash = Animator.StringToHash(FADE_OUT_ANIMATION_NAME);

        if (filledFromStart)
            canvasGroup.alpha = 1.0f;
    }

    public void FadeIn()
    {
        animator.Play(fadeInAnimationHash);
    }

    public void FadeOut()
    {
        animator.Play(fadeOutAnimationHash);
    }
}
