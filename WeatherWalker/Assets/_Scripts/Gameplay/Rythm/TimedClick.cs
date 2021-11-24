using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TimedClick : MonoBehaviour
{
    [SerializeField] private Image center;
    [SerializeField] private Image background;
    [SerializeField] private Animator animator;
    [SerializeField] private float fadeInAllocatedTime;
    [SerializeField] private float fadeOutAllocatedTime;
    [SerializeField] private float narrowingAllocatedTime;

    private const string FADE_IN_ANIM = "FadeIn";
    private const string FADE_OUT_ANIM = "FadeOut";

    private Coroutine scalingCoroutine;

    private int fadeInAnimHash;
    private int fadeOutAnimHash;

    private bool isFadedIn = false;
    private bool isNarrowed = false;
    private bool isClickable = true;

    private Vector3 narrowScale = Vector3.zero;
    private float counter = 0.0f;

    private void Awake()
    {
        fadeInAnimHash = Animator.StringToHash(FADE_IN_ANIM);
        fadeOutAnimHash = Animator.StringToHash(FADE_OUT_ANIM);

        narrowScale.x = center.rectTransform.rect.width / background.rectTransform.rect.width;
        narrowScale.y = center.rectTransform.rect.height / background.rectTransform.rect.height;
    }

    private void Start()
    {
        animator.SetTrigger(fadeInAnimHash);
    }

    public void UpdateClick()
    {
        counter += Time.deltaTime;
        if (!isFadedIn && counter >= fadeInAllocatedTime)
        {
            isFadedIn = true;
            scalingCoroutine = MovingAnimations.Instance.SmoothScaling(
                background.gameObject, narrowScale, narrowingAllocatedTime);
            counter = 0.0f;
        }
        else if (!isNarrowed && counter >= narrowingAllocatedTime)
        {
            animator.SetTrigger(fadeOutAnimHash);
            counter = 0.0f;
            isNarrowed = true;
            isClickable = false;
        }
        else if (isNarrowed && counter >= fadeOutAllocatedTime)
        {
            DestroyClick();
        }
    }

    public void CenterOnClick()
    {
        if (!isClickable)
            return;

        isClickable = false;
        isNarrowed = false;
        counter = narrowingAllocatedTime;
        MovingAnimations.Instance.StopAnimation(scalingCoroutine);
    }

    public void FadeOutDestroy()
    {
        StartCoroutine(FadeOutDestroyCoroutine());
    }

    private IEnumerator FadeOutDestroyCoroutine()
    {
        if (isClickable)
            animator.SetTrigger(fadeOutAnimHash);

        yield return new WaitForSeconds(fadeOutAllocatedTime);
        DestroyClick();
    }

    public void DestroyClick()
    {
        Destroy(this.gameObject);
    }
}
