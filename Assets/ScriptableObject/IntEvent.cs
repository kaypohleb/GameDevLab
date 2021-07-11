using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "IntEvent", menuName = "Generic/IntEvent", order = 2)]
public class IntEvent : ScriptableObject
{
    private readonly List<IntEventListener> eventListeners = 
        new List<IntEventListener>();

    public void Raise(int k)
    {
        for(int i = eventListeners.Count -1; i >= 0; i--)
            eventListeners[i].OnEventRaised(k);
    }

    public void RegisterListener(IntEventListener listener)
    {
        if (!eventListeners.Contains(listener))
            eventListeners.Add(listener);
    }

    public void UnregisterListener(IntEventListener listener)
    {
        if (eventListeners.Contains(listener))
            eventListeners.Remove(listener);
    }
}
