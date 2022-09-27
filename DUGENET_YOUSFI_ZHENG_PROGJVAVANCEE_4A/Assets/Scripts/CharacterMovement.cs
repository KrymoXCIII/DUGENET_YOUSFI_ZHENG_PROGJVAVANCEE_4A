using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public int speed;
    public GameObject bomb;
    private Rigidbody _rb;
    public Map map;
    PlayerBomberman Player;
    



    // Start is called before the first frame update
    void Start()
    {
        Player = FindObjectOfType<PlayerBomberman>();
        _rb = GetComponent<Rigidbody>();


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            Player.MovePlayerUp(_rb);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            Player.MovePlayerUp(_rb);
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            Player.MovePlayerUp(_rb);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Player.MovePlayerUp(_rb);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            Player.PlantBomb(map);
        }
    }
}
