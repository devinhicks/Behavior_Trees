﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrive : Kinematic
{
    public Kinematic character;
    public GameObject target;

    public float maxAcceleration = 10f;
    float maxSpeed = 2f;

    float targetRadius = .75f; // the radius for arriving at the target                            
    float slowRadius = 1.5f; // the radius for beginning to slow down                 
    float timeToTarget = 0.1f; // the time over which to achieve target speed

    protected virtual Vector3 getTargetPosition()
    {
        return target.transform.position;
    }

    public SteeringOutput getSteering()
    {
        SteeringOutput result = new SteeringOutput();

        Vector3 direction, targetVelocity;
        float distance, targetSpeed;

        // get the direction to the target
        direction = getTargetPosition() - character.transform.position;
        distance = direction.magnitude;

        // if (distance < targetRadius)
        //     return null

        // if outside slowRadius, move at maxSpeed
        if (distance > slowRadius)
        {
            targetSpeed = maxSpeed;
        }
        // otherwise calculate a scaled speed
        else
        {
            targetSpeed = maxSpeed * (distance - targetRadius) / targetRadius;
        }

        //the target velocity combines speed and direction
        targetVelocity = direction;
        targetVelocity.Normalize();
        targetVelocity *= targetSpeed;

        // acceleration tries to get to the target velocity
        result.linear = targetVelocity - character.linearVelocity;
        result.linear /= timeToTarget;

        // check if the acceleration is too fast
        if (result.linear.magnitude > maxAcceleration)
        {
            result.linear.Normalize();
            result.linear *= maxAcceleration;
        }

        result.angular = 0;
        return result;
    }
}