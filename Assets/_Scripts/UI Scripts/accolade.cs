using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class accolade : MonoBehaviour
{
    private Vector3 offset = new Vector3(0, 3f, 0);
    public float destroyTime = 1f;
    // Start is called before the first frame update
    void Start()
    {   
        int index = Random.Range(0, 5);
        this.gameObject.GetComponent<Animator>().SetInteger("chosenAccolade", index);
        transform.localPosition += offset;
        Destroy(this.gameObject, destroyTime);
    }
}
