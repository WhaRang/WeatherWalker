using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    [SerializeField] private Animator characterAnimator;

    private const string IDLE_ANIMATION = "Idle";
    private const string WALK_ANIMATION = "Walk";

    private int characterIdleAnimationHash;
    private int characterWalkAnimationHash;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        characterIdleAnimationHash = Animator.StringToHash(IDLE_ANIMATION);
        characterWalkAnimationHash = Animator.StringToHash(WALK_ANIMATION);
    }

    public void Walk()
    {
        characterAnimator.SetTrigger(characterWalkAnimationHash);
    }

    public void Idle()
    {
        characterAnimator.SetTrigger(characterIdleAnimationHash);
    }
}
