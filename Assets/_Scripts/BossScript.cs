using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : Unit
{
    // Start is called before the first frame update
    void Awake()
    {
        transform.localScale = new Vector3(1.2f ,1.2f ,1.2f);
        base.Awake();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }
}
