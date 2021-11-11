using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    
    public GameObject floatingText;
    private GameObject tempText;
    public Transform tm;
    [SerializeField]
    private int[] damages = {40, 60, 80, 100, 120, 150};

    private int damage;
    private void Start() {
        if (this.gameObject.tag == "sword1"){
            damage = damages[0];
        }
        else if (this.gameObject.tag == "sword2"){
            damage = damages[1];
        }
        else if (this.gameObject.tag == "sword3"){
            damage = damages[2];
        }
        else if (this.gameObject.tag == "sword4"){
            damage = damages[3];
        }
        else if (this.gameObject.tag == "sword5"){
            damage = damages[4];
        }
        else if (this.gameObject.tag == "sword6"){
            damage = damages[5];
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player"){
            tempText = Instantiate(floatingText, tm.position + new Vector3(0, 2, 0), Quaternion.identity, tm);
            tempText.GetComponent<TextMesh>().text = damage + " Damage";
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "Player"){
            Destroy(tempText, 0.1f);
        }
    }
}
