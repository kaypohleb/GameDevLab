using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillAfter : MonoBehaviour
{
    public GameConstants gameConstants;

    private void Start() {
        Destroy(gameObject, gameConstants.explosionTTL);    
    }
}
