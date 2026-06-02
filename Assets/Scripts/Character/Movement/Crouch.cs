using UnityEngine;

public class Crouch : Feature
{
    private float initalHeight;
    private float crouchHeight;

    //Transition
    private bool inTransition;
    private float startHeight;
    private float targetHeight;

    public override int Order() => 125;

    public override void Initialize(Controller controller)
    {
        base.Initialize(controller);
        initalHeight = invoker.Height;
        crouchHeight = invoker.Height * settings.crouchHeightMultiplier;
    }

    public override void UpdateFeature()
    {
        if(!inTransition) return;

        invoker.crouchTranstion.Update(Time.deltaTime);

        if(!invoker.crouchTranstion.IsRunning)
        {
            inTransition = false;
            invoker.Height = targetHeight;
            return;
        }

        float parameter = invoker.crouchTranstion.ElapsedTime / settings.crouchTransitionTime;
        invoker.Height = Mathf.Lerp(startHeight, targetHeight, parameter);
    }

    public override void OnInput(InputReader.InputData input)
    {
        if(input.action != InputReader.Actions.CROUCH || input.interaction != InputReader.Interactions.PRESS) return;

        if(inTransition) return;
        if (!invoker.IsCrouching) CrouchAction();
        else if (!invoker.HeadBlocked) CrouchAction();
    }

    private void CrouchAction()
    {
        inTransition = true;
        startHeight = invoker.Height;
        invoker.crouchTranstion.Start();

        if (invoker.IsCrouching)
        {
            targetHeight = initalHeight;
            invoker.IsCrouching = false;
        } else
        {
            targetHeight = crouchHeight;
            invoker.IsCrouching = true;
        }
    }
}