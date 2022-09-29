using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterMovement1 : MonoBehaviour
{
    public Map map;
    public PlayerBomberman Player;
    public int AgentMode;
    private int randomControls;
    private float randomTimer;
    private bool randomReady;
    
    // Start is called before the first frame update
    void Start()
    {
        //Player = FindObjectOfType<PlayerBomberman>();
       randomControls = Random.Range(0, 5);
       AgentMode = ChooseMode.instance.CM1;
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
            if (Input.GetKey(KeyCode.Z))
            {
                map.updateMap(Player, move.UP);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                map.updateMap(Player, move.DOWN);
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                map.updateMap(Player, move.LEFT);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                map.updateMap(Player, move.RIGHT);
            }
            else if (Input.GetKey(KeyCode.Space))
            {
                map.updateMap(Player, move.BOMB);
            }
            else
                map.updateMap(Player, move.NOMOVE);

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
                map.updateMap(Player, move.UP);
                break;
            case 1 :
                map.updateMap(Player, move.DOWN);
                break;
            case 2 :
                map.updateMap(Player, move.LEFT);
                break;
            case 3 :
                map.updateMap(Player, move.RIGHT);
                break;
            case 4 :
                map.updateMap(Player, move.BOMB);
                break;
        }
    }
}
