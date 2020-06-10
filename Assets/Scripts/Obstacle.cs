using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Obstacle : MonoBehaviour
{
    [SerializeField] Vector3 movingVector = new Vector3(5f, 5f, 5f);
    [SerializeField] bool isRotating = false;

    //todo remove from inspector l8er
    float moveFactor;//0 not moved,1 fully moved
    [SerializeField] float period = 2f;
    [SerializeField] float rotationSpeed = 0.3f;

    Vector3 startingPosition;
    void Start()
    {
        
            startingPosition = transform.position;
    }

    void Update()
    {
       
        ApplyRotation();
        MovingCicle();
    }

    private void ApplyRotation()
    {
        if (isRotating)
        {
            transform.Rotate(0, 0, rotationSpeed);
        }
    }

    private void MovingCicle()
    {
        if (period <= Mathf.Epsilon) return;// protect against 0 value
        float cycles = Time.time / period;// grows continually from 0
        const float tau = Mathf.PI * 2;//about 6.28
        float rawSinWave = Mathf.Sin(cycles * tau);//goes from -1 to +1

        moveFactor = rawSinWave / 2f + 0.5f;// goes from 0 to 1

        Vector3 offset = movingVector * moveFactor;
        transform.position = startingPosition + offset;
    }
}
