using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class starMovement : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;    

    public Vector2 thrownDirection;
    private float movementSpeed = 5.0f;
    public GameObject smokeAnimation;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void setDirection(Vector2 direction){
        thrownDirection = direction;
    }

    private void FixedUpdate() {
        rb.MovePosition(rb.position + thrownDirection * movementSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Destroy(this.gameObject);
        GameObject smoke = Instantiate(smokeAnimation, rb.position, Quaternion.identity);
    }
}
