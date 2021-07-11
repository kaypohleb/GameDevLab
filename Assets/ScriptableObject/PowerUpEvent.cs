using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PowerUpEvent", menuName = "Generic/PowerUpEvent", order = 3)]
public class PowerUpEvent : ScriptableObject
{
    private readonly List<PowerUpEventListener> eventListeners = 
        new List<PowerUpEventListener>();

    public void Raise(PowerUp p)
    {
        for(int i = eventListeners.Count -1; i >= 0; i--)
            eventListeners[i].OnEventRaised(p);
    }

    public void RegisterListener(PowerUpEventListener listener)
    {
        if (!eventListeners.Contains(listener))
            eventListeners.Add(listener);
    }

    public void UnregisterListener(PowerUpEventListener listener)
    {
        if (eventListeners.Contains(listener))
            eventListeners.Remove(listener);
    }
}
