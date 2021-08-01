/***
 * Cinemachine camera shake control with intencity
 * and time that camera will shake.
 ***/
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private float shakeTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void ShakeCamera(float intensity = 3f, float time = 0.3f)
    {
        if (Instance != null)
        {
            CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
                cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
            shakeTimer = time;
        }
        else
        {
            Debug.LogError("CameraShake instance in null.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f)
            {
                // time's over
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
                            cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
            }
        }
    }
}
