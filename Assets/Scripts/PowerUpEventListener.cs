using UnityEngine;
using UnityEngine.Events;
public class PowerUpEventListener : MonoBehaviour
{
    public PowerUpEvent Event;

    public PowerUpUnityEvent Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(PowerUp p)
    {
        Response.Invoke(p);
    }
}

[System.Serializable]
public class PowerUpUnityEvent : UnityEvent<PowerUp> {} // Leave it empty