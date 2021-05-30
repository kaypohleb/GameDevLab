using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioClip[] audioClips;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playJumpSound(bool grow){
        if(grow){
            audioSource.PlayOneShot(audioClips[1]);
        }else{
            audioSource.PlayOneShot(audioClips[0]);
        }
    }

    public void playDieSound(){
        audioSource.PlayOneShot(audioClips[5]);
    }
    
    public void playStompSound(){
        audioSource.PlayOneShot(audioClips[4]);
    }
    public void playPowerSound(bool up){
        if(audioSource.isPlaying){
            audioSource.Stop();
        }
        if(up){
            audioSource.PlayOneShot(audioClips[3]);
        }else{
            audioSource.PlayOneShot(audioClips[2]);
        }
        
    }
}
