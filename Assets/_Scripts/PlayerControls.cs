using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    [SerializeField]
    private float speed = 100.0f;
    private Vector3 change;
    private Rigidbody2D myRigidbody;


    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");
        if (change != Vector3.zero)
        {
            MoveCharacter();
        }
    }

    void MoveCharacter()
    {
        myRigidbody.MovePosition(
            transform.position + change * speed * Time.deltaTime
        );
    }
    public void SpawnPlayer(Vector2 newPosition)
    {
        transform.position = newPosition;
    }
}
