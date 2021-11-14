using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBow : MonoBehaviour
{
    public void pickUpMe(){
        Destroy(this.gameObject);
    }
}
