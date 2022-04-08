using UnityEngine;
using System.Collections;

// copied and adapted 
// https://gist.github.com/ftvs/5822103

public class CameraShake : MonoBehaviour
{
    // Transform of the camera to shake. Grabs the gameObject's transform
    // if null.
    public Transform camTransform;

    // Should the camera shake?
    public bool shaking;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount = 0.7f;

    public float shakeTime = -1f;

    Vector3 originalPos;

    void Awake()
    {
        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    void OnEnable()
    {
        originalPos = camTransform.localPosition;
    }

    void Update()
    {
        if (shaking || shakeTime > 0f)
        {
            camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
        }
        else
        {
            camTransform.localPosition = originalPos;
            shakeAmount = Mathf.Lerp(shakeAmount, 0, Time.deltaTime * 0.5f);
        }
        if (shakeTime > 0f)
            shakeTime -= Time.deltaTime;
    }

    public void SetShakeTimer(float time, float mul = 0.5f)
    {
        shakeTime += time;

        StartCoroutine(shakeSet(mul));
    }

    public void SetShakeMultiplier(float mul, float limit = -1)
    {
        StartCoroutine(shakeSet(mul));
        if(limit != -1)
            shakeAmount = Mathf.Clamp(shakeAmount, 0, limit);
    }

    IEnumerator shakeSet(float amount)
    {
        float time = 0f;
        while (time < 2)
        {
            shakeAmount = Mathf.Lerp(shakeAmount, amount, time/2);
            time += Time.deltaTime;

            yield return null;
        }

        yield return null;
    }
}