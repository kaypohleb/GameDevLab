using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioClip[] audioClips;
    AudioSource[] audioSource;
    bool loaded;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponents<AudioSource>();
        loaded = false;
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
        if(audioSource[1].clip == audioClips[10] && audioSource[1].isPlaying){
            audioSource[1].Stop();
        }
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

    public void playThemeSound(bool mode){
        if(mode){
            if(!loaded){
                audioSource[1].loop = true;
                audioSource[1].clip = audioClips[10];
                loaded = true;
            }
            if(!audioSource[1].isPlaying && audioSource[1].clip == audioClips[10]){
                audioSource[1].Play();
            }
        }else{
            audioSource[1].Pause();
        }
    }

    public void PlayClearSound(){
        if(audioSource[1].clip == audioClips[10] && audioSource[1].isPlaying){
            audioSource[1].Stop();
        }
        if(!audioSource[1].isPlaying){
            audioSource[1].PlayOneShot(audioClips[11]);
        }
       
    }
}
