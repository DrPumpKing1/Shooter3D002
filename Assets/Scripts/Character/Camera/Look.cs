using UnityEngine;

public class Look : Feature
{
    public override int Order() => 100;

    public override void UpdateFeature()
    {
        Cursor.visible = settings.cursorVisible;
        Cursor.lockState = settings.cursorLockMode;

        Vector2 input = reader.Look;
        Vector2 delta = new Vector3(-input.y, input.x, 0) * Time.deltaTime * settings.sensibility;
        float pitch = invoker.Pitch + delta.x;
        float yaw = invoker.Yaw + delta.y;
        pitch = Mathf.Clamp(pitch, settings.minPitch, settings.maxPitch);
        Quaternion targetRot = Quaternion.Euler(pitch, yaw, 0f);
        invoker.CameraRotation = Quaternion.Slerp(invoker.CameraRotation, targetRot, settings.smoothRotation);

        Vector3 targetPos = invoker.Position + settings.offset;
        invoker.CameraPosition = Vector3.Lerp(invoker.CameraPosition, targetPos, settings.smoothPosition);
    }
}