using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public Settings settings;
    [SerializeField] private InputReader reader;
    public InputReader Reader => reader;

    public DependencyInjector injector {get; private set;}
    public Invoker invoker {get; private set;}

    private List<Feature> features;

    void Awake()
    {
        features = GetComponents<Feature>().OrderByDescending(f => f.Order()).ToList();
        injector = new(features);
        invoker = new(this);
        reader.OnInput += OnInput;

        foreach(Feature feature in features)
        {
            feature.Initialize(this);
        }
    }

    void Update()
    {
        TimeData timeData = new TimeData
        {
            time = Time.time,
            deltaTime = Time.deltaTime,
        };
        foreach(Feature feature in features)
        {
            feature.UpdateFeature();
        }
    }

    void FixedUpdate()
    {
        TimeData timeData = new TimeData
        {
            time = Time.time,
            deltaTime = Time.fixedDeltaTime,
        };
        foreach(Feature feature in features)
        {
            feature.FixedUpdateFeature();
        }
    }

    void OnInput(InputReader.InputData inputData)
    {
        foreach(Feature feature in features)
        {
            feature.OnInput(inputData);
        }
    }
}