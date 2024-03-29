using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;    

    public Vector2 thrownDirection;
    private float movementSpeed = 10.0f;
    public GameObject smokeAnimation;

    public Transform tm;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void setDirection(Vector2 direction){
        thrownDirection = direction;
        if (thrownDirection == Vector2.left){
            tm.Rotate(0.0f, 180.0f, 0.0f);
        }
        else if (thrownDirection == Vector2.down){
            tm.Rotate(0.0f, 0.0f, -90.0f);
        }
        else if (thrownDirection == Vector2.up){
            tm.Rotate(0.0f, 0.0f, 90.0f);
        }
    }

    private void FixedUpdate() {
        rb.MovePosition(rb.position + thrownDirection * movementSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag != "Player"){
            if (other.gameObject.tag == "Enemy"){
                other.gameObject.GetComponent<BaseEnemy>().TakeDamage(30);
            }
            Destroy(this.gameObject);
            GameObject smoke = Instantiate(smokeAnimation, rb.position, Quaternion.identity);
        }
    }
}
