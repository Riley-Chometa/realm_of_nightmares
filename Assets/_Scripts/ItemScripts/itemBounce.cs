using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemBounce : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;
    private float heightTime = .25f;
    private float currTime;
    private float moveSpeed = .25f;
    private bool goingUp = true;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        currTime = Random.Range(0, 1);
    }

    private void FixedUpdate() {
        if (goingUp){
            rb.MovePosition(rb.position + moveSpeed * Vector2.up*Time.fixedDeltaTime);
        }
        else{
            rb.MovePosition(rb.position + moveSpeed * Vector2.down*Time.fixedDeltaTime);
        }
        if (currTime < 0){
            goingUp = !goingUp;
            currTime = heightTime;
        }
        currTime -= Time.fixedDeltaTime;
    }
}
