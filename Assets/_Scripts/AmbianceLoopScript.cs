using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbianceLoopScript : MonoBehaviour
{   
    public AudioSource source;
    public AudioClip[] clips;
    private void Start() {

        source.clip = clips[Random.Range(0, clips.Length)];
        source.Play();
    }
}
