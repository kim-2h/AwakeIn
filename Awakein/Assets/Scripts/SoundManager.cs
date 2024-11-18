using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
        public AudioSource[] BGMs;
    public AudioSource[] SFXs;
    void Awake(){
    SoundManager.Instance = this; 
     }
    public enum EScenes
    {
        Title, Room1, Room2, Room3, Ending
    }
    public EScenes Scenes = EScenes.Title;

    void Start()
    {
        // GameObject[] AAudios = GameObject.FindGameObjectsWithTag("Audio");
        // if (AAudios.Length >=2) Destroy(this.gameObject);
        // else DontDestroyOnLoad(this.gameObject);
 
        BGMs[0].Play();
    }

    public void StopBGM()
    {
        for (int i = 0; i < BGMs.Length; i++)
        {
            BGMs[i].Stop();
        }
    }

    public void PlayBGM(int idx)
    {
        BGMs[idx].Play();
    }
    

    public void ChangeBGM()
    {
        switch (Scenes)
        {
            case EScenes.Title:
                BGMs[0].Play();
                break;
            case EScenes.Room1:
                BGMs[1].Play();
                break;
            case EScenes.Room2:
                BGMs[2].Play();
                break;
            case EScenes.Room3:
                BGMs[3].Play();
                break;
            case EScenes.Ending:
                BGMs[4].Play();
                break;
        }
    }

    public void PlaySFX(int idx)
    {
        //Debug.Log(SFXs[idx].name);
        SFXs[idx].Play();
    }
    public void StopSFX(int idx){
        SFXs[idx].Stop();
    }
}
