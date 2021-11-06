using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class starThrower : MonoBehaviour
{
    [SerializeField]
    private Transform tm;

    public GameObject star;
    // Start is called before the first frame update
    void Start()
    {
        tm = GetComponent<Transform>();
        InvokeRepeating("createStar", 2.0f, 2.0f);
    }

    private void createStar(){
        Instantiate(star, tm.position, Quaternion.identity);
    }
}
