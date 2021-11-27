using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCannon : MonoBehaviour
{
    public Transform left;
    public Transform right;
    public Transform up;
    public Transform down;
    private float spawnTime;
    public GameObject fireBallShoot;
    Vector2 throwDirectionEast = Vector2.right; // East or North
    Vector2 throwDirectionWest = Vector2.left; // West or South

    Vector2 throwDirectionNorth = Vector2.up;
    Vector2 throwDirectionSouth = Vector2.down;
    // Start is called before the first frame update
    void Start()
    {
        spawnTime = Random.Range(2.1f, 3.1f);
        // Invoke("createFire1", spawnTime);
        InvokeRepeating("createFire1", spawnTime, spawnTime);
        InvokeRepeating("createFire2", spawnTime, spawnTime);
        InvokeRepeating("createFire3", spawnTime, spawnTime);
        InvokeRepeating("createFire4", spawnTime, spawnTime);
    }
    private void createFire1(){
        GameObject fireBall1 = Instantiate(fireBallShoot, left.position, Quaternion.identity);
        fireBall1.GetComponent<shootFireBall>().setDirection(throwDirectionWest);
    }
    private void createFire2(){
        GameObject fireBall2 = Instantiate(fireBallShoot, right.position, Quaternion.identity);
        fireBall2.GetComponent<shootFireBall>().setDirection(throwDirectionEast);
    }

    private void createFire3(){
        GameObject fireBall3 = Instantiate(fireBallShoot, down.position, Quaternion.identity);
        fireBall3.GetComponent<shootFireBall>().setDirection(throwDirectionSouth);
    }

    private void createFire4(){
        GameObject fireBall4 = Instantiate(fireBallShoot, up.position, Quaternion.identity);
        fireBall4.GetComponent<shootFireBall>().setDirection(throwDirectionNorth);
    }
}
