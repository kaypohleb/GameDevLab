using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOverManager : MonoBehaviour
{
    // Start is called before the first frame update
    DDOL gameState;
    public void PlayGame(){
        gameState = GameObject.FindGameObjectWithTag("DDOL").GetComponent<DDOL>();
        gameState.Reset();
        SceneManager.LoadScene(0);
    }
}
