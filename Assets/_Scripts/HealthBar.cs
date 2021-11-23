using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Aidan von Holwede
// 11213321
// ajv782
// CMPT 306
// Cody Phillips
// A1

public class HealthBar : MonoBehaviour
{

    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public Vector3 offset;

    public AudioClip hitSoundEffect;

    public GameObject notch;
    public RectTransform healthFrame;

    public List<GameObject> notchList = new List<GameObject>();
    


    public void setMaxHealth(int health) {

        int notchCount = notchList.Count;                                   // Calculates the amount of notches (if a non-startup function call was made)

        for (int i = 0; i < notchCount; i++) {                              // Removes all previously existing notches if this is a non-startup call
            GameObject selectedNotch = notchList[0];
            notchList.RemoveAt(0);
            Destroy(selectedNotch);
        }

        
        slider.maxValue = health;                                           // Sets slider maximum and current values (full HP)
        slider.value = health;

        slider.gameObject.SetActive(true);

        fill.color = gradient.Evaluate(1f);

        int numNotches = health / 25;                                      // Each notch is set at intervals of 100 HP

        // Adds the amount of needed notches and positions them on the HP bar
        // to show chunks of 100 HP
        for (int i = 1; i <= numNotches; i++) { 
            GameObject currentNotch = Instantiate(notch, healthFrame);                  // Creates new notch
            currentNotch.transform.localPosition = new Vector3(healthFrame.rect.xMin + healthFrame.rect.width * (float) (25 * i) / health, 0, 0);              // Sets each notch's position
            notchList.Add(currentNotch);                                                // Adds new notch to the container
        }

        Text healthText = this.transform.GetChild(3).GetComponent<Text>();
        healthText.text = "Current Health: " + health;
    }
    
    public void setHealth(int health) {

        slider.gameObject.SetActive(true);                                  // Sets the health of the enemy - could be either from a heal or damage
        
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);             // Changes colour of bar if certain thresholds are met

        AudioSource.PlayClipAtPoint(hitSoundEffect, transform.position);    // Play punch sound effect

        Text healthText = this.transform.GetChild(3).GetComponent<Text>();
        healthText.text = "Current Health: " + health;

        
    }

    void Update() {
        slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.parent.position + offset);      // Update health bar position relative to the enemy's position
    }
}
