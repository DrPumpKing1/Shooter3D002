using System;

public class Timer
{
    private float elapsedTime;
    private float duration;
    private bool isRunning;
    private bool loop;

    public float ElapsedTime => elapsedTime;
    public float RemainingTime => duration - elapsedTime;
    public float Duration => duration;
    public bool IsRunning => isRunning;
    public bool Loop => loop;

    public event Action OnComplete;
    public event Action OnStart;

    public Timer(float duration, bool autostart = false, bool loop = false)
    {
        this.duration = duration;
        this.loop = loop;

        elapsedTime = 0f;
        isRunning = false;
        if(autostart) Start();
    }

    public void Start()
    {
        isRunning = true;
        OnStart?.Invoke();
    }

    public void Update(float deltaTime)
    {
        if(!isRunning) return;

        elapsedTime += deltaTime;
        if(elapsedTime > duration)
        {
            elapsedTime -= duration;
            if(!loop)
            {
                isRunning = false;
            }
            OnComplete?.Invoke();
        }
    }
}