using System;
using Cinemachine;
using UnityEngine;

public class CinemachineChange : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera CmCamera;
    [SerializeField] private float changeSpeed = 5;
    [SerializeField] private float orthoSize = 16;
    [SerializeField] private float followOffsetX = 7;
    [SerializeField] private float followOffsetY = 0;
    
    private float tragetOrthoSize;
    private float targetFollowOffsetX; 
    private float targetFollowOffsetY;
    private CinemachineTransposer transposer;
    public bool activatedFlag;

    private void OnEnable()
    {
        GameManager.CheckPointReset += ResetCamera;
    }

    private void OnDestroy()
    {
        GameManager.CheckPointReset -= ResetCamera;
    }

    private void Start()
    {
        transposer = CmCamera.GetCinemachineComponent<CinemachineTransposer>();
        tragetOrthoSize = orthoSize;
        targetFollowOffsetX = followOffsetX;
        targetFollowOffsetY = followOffsetY != 0 ? followOffsetY : transposer.m_FollowOffset.y;
    }

    private void Update()
    {
        if (!activatedFlag) return;
        
        if (CmCamera.m_Lens.OrthographicSize != tragetOrthoSize ||
            transposer.m_FollowOffset.x != targetFollowOffsetX ||
            transposer.m_FollowOffset.y != targetFollowOffsetY)
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
        
        if (transposer.m_FollowOffset.y < targetFollowOffsetY)
        {
            transposer.m_FollowOffset.y += changeSpeed * Time.deltaTime;
            if (transposer.m_FollowOffset.y > targetFollowOffsetY)
                transposer.m_FollowOffset.y = targetFollowOffsetY;
        }
        else
        {
            transposer.m_FollowOffset.y -= changeSpeed * Time.deltaTime;
            if (transposer.m_FollowOffset.y < targetFollowOffsetY)
                transposer.m_FollowOffset.y = targetFollowOffsetY;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
            activatedFlag = true;
    }

    private void ResetCamera()
    {
        CmCamera.m_Lens.OrthographicSize = 17;
        transposer.m_FollowOffset.x = 7;
        transposer.m_FollowOffset.y = 2.25f;

    }
}
