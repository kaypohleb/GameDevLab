using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "KeyCodeEvent", menuName = "Generic/KeyCodeEvent", order = 2)]
public class KeyCodeEvent : ScriptableObject
{
    private readonly List<KeyCodeEventListener> eventListeners = 
        new List<KeyCodeEventListener>();

    public void Raise(KeyCode k)
    {
        for(int i = eventListeners.Count -1; i >= 0; i--)
            eventListeners[i].OnEventRaised(k);
    }

    public void RegisterListener(KeyCodeEventListener listener)
    {
        if (!eventListeners.Contains(listener))
            eventListeners.Add(listener);
    }

    public void UnregisterListener(KeyCodeEventListener listener)
    {
        if (eventListeners.Contains(listener))
            eventListeners.Remove(listener);
    }
}
