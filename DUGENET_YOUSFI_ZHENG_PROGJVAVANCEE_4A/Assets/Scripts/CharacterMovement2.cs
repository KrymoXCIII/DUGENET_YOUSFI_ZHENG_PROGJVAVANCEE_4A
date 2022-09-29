using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterMovement2 : MonoBehaviour
{
    public int speed;
    public GameObject bomb;
    public Transform model2;
    


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
       AgentMode = ChooseMode.instance.CM2;
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
                transform.position += Player.MovePlayerUp(gameObject,model2);
            }
            else if (Input.GetKey(KeyCode.L))
            {
                transform.position += Player.MovePlayerDown(gameObject,model2);
            }
            else if (Input.GetKey(KeyCode.K))
            {
                transform.position += Player.MovePlayerLeft(gameObject,model2);
            }
            else if (Input.GetKey(KeyCode.M))
            {
                transform.position += Player.MovePlayerRight(gameObject,model2);
            }
            else if (Input.GetKey(KeyCode.KeypadEnter))
            {
                Player.PlantBomb();
            }
        }
        else if (AgentMode == 2) // MCTS
        {
            MCTS ia = new MCTS(map, Player);  
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
                
                transform.position += Player.MovePlayerUp(gameObject,model2);
                break;
            case 1 :
                
                transform.position += Player.MovePlayerDown(gameObject,model2);
                break;
            case 2 : 
                
                transform.position += Player.MovePlayerLeft(gameObject,model2);
                break;
            case 3 :
               
                transform.position += Player.MovePlayerRight(gameObject,model2);
                break;
            case 4 :
                
                Player.PlantBomb();
                break;
        }
    }
}
