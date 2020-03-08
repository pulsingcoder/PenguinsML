using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public float fishSpeed;
    private float randomisedSpeed = 0f;
    private float nextActionTime = 2f;
    private Vector3 targetPosition;


    private void FixedUpdate()
    {
        if (fishSpeed > 0f)
        {
            Swim();
        }
    }

    private void Swim()
    {
        if (Time.fixedTime >=  nextActionTime)
        {
            // randomised the speed 
            randomisedSpeed = fishSpeed * UnityEngine.Random.Range(.5f, 1.5f);
            // pic the target position
            targetPosition = PenguinArea.ChooseRandomPosition(transform.parent.position, 260f, 100f, 13f, 2f);
            //Rotate the target 
            transform.rotation = Quaternion.LookRotation(targetPosition - transform.position, Vector3.up);
            // calculate time to get there
            float timeToGetThere = Vector3.Distance(transform.position, targetPosition) / randomisedSpeed;
            nextActionTime = Time.fixedTime + timeToGetThere;
        }
        else
        {
            // Make sure that the fish does not past the target 
            Vector3 moveVector = randomisedSpeed * Time.fixedDeltaTime * transform.forward;
            if (moveVector.magnitude <= Vector3.Distance(transform.position,targetPosition))
            {
                transform.position += moveVector; 
            }
            else
            {
                transform.position = targetPosition;
                nextActionTime = Time.fixedTime;
            }
        }
    }
}
