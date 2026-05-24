using UnityEngine;

[CreateAssetMenu]
public class Settings : ScriptableObject
{
    [Header("Camera")]
    public float sensibility;
    public float minPitch;
    public float maxPitch;
    public float smoothRotation;
    public float smoothPosition;
    public Vector3 offset;

    [Header("Cursor")]
    public bool cursorVisible;
    public CursorLockMode cursorLockMode;

    [Header("Ground Check")]
    public LayerMask ground;
    public float maxSlopeAngle;

    [Header("Movement")]
    public float acceleration;
    public float maxSpeed;
    public float groundFriction;
    public float airFriction;
    public float slopeSnapForce;

    [Header("Jump")]
    public float jumpForce;
    public float jumpCooldown;
    public float fallMultiplier;
}