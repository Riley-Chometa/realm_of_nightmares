using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb_Explosion : MonoBehaviour
{

    //Variables for the bomb's explosion
    public Animator anim;
    public AudioSource source;
    public AudioClip countDown;
    public AudioClip explosionSound;

    //Variables for making the explosion bigger. 
    public GameObject bomb;
    private Vector3 scaleChange;
    public int bombDamage;

    //To hold all of the game objects that the bomb comes into contact with. 
    private List<GameObject> list = new List<GameObject>(); 

    // Start is called before the first frame update
    void Start()
    {
        scaleChange = new Vector3(18f, 18f, 1f);
        Debug.Log("Using a bomb");
        StartCoroutine(bombExplosion());
    }

    // Update is called once per frame
    void Update()
    {
       // if (Input.GetKeyDown("b"))
       // {
         //   Debug.Log("Using a bomb");
           // StartCoroutine(bombExplosion());
       // }
    }

    private void OnTriggerEnter2D(Collider2D coli)
    {
        if (coli.gameObject.tag == "Player")
        {
            list.Add(coli.gameObject);
            Debug.Log(coli.gameObject.name + " has entered the bomb radius");
        }
        if (coli.gameObject.tag == "BreakableObjects")
        {
            list.Add(coli.gameObject);
            Debug.Log(coli.gameObject.name + " has entered the bomb radius");
        }
        if (coli.gameObject.tag == "Enemy")
        {
            list.Add(coli.gameObject);
            Debug.Log(coli.gameObject.name + " has entered the bomb radius");
        }
    }

    private void OnTriggerExit2D(Collider2D coli)
    {
        if (coli.gameObject.tag == "Player")
        {
            list.Remove(coli.gameObject);
            Debug.Log(coli.gameObject.name + " has left the bomb radius");
        }
        if (coli.gameObject.tag == "enemy")
        {
            list.Remove(coli.gameObject);
            Debug.Log(coli.gameObject.name + "has left the bomb radius");
            //We don't need to do one of these for the breakable objects since they do not move. 
        }
    }
    //Create a function for the bomb object explosion
    public IEnumerator bombExplosion()
    {
        yield return new WaitForSeconds(0.5f);
        source.PlayOneShot(countDown);
        anim.Play("Bomb_3");
        yield return new WaitForSeconds(1);
        anim.Play("Bomb_2");
        yield return new WaitForSeconds(1);
        anim.Play("Bomb_1");
        yield return new WaitForSeconds(1);
        source.PlayOneShot(explosionSound);
        bomb.transform.localScale += scaleChange;
        //Once the gameobject grows in size for the explosion we'll need two if statements for if it collides with specific entities by checking if any object are in range. 
        anim.Play("ExplosionEffect");
        foreach (var obj in list)
        {
            if (obj.gameObject.tag == "Player") //If the player is within the range of the bomb
            {
                obj.gameObject.GetComponent<PlayerMovement>().getHit();
                GameObject.Find("PlayerStats").GetComponent<PlayerStatsComponent>().modifyHealth(-2);
            }
            if (obj.gameObject.tag == "BreakableObjects")
            {
                obj.gameObject.GetComponent<BreakableRock>().destroyRock();
            }
            if (obj.gameObject.tag == "Enemy")
            {
                obj.gameObject.GetComponent<BaseEnemy>().TakeDamage(bombDamage);
            }
        }

        yield return new WaitForSeconds(0.90f);
        Destroy(gameObject);
        Debug.Log("Bomb has been detonated");
    }
}
