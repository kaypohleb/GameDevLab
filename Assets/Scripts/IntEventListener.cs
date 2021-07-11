using UnityEngine;
using UnityEngine.Events;
public class IntEventListener : MonoBehaviour
{
    public IntEvent Event;

    public IntUnityEvent Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(int k)
    {
        Response.Invoke(k);
    }
}

[System.Serializable]
public class IntUnityEvent : UnityEvent<int> {} // Leave it empty