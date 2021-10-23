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
        Vector3 newPos = new Vector3(0.0f, 0.0f, mainMenuTransform.position.z);

        MovingAnimations.Instance.MoveObjTo(mainMenuTransform.gameObject,
            newPos, mainMenuAnimationDuration);
    }

    public void CloseMainMenu()
    {
        Vector3 newPos = new Vector3(-Screen.width, 0.0f, 0.0f);

        MovingAnimations.Instance.MoveObjTo(mainMenuTransform.gameObject,
            mainMenuTransform.TransformPoint(newPos), mainMenuAnimationDuration);
    }
}
