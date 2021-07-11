using UnityEngine;
using UnityEngine.Events;
public class KeyCodeEventListener : MonoBehaviour
{
    public KeyCodeEvent Event;

    public KeyCodeUnityEvent Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(KeyCode k)
    {
        Response.Invoke(k);
    }
}

[System.Serializable]
public class KeyCodeUnityEvent : UnityEvent<KeyCode> {} // Leave it empty