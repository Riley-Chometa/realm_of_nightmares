using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyDamageText : MonoBehaviour
{
    private Vector3 offset = new Vector3(0, 2.5f, 0);
    public float destroyTime = 1f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, destroyTime);
        this.gameObject.GetComponent<MeshRenderer>().sortingOrder = 11;
        transform.localPosition += offset;
    }

}
