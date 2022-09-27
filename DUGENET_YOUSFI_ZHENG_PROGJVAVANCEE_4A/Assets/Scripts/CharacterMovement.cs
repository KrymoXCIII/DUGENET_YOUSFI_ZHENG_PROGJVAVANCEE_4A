using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public int speed;
    public GameObject bomb;
    private Rigidbody rb;

    PlayerBomberman Player;
    



    // Start is called before the first frame update
    void Start()
    {
        Player = FindObjectOfType<PlayerBomberman>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Player.MovePlayer(gameObject);
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Player.PlantBomb(bomb,gameObject);

        }
        
    }
}
