using System.Collections;
using UnityEngine;

public class Sprint : Feature
{
    private enum MovementState
    {
        WALK,
        CROUCH,
        RUN,
    }

    public override int Order() => 135;

    private MovementState state;
    private Coroutine transition;
    private bool onTranstion;
    private float currentMaxSpeed;
    private bool currentSpeedSet;

    public override void UpdateFeature()
    {
        if(!currentSpeedSet)
        {
            currentMaxSpeed = invoker.maxSpeed;
            currentSpeedSet = true;
        }

        if(!onTranstion)
            currentMaxSpeed = invoker.maxSpeed;

        float destinationMaxSpeed;
        if(invoker.IsCrouching)
        {
            state = MovementState.CROUCH;
            destinationMaxSpeed = settings.crouchMaxSpeed;
        } 
        else if (invoker.IsRunning)
        {
            state = MovementState.RUN;
            destinationMaxSpeed = settings.runMaxSpeed;
        }
        else
        {
            state = MovementState.WALK;
            destinationMaxSpeed = settings.walkMaxSpeed;
        }

        float diffSpeed = Mathf.Abs(currentMaxSpeed - destinationMaxSpeed);
        if(diffSpeed != destinationMaxSpeed)
        {
            if(diffSpeed > settings.maxSpeedDiffThreshold)
            {
                if(onTranstion)
                {
                    StopCoroutine(transition);
                    transition = null;
                    onTranstion = false;
                }
                transition = StartCoroutine(UpdateMovementState(destinationMaxSpeed));
            }
            else invoker.maxSpeed = destinationMaxSpeed;
        }
    }

    private IEnumerator UpdateMovementState(float destinationMaxSpeed)
    {
        onTranstion = true;
        float startMaxSpeed = invoker.maxSpeed;
        float elapsedTime = 0f;
        while(elapsedTime < settings.maxSpeedTransitionDuration)
        {
            elapsedTime += Time.deltaTime;
            invoker.maxSpeed = Mathf.Lerp(startMaxSpeed, destinationMaxSpeed, elapsedTime / settings.maxSpeedTransitionDuration);
            yield return null;
        }
        invoker.maxSpeed = destinationMaxSpeed;
        onTranstion = false;
    }
}