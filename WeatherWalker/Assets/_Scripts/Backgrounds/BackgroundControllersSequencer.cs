using System.Collections.Generic;
using UnityEngine;

public class BackgroundControllersSequencer : MonoBehaviour
{
    [SerializeField] private List<BackgroundController> backgroundControllers;

    public void ActivateControllerNeeded(int index)
    {
        backgroundControllers[index].gameObject.SetActive(true);
    }

    public void DisableAllControllers()
    {
        foreach (BackgroundController backgroundController in backgroundControllers)
            backgroundController.gameObject.SetActive(false);
    }

    public void UpdateControllersSequencer(int controllerToUpdate)
    {
        backgroundControllers[controllerToUpdate].UpdateController();
    }
}
