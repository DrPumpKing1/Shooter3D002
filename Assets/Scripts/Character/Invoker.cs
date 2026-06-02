using UnityEngine;

public class Invoker
{
    [SerializeField] private Camera cam;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private CapsuleCollider coll;
    [SerializeField] private Transform player;
    private Settings settings;
    private bool onGround;

    public Invoker(Controller controller)
    {
        cam = Camera.main;
        player = controller.transform;
        rb = controller.GetComponent<Rigidbody>();
        coll = controller.GetComponent<CapsuleCollider>();

        settings = controller.settings;
        jumpCooldown = new(settings.jumpCooldown);
        crouchTranstion = new(settings.crouchTransitionTime);

        maxSpeed = settings.walkMaxSpeed;
    }

    public Vector3 Position
    {
        get => player.position;
        set => player.position = value;
    }

    public Vector3 CameraPosition
    {
        get => cam.transform.position;
        set => cam.transform.position = value;
    }

    public Quaternion CameraRotation
    {
        get => cam.transform.rotation;
        set => cam.transform.rotation = value;
    }

    public float Yaw
    {
        get => cam.transform.rotation.eulerAngles.y;
        set => cam.transform.rotation = Quaternion.Euler(cam.transform.rotation.eulerAngles.x, value, cam.transform.rotation.eulerAngles.z);
    }

    public float Pitch
    {
        get
        {
            float angle = cam.transform.rotation.eulerAngles.x;
            if(angle > 180f) angle -= 360f;
            return angle;
        }
        set => cam.transform.rotation = Quaternion.Euler(value, cam.transform.rotation.eulerAngles.y, cam.transform.rotation.eulerAngles.z);
    }

    public Quaternion Orientation
    {
        get => Quaternion.Euler(0f, CameraRotation.eulerAngles.y, 0f);
    }

    public Vector3 Right
    {
        get => Orientation * Vector3.right;
    }

    public Vector3 Forward
    {
        get => Orientation * Vector3.forward;
    }

    public Vector3 Size
    {
        get => coll.bounds.size;
    }

    public float Height
    {
        get => player.localScale.y;
        set => player.localScale = new Vector3(player.localScale.x, value, player.localScale.z);
    }

    public Vector3 Velocity
    {
        get => rb.linearVelocity;
        set => rb.linearVelocity = value;
    }

    public bool UseGravity
    {
        get => rb.useGravity;
        set => rb.useGravity = value;
    }

    public void AddForce(Vector3 force) => rb.AddForce(force, ForceMode.Acceleration);
    public void AddImpulse(Vector3 impulse) => rb.AddForce(impulse, ForceMode.VelocityChange);

    public float Steer;
    public bool OnGround
    {
        get => onGround;
        set
        {
            onGround = value;
            if(onGround) LastGroundTime = Time.time;
        }
    }
    public float LastGroundTime {get; private set;}
    public bool OnSlope;
    public bool HeadBlocked;
    public Vector3 SlopeNormal;
    public bool ExitingSlope;
    public Timer jumpCooldown;
    public bool IsCrouching;
    public Timer crouchTranstion;
    public float maxSpeed;
    public bool IsRunning;
}