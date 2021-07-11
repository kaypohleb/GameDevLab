using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioClip[] audioClips;
    [SerializeField] AudioClip[] playerAudioClips;
     [SerializeField] AudioClip[] breakBlockClips; 
    AudioSource[] audioSource;
    public BoolReference grow;
    bool loaded;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponents<AudioSource>();
        loaded = false;
    }

    
    public void playThemeSound(bool mode){
        if(mode){
            if(!loaded){
                audioSource[0].loop = true;
                audioSource[0].clip = audioClips[0];
                loaded = true;
            }
            if(!audioSource[0].isPlaying && audioSource[0].clip == audioClips[0]){
                audioSource[0].Play();
            }
        }else{
            audioSource[0].Pause();
        }
    }

    public void PlayClearSound(){
        playThemeSound(false);
        if(!audioSource[1].isPlaying){
            audioSource[1].PlayOneShot(playerAudioClips[1]);
        }
       
    }
     public void playJumpSound(){
        if(grow.Value){
            audioSource[2].PlayOneShot(playerAudioClips[1]);
        }else{
            audioSource[2].PlayOneShot(playerAudioClips[0]);
        }
    }
    public void playPipeSound(){
        audioSource[2].PlayOneShot(playerAudioClips[2]);
    }

    public void playDieSound(){
        audioSource[2].PlayOneShot(playerAudioClips[5]);
    }
    
    public void playStompSound(){
        audioSource[2].PlayOneShot(playerAudioClips[4]);
    }
    public void playPowerSound(bool up){
        if(audioSource[2].isPlaying){
            audioSource[2].Stop();
        }
        if(up){
            audioSource[2].PlayOneShot(playerAudioClips[3]);
        }else{
            audioSource[2].PlayOneShot(playerAudioClips[2]);
        }
    }
    public void playPoleSound(){
        audioSource[2].PlayOneShot(playerAudioClips[6]);
    }
    
    public void playBumpBrickSound(){
        audioSource[3].PlayOneShot(breakBlockClips[0]);
    }
    public void playBreakBrickSound(){
        audioSource[3].PlayOneShot(breakBlockClips[1]);
    }
    public void playItemBoxSound(){
        audioSource[3].PlayOneShot(breakBlockClips[2]);
    }
}
