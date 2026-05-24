using UnityEngine;

public struct TimeData
{
    public float deltaTime;
    public float time;
}

public abstract class Feature : MonoBehaviour
{
    protected Settings settings;
    protected DependencyInjector injector;
    protected Invoker invoker;
    protected InputReader reader;

    public abstract int Order();

    public virtual void Initialize(Controller controller)
    {
        settings = controller.settings;
        injector = controller.injector;
        invoker = controller.invoker;
        reader = controller.Reader;
    }
    public virtual void UpdateFeature() { }
    public virtual void FixedUpdateFeature() { }
    public virtual void OnInput(InputReader.InputData input) { }
}