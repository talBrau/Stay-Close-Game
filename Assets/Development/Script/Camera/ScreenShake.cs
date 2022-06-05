using UnityEngine;
using Cinemachine;


public class ScreenShake : MonoBehaviour
{ 
    [SerializeField] private CinemachineVirtualCamera VirtualCamera;
    [SerializeField] private bool OnTime;
    public float ShakeDuration = 0.3f;          // Time the Camera Shake effect will last
    public float ShakeAmplitude = 1.2f;         // Cinemachine Noise Profile Parameter
    public float ShakeFrequency = 2.0f;         // Cinemachine Noise Profile Parameter
    private float ShakeElapsedTime;
    public bool isActive;
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;

    private void OnEnable()
    {
        GameManager.CheckPointReset += ResetCamera;
    }

    private void OnDestroy()
    {
        GameManager.CheckPointReset -= ResetCamera;
    }

    void Start()
    {
        if (VirtualCamera != null)
            virtualCameraNoise = VirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    void Update()
    {
        if (VirtualCamera == null || virtualCameraNoise == null) return;
        switch (OnTime)
        {
            case true when ShakeElapsedTime > 0:
                virtualCameraNoise.m_AmplitudeGain = ShakeAmplitude;
                virtualCameraNoise.m_FrequencyGain = ShakeFrequency;
                
                ShakeElapsedTime -= Time.deltaTime;
                break;
            
            case true:
                virtualCameraNoise.m_AmplitudeGain = 0f;
                ShakeElapsedTime = 0f;
                break;
            
            case false when isActive:
                virtualCameraNoise.m_AmplitudeGain = ShakeAmplitude;
                virtualCameraNoise.m_FrequencyGain = ShakeFrequency;
                break;
        }
    }

    public void ActivateShake()
    {
        if (OnTime)
            ShakeElapsedTime = ShakeDuration;
        else
            isActive = true;
    }

    public void DisableShake()
    {
        if (OnTime) return;
        isActive = false;
        virtualCameraNoise.m_AmplitudeGain = 0f;
        ShakeElapsedTime = 0f;
    }
    
    
    public void ResetCamera()
    {
        virtualCameraNoise.m_AmplitudeGain = 0f;
        virtualCameraNoise.m_FrequencyGain = 0f;
        ShakeElapsedTime = 0f;
        isActive = false;
    }
}

