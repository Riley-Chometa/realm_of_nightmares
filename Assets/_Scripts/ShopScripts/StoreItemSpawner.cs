using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreItemSpawner : MonoBehaviour
{
    public GameObject[] weapons;
    public GameObject heart;
    public GameObject[] rangedWeapons;

    [SerializeField]
    private GameObject[] spawnLocations;
    private GameObject tempHeart;
    private GameObject tempWeapon;
    private GameObject tempRanged;
    void Start()
    {
        int randomWeaponIndex = Random.Range(0, 5);
    }
}
