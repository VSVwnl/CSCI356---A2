using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChange : MonoBehaviour
{
    public int weapon;
    private GameObject player;
    public GameObject mainCamera;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        Camera.main.GetComponent<Shooter>().changeWeapon(weapon);
    }
}