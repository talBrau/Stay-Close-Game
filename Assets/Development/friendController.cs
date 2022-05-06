using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class friendController : MonoBehaviour
{
    public List<Spot> spots;
    [SerializeField] private float smoothTime = 0.3f;
    [SerializeField] private float maxSpeed = 5;
    [SerializeField] private float borderOffset;
    [SerializeField] private GameObject border;
    private Vector2 _velocity = Vector2.zero;
    private float xOff=0;
    private float yOff=0;

    private bool hasTarget;
    public bool HasTarget
    {
        get => hasTarget;
        set => hasTarget = value;
    }


    void Start()
    {
        spots = new List<Spot>();
    }

    void Update()
    {
        if (!hasTarget)
            MoveAroundPlayer();
    }

    public void EnteredSpot(Spot spot)
    {
        spots.Add(spot);
    }

    public void ExitSpot(Spot spot)
    {
        spots.Remove(spot);
    }

    private void MoveAroundPlayer()
    {
        // xOff += Random.value;
        // yOff += Random.value;
        // var randomCirclePoint = new Vector3(Mathf.PerlinNoise(xOff,yOff),Mathf.PerlinNoise(xOff,yOff));

        var randomCirclePoint = Random.insideUnitSphere;

        var targetPos = border.transform.TransformPoint(randomCirclePoint * borderOffset);
        transform.position = Vector2.SmoothDamp(transform.position, targetPos,
            ref _velocity, smoothTime, maxSpeed);
    }
}