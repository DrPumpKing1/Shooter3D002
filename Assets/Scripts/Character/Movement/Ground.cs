using UnityEngine;

public class Ground : Feature
{
    private const float EXTRA_DISTANCE_GROUND = 0.05f;
    private const float EXTRA_DISTANCE_SLOPE = 0.15f;

    public override int Order() => 200;

    private RaycastHit slopeHit;

    public override void UpdateFeature()
    {
        CheckGround();
        CheckSlope();
        CheckHeadBlocked();
    }

    private void CheckGround()
    {
        float distance = invoker.Size.y / 2f;
        Vector3 footSize = new (invoker.Size.x, EXTRA_DISTANCE_GROUND, invoker.Size.z);
        invoker.OnGround = Physics.BoxCast(invoker.Position, footSize, Vector3.down, Quaternion.identity, distance, settings.ground);
    }

    private void CheckSlope()
    {
        float distance = invoker.Size.y / 2f + EXTRA_DISTANCE_SLOPE;
        if(Physics.Raycast(invoker.Position, Vector3.down, out slopeHit, distance, settings.ground))
        {
            float angle = Vector3.Angle(slopeHit.normal, Vector3.up);
            invoker.OnSlope = angle <= settings.maxSlopeAngle && angle != 0f;
            if(invoker.OnSlope) invoker.SlopeNormal = slopeHit.normal;
        }
        else
        {
            invoker.OnSlope = false;
        }
    }

    private void CheckHeadBlocked()
    {
        float distance = invoker.Size.y * 1.5f;
        Vector3 halfSize = invoker.Size / 2f;
        halfSize.y = EXTRA_DISTANCE_GROUND;
        invoker.HeadBlocked = Physics.BoxCast(invoker.Position, halfSize, Vector3.up, Quaternion.identity, distance, settings.ground);
    }
}