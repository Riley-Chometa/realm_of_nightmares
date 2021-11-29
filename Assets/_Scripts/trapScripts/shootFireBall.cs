using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootFireBall : MonoBehaviour
{
    public GameObject fireball;
    private Transform tm;
    public Vector2 throwDirection;

    private void Start() {
        tm = this.GetComponent<Transform>();
    }
    public void setDirection(Vector2 direction){
        throwDirection = direction;
    }
    public void createFireBall(){
        GameObject newFireBall = Instantiate(fireball, tm.position, Quaternion.identity);
        newFireBall.GetComponent<trapFireBall>().setDirection(throwDirection);
        Destroy(this.gameObject, 1);
    }
}
