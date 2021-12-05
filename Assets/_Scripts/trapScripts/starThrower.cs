using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class starThrower : MonoBehaviour
{
    [SerializeField]
    private Transform tm;
    private float spawnTime;
    public GameObject star;
    Vector2 throwDirection;
    // Start is called before the first frame update
    void Start()
    {
        tm = GetComponent<Transform>();
        spawnTime = Random.Range(.85f, 1.05f);
        // if facing right
        if (this.gameObject.transform.rotation.eulerAngles.y == 180){
            throwDirection = Vector2.right;
            // Debug.Log("Facing Right");
        }
        else{
            throwDirection = Vector2.left;
        }
    }

    public void activateStarThrower(){
        InvokeRepeating("createStar", spawnTime, spawnTime);
    }

    public void deactivateStarThrower(){
        CancelInvoke();
    }

    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player"){
            activateStarThrower();
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "Player"){
            deactivateStarThrower();
        }
    }

    private void createStar(){
        GameObject newStar = Instantiate(star, tm.position, Quaternion.identity, tm);
        newStar.GetComponent<starMovement>().setDirection(throwDirection);
    }
}
