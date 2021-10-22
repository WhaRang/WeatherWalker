using UnityEngine;

public class AnimationControllerUI : MonoBehaviour
{
    public static AnimationControllerUI Instance = null;

    [SerializeField] private Transform mainMenuTransform = null;
    [SerializeField] private float mainMenuAnimationDuration = 0.0f;

    private void Awake()
    {
        Instance = this;
    }

    public void OpenMainMenu()
    {
        MovingAnimations.Instance.MoveObjTo(mainMenuTransform.gameObject,
            Vector3.zero, mainMenuAnimationDuration);
    }

    public void CloseMainMenu()
    {
        MovingAnimations.Instance.MoveObjTo(mainMenuTransform.gameObject,
            new Vector3(-Screen.width, 0.0f, 0.0f), mainMenuAnimationDuration);
    }
}
