using System.Collections;
using UnityEngine;

public class MovingAnimations : MonoBehaviour
{
    public static MovingAnimations Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void MoveObjTo(GameObject obj, Vector3 newPos, float seconds)
    {
        StartCoroutine(SmoothMove(obj, newPos, seconds));
    }

    private IEnumerator SmoothMove(GameObject obj, Vector3 endPos, float seconds)
    {
        Vector3 startPos = obj.transform.position;

        float t = 0f;
        while (t <= 1f)
        {
            if (obj == null)
                break;

            t += Time.deltaTime / seconds;
            obj.transform.position = Vector3.Lerp(startPos, endPos, Mathf.SmoothStep(0f, 1f, t));

            yield return null;
        }
    }

    public void RotateObjTo(GameObject obj, Quaternion newRot, float seconds)
    {
        StartCoroutine(SmoothRotation(obj, newRot, seconds));
    }

    private IEnumerator SmoothRotation(GameObject obj, Quaternion endRot, float seconds)
    {
        Quaternion startRot = obj.transform.rotation;

        float t = 0f;
        while (t <= 1f)
        {
            if (obj == null)
                break;

            t += Time.deltaTime / seconds;
            obj.transform.rotation = Quaternion.Slerp(startRot, endRot, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
    }

    public void MakePulse(GameObject obj, float scaler, float seconds)
    {
        StartCoroutine(MakePulseCoroutine(obj, scaler, seconds));
    }

    private IEnumerator MakePulseCoroutine(GameObject obj, float scaler, float seconds)
    {
        Vector3 startScale = obj.transform.localScale;
        Vector3 peakScaler = obj.transform.localScale;

        peakScaler.x *= scaler;
        peakScaler.y *= scaler;
        peakScaler.z *= scaler;

        SmoothScaling(obj, peakScaler, seconds / 2);
        yield return new WaitForSeconds(seconds / 2);
        SmoothScaling(obj, startScale, seconds / 2);
    }

    public void SmoothScaling(GameObject obj, Vector3 newScale, float seconds)
    {
        StartCoroutine(SmoothScaleChanging(obj, newScale, seconds));
    }

    private IEnumerator SmoothScaleChanging(GameObject obj, Vector3 newScale, float seconds)
    {
        Vector3 oldScale = obj.transform.localScale;

        float t = 0f;
        while (t <= 1f)
        {
            if (obj == null)
                break;

            t += Time.deltaTime / seconds;
            obj.transform.localScale = Vector3.Lerp(oldScale, newScale, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
    }
}