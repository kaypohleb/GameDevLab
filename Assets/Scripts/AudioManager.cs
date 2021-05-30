using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioClip[] audioClips;
    AudioSource[] audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponents<AudioSource>();
        playThemeSound();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playJumpSound(bool grow){
        if(grow){
            audioSource[0].PlayOneShot(audioClips[1]);
        }else{
            audioSource[0].PlayOneShot(audioClips[0]);
        }
    }
    public void playPipeSound(){
        audioSource[0].PlayOneShot(audioClips[2]);
    }

    public void playDieSound(){
        audioSource[0].PlayOneShot(audioClips[5]);
    }
    
    public void playStompSound(){
        audioSource[0].PlayOneShot(audioClips[4]);
    }
    public void playPowerSound(bool up){
        if(audioSource[0].isPlaying){
            audioSource[0].Stop();
        }
        if(up){
            audioSource[0].PlayOneShot(audioClips[3]);
        }else{
            audioSource[0].PlayOneShot(audioClips[2]);
        }
    }
    public void playBumpSound(){
        audioSource[0].PlayOneShot(audioClips[8]);
    }
    public void playPoleSound(){
        audioSource[0].PlayOneShot(audioClips[6]);
    }

    public void playThemeSound(){
        if(audioSource[1].isPlaying){
            audioSource[1].Stop();
        }
        audioSource[1].loop = true;
        audioSource[1].clip = audioClips[10];
        audioSource[1].Play();
    }
}
