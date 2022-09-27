using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterMovement2 : MonoBehaviour
{
    public int speed;
    public GameObject bomb;
    private Rigidbody _rb;


    public Map map;
    public PlayerBomberman Player;
    public int AgentMode;
    private int randomControls;
    private float randomTimer;
    private bool randomReady;
    
    // Start is called before the first frame update
    void Start()
    {
       Player = FindObjectOfType<PlayerBomberman>();
       randomControls = Random.Range(0, 5);
       //AgentMode = ChooseMode.instance.CM2;
    }

    // Update is called once per frame
    void Update()
    {
        if (AgentMode == 0) // Random
        {
            RandomController();
        }
        else if (AgentMode == 1) // Humain
        {
            if (Input.GetKey(KeyCode.O))
            {
                Player.MovePlayerUp(Player.gameObject);
            }
            else if (Input.GetKey(KeyCode.L))
            {
                Player.MovePlayerDown(Player.gameObject);
            }
            else if (Input.GetKey(KeyCode.K))
            {
                Player.MovePlayerLeft(Player.gameObject);
            }
            else if (Input.GetKey(KeyCode.M))
            {
                Player.MovePlayerRight(Player.gameObject);
            }
            else if (Input.GetKey(KeyCode.KeypadEnter))
            {
                Player.PlantBomb(map);
            }
        }
        else if (AgentMode == 2) // MCTS
        {
            
        }
        
    }

    void RandomController()
    {
        if (randomReady)
        {
            randomTimer = Time.time;

            randomControls = Random.Range(0, 5);
            randomReady = false;
        }
        
        if (Time.time - randomTimer >= 2)
        {

            
            randomReady = true;

        }


        switch (randomControls)
        {
            case 0 :
                
                Player.MovePlayerUp(Player.gameObject);
                break;
            case 1 :
                
                Player.MovePlayerDown(Player.gameObject);
                break;
            case 2 : 
                
                Player.MovePlayerLeft(Player.gameObject);
                break;
            case 3 :
               
                Player.MovePlayerRight(Player.gameObject);
                break;
            case 4 :
                
                Player.PlantBomb(map);
                break;
        }
    }
}
