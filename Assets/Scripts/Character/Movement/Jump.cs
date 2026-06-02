using UnityEngine;

public class Jump : Feature
{
    public override int Order() => 100;

    public override void UpdateFeature()
    {
        invoker.jumpCooldown.Update(Time.deltaTime);

        if(!invoker.jumpCooldown.IsRunning && invoker.OnGround)
        {
            invoker.ExitingSlope = true;
        }
    }

    public override void FixedUpdateFeature()
    {
        GravityMultiplier();
    }

    public override void OnInput(InputReader.InputData input)
    {
        if(input.action == InputReader.Actions.JUMP && input.interaction == InputReader.Interactions.PRESS)
        {
            JumpAction();
        }
    }

    private void JumpAction()
    {
        if(invoker.jumpCooldown.IsRunning || Time.time - invoker.LastGroundTime > settings.coyoteTime) return;

        float extraJumpForce = new Vector3(invoker.Velocity.x, 0f, invoker.Velocity.z).magnitude * settings.velocityExtraJumpForce;
        float jumpForce = settings.jumpForce + extraJumpForce;
        if(invoker.IsCrouching) jumpForce *= settings.crouchSpeedMultiplier;

        invoker.AddImpulse(Vector3.up * jumpForce);
        invoker.ExitingSlope = true;
        invoker.jumpCooldown.Start();
    }

    private void GravityMultiplier()
    {
        float multiplier = 0f;
        if(!invoker.OnGround && invoker.Velocity.y <= 0f)
        {
            multiplier = settings.fallMultiplier;
        }
        if(multiplier == 0f) return;
        invoker.AddImpulse(Vector3.up * Physics.gravity.y * (multiplier - 1) * Time.fixedDeltaTime);
    }
}