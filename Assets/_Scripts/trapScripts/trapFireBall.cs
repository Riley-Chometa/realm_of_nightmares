using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trapFireBall : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;    

    public Vector2 thrownDirection;
    private float movementSpeed = 12.0f;
    public GameObject smokeAnimation;

    public Transform tm;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void setDirection(Vector2 direction){
        thrownDirection = direction;
        if (thrownDirection == Vector2.right){
            tm.Rotate(new Vector3(0.0f, 180.0f, 0.0f), Space.World);
        }
        else if (thrownDirection == Vector2.down){
            tm.Rotate(0.0f, 0.0f, 90.0f, Space.World);
        }
        else if (thrownDirection == Vector2.up){
            tm.Rotate(0.0f, 0.0f, -90.0f, Space.World);
        }
    }

    private void FixedUpdate() {
        rb.MovePosition(rb.position + thrownDirection * movementSpeed * Time.fixedDeltaTime);
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
            if (other.gameObject.tag == "Enemy"){
                other.gameObject.GetComponent<BaseEnemy>().TakeDamage(50);
            }
            else if (other.gameObject.tag == "Player"){
                other.gameObject.GetComponent<PlayerMovement>().getHit();   
            }
            Destroy(this.gameObject);
            GameObject smoke = Instantiate(smokeAnimation, rb.position, Quaternion.identity);
    }
}
