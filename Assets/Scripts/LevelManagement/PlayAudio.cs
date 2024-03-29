﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class PlayAudio : MonoBehaviour
{
    /// <summary>
    /// Play Audio
    /// Dylan Loe
    /// 
    /// Updated June 15, 2020
    /// 
    /// - Playing audio events
    /// </summary>
    
    AudioSource audioData;
    // Start is called before the first frame update
    void Start()
    {
        audioData = GetComponent<AudioSource>();
        //audioData.Play(0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            audioData.Play();
            Debug.Log("play clip");
        }
    }
}
