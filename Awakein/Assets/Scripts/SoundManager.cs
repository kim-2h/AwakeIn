using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource[] BGMs;
    private enum EScenes
    {
        Lobby, Room1, Room2, Room3, Ending

    }
    EScenes Scenes = EScenes.Lobby;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        BGMs[0].Play();
    }

    public void ChangeBGM()
    {
        switch (Scenes)
        {
            case EScenes.Lobby:
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

}
