using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spikeTrap : MonoBehaviour
{
    [SerializeField]
    private GameObject spikesChild;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip startTrap;
    
    // Start is called before the first frame update
    private void Start() {
        spikesChild.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy"){
            audioSource.PlayOneShot(startTrap);
            Invoke("startSpikes", 1);
        }
    }

    private void startSpikes(){
        spikesChild.SetActive(true);
        Invoke("stopSpikes", 2);
    }

    private void stopSpikes(){
        spikesChild.SetActive(false);
    }

}
