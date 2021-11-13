using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shopDialogTrigger : MonoBehaviour
{
    public ShopKeeper shop;

    private void OnTriggerEnter2D(Collider2D other) {
        shop.startDialog(0);
    }

    private void OnTriggerExit2D(Collider2D other) {
        shop.stopDialog();
    }
}
