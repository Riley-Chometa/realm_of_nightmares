using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    
    public GameObject floatingText;
    public GameObject pressE;
    private GameObject tempText;
    private GameObject tempE;
    public Transform tm;
    [SerializeField]
    private int[] damages = {40, 60, 80, 100, 120, 150};

    private string printMe;
    private int damage;
    private void Start() {
        if (this.gameObject.tag == "sword1"){
            printMe = damages[0] + " Damage";
        }
        else if (this.gameObject.tag == "sword2"){
            printMe = damages[1] + " Damage";
        }
        else if (this.gameObject.tag == "sword3"){
            printMe = damages[2] + " Damage";
        }
        else if (this.gameObject.tag == "sword4"){
            printMe = damages[3] + " Damage";
        }
        else if (this.gameObject.tag == "sword5"){
            printMe = damages[4] + " Damage";
        }
        else if (this.gameObject.tag == "sword6"){
            printMe = damages[5] + " Damage";
        }
        else if (this.gameObject.tag == "FireBow"){
            printMe = "Fire Bow";
        }
        else if (this.gameObject.tag == "BasicBow"){
            printMe = "Basic Bow";
        }
        else if (this.gameObject.tag == "FirePotion"){
            printMe = "Fire Ball Potion!";
        }

    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player"){
            tempText = Instantiate(floatingText, tm.position + new Vector3(0, 2, 0), Quaternion.identity, tm);
            tempText.GetComponent<TextMesh>().text = printMe;
            tempE = Instantiate(pressE, tm.position + new Vector3(0, -1, 0), Quaternion.identity, tm);
            tempE.GetComponent<Transform>().localScale = new Vector3(.1f, .1f, .1f);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "Player"){
            Destroy(tempText, 0.1f);
            Destroy(tempE, .01f);
        }
    }
}
