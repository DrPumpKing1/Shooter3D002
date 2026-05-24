using System;
using System.Collections.Generic;
using System.Linq;

public class DependencyInjector
{
    private Dictionary<Type, Feature> featureMap = new();

    public DependencyInjector(List<Feature> features)
    {
        foreach(Feature feature in features)
        {
            featureMap.Add(feature.GetType(), feature);
        }
    }

    public T Get<T>() where T : Feature
    {
        if(!featureMap.TryGetValue(typeof(T), out Feature feature))
        {
            return null;
        }

        if(feature is not T specificFeature)
        {
            return null;
        }

        return specificFeature;
    }
}