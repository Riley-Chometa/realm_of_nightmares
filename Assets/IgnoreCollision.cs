using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollision : MonoBehaviour
{
    // Start is called before the first frame update

    public CapsuleCollider2D characterCollider;
    public CapsuleCollider2D characterCollisionBlocker;
    void Start()
    {
        Physics2D.IgnoreCollision(characterCollider, characterCollisionBlocker, true);
    }

    
}
