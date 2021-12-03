using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFireBall : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;    
    private Vector2 thrownDirection;
    private float movementSpeed = 2.0f;
    public GameObject smokeAnimation;

    public Transform tm;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void setTarget(Vector2 targetPlayer){
        thrownDirection = new Vector2(targetPlayer.x - transform.position.x, targetPlayer.y - transform.position.y);
        float angle = Mathf.Atan2(thrownDirection.y, thrownDirection.x)* Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0,0, angle+180.0f));
    }

    private void FixedUpdate() {
        rb.MovePosition(rb.position + thrownDirection * movementSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag != "Enemy"){
            if (other.gameObject.transform.parent != null && other.gameObject.transform.parent.tag =="Player"){
                other.gameObject.transform.parent.GetComponent<PlayerMovement>().getHit();
            }else if(other.gameObject.tag == "BreakableObjects"){
                other.gameObject.GetComponent<BreakableRock>().rockHit(1);
            }
            Destroy(this.gameObject);
            GameObject smoke = Instantiate(smokeAnimation, rb.position, Quaternion.identity);
        }
    }
}

