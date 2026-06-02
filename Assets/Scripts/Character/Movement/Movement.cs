using UnityEngine;

public class Movement : Feature
{
    public override int Order() => 150;

    public override void UpdateFeature()
    {
        SetGravity();
    }

    public override void FixedUpdateFeature()
    {
        Move();
        LimitSpeed();
        Friction();
    }

    private void Move()
    {
        Vector3 direction = invoker.Right * reader.Move.x + invoker.Forward * reader.Move.y;
        direction.Normalize();
        if(invoker.OnSlope && !invoker.ExitingSlope)
        {
            direction = Vector3.ProjectOnPlane(direction, invoker.SlopeNormal);
            invoker.AddForce(-invoker.SlopeNormal * settings.slopeSnapForce);
        }
        invoker.AddForce(direction * settings.acceleration);
    }

    private void LimitSpeed()
    {
        Vector3 velocity = invoker.Velocity;
        if(!invoker.OnSlope || invoker.ExitingSlope)
        {
            velocity.y = 0f;
        }

        if (velocity.sqrMagnitude > invoker.maxSpeed * invoker.maxSpeed)
        {
            velocity = velocity.normalized * invoker.maxSpeed;
        }

        if (!invoker.OnSlope || invoker.ExitingSlope)
        {
            invoker.Velocity = new Vector3(velocity.x, invoker.Velocity.y, velocity.z);
            return;
        }
        invoker.Velocity = velocity;
    }

    private void Friction()
    {
        float friction = invoker.OnGround ? settings.groundFriction : settings.airFriction;
        Vector3 moveIntention = invoker.Right * reader.Move.x + invoker.Forward * reader.Move.y;
        bool isTurning = Vector3.Dot(moveIntention, invoker.Velocity) < 0;

        Vector3 flatVel = invoker.Velocity;
        flatVel.y = 0f;
        if(moveIntention == Vector3.zero || isTurning)
        {
            invoker.AddImpulse(-flatVel * friction * Time.fixedDeltaTime);
        }
    }

    private void SetGravity()
    {
        invoker.UseGravity = !invoker.OnSlope || invoker.ExitingSlope;
    }
}