using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public int speed;
    public GameObject bomb;
<<<<<<< Updated upstream
    private Rigidbody _rb;

=======
    private Rigidbody rb;
    
>>>>>>> Stashed changes
    PlayerBomberman Player;
    public int AgentMode;


    
    



    // Start is called before the first frame update
    void Start()
    {
<<<<<<< Updated upstream
        Player = GetComponent<PlayerBomberman>();
        _rb = GetComponent<Rigidbody>();
=======
        Player = FindObjectOfType<PlayerBomberman>();
        
>>>>>>> Stashed changes
    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< Updated upstream
<<<<<<< Updated upstream
        Player.MovePlayer(_rb);
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Player.PlantBomb(bomb,gameObject);

=======
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Player.MovePlayerUp(_rb);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Player.MovePlayerDown(_rb);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            Player.MovePlayerLeft(_rb);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Player.MovePlayerRight(_rb);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            Player.PlantBomb(map);
>>>>>>> Stashed changes
=======
        Player.MovePlayer(gameObject);
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Player.PlantBomb(bomb,gameObject);

>>>>>>> Stashed changes
        }
        
    }
}
