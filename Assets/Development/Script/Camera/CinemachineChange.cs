using System;
using Cinemachine;
using UnityEngine;

public class CinemachineChange : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera CmCamera;
    [SerializeField] private float changeSpeed = 5;
    [SerializeField] private float orthoSize = 16;
    [SerializeField] private float followOffsetX = 7;
    
    private float tragetOrthoSize;
    private float targetFollowOffsetX;
    private CinemachineTransposer transposer;
    public bool activatedFlag = false;

    private void Start()
    {
        transposer = CmCamera.GetCinemachineComponent<CinemachineTransposer>();
        tragetOrthoSize = orthoSize;
        targetFollowOffsetX = followOffsetX;
    }

    private void Update()
    {
        if (!activatedFlag) return;
        
        if (CmCamera.m_Lens.OrthographicSize != tragetOrthoSize ||
            transposer.m_FollowOffset.x != targetFollowOffsetX)
        {
            updateCameraValues();
        }

        else
        {
            activatedFlag = false;
        }
    }

    private void updateCameraValues()
    {
        if (CmCamera.m_Lens.OrthographicSize < tragetOrthoSize)
        {
            CmCamera.m_Lens.OrthographicSize += changeSpeed * Time.deltaTime;
            if (CmCamera.m_Lens.OrthographicSize > tragetOrthoSize)
                CmCamera.m_Lens.OrthographicSize = tragetOrthoSize;
        }
        else
        {
            CmCamera.m_Lens.OrthographicSize -= changeSpeed * Time.deltaTime;
            if (CmCamera.m_Lens.OrthographicSize < tragetOrthoSize)
                CmCamera.m_Lens.OrthographicSize = tragetOrthoSize;
        }

        if (transposer.m_FollowOffset.x < targetFollowOffsetX)
        {
            transposer.m_FollowOffset.x += changeSpeed * Time.deltaTime;
            if (transposer.m_FollowOffset.x > targetFollowOffsetX)
                transposer.m_FollowOffset.x = targetFollowOffsetX;
        }
        else
        {
            transposer.m_FollowOffset.x -= changeSpeed * Time.deltaTime;
            if (transposer.m_FollowOffset.x < targetFollowOffsetX)
                transposer.m_FollowOffset.x = targetFollowOffsetX;
        }
    }
    
    
}
