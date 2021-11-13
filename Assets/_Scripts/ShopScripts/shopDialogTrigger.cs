using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shopDialogTrigger : MonoBehaviour
{
    public ShopKeeper shop;
    private bool played = false;
    private void OnTriggerEnter2D(Collider2D other) {
        if (!played){
        shop.startDialog(0, "This is the Store!");
        played = true;
        }
    }

    // private void OnTriggerExit2D(Collider2D other) {
    //     shop.stopDialog();
    // }
}
