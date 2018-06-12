using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class cameraShake : MonoBehaviour
{
    public PostProcessVolume pV;

    // Transform of the camera to shake. Grabs the gameObject's transform
    // if null.
    public Transform camTransform;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;

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
        Bloom b = null;
        pV.profile.TryGetSettings(out b);

        camTransform.localPosition = originalPos + Random.insideUnitSphere * b.intensity.value/50;
    }
}