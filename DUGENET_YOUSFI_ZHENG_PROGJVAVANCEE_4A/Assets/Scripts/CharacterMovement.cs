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
    public int AgentMode;

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
            Player.MovePlayerUp(gameObject);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            Player.MovePlayerDown(gameObject);
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            Player.MovePlayerLeft(gameObject);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Player.MovePlayerRight(gameObject);
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            Player.PlantBomb(map);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Player.PlantBomb(map);
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Player.PlantBomb(map);
        }
    }
}
