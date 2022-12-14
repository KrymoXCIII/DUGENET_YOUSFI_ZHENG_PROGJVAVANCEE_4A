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
       //AgentMode = ChooseMode.instance.CM1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (AgentMode == 0) // Random
        {
            RandomController();
        }
        else if (AgentMode == 1) // Humain
        {
            if (Input.GetKey(KeyCode.Z))
            {
<<<<<<< HEAD
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
=======
                Player.MovePlayerUp(gameObject,model1);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                Player.MovePlayerDown(gameObject,model1);
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                Player.MovePlayerLeft(gameObject,model1);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                Player.MovePlayerRight(gameObject,model1);
>>>>>>> parent of 97f5032 (fix movement)
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
            Debug.Log("okkkkkkk");
            MCTS mcts = new MCTS(map, Player);
            var listMoves = mcts.computeMCTS(50);
            float bestRate = 0;
            move bestMove = move.NULL;
            foreach (var move in listMoves)
            {
                if (move.nbWin / (float) move.nbWin > bestRate)
                {
                    bestRate = move.nbWin / (float) move.nbWin;
                    bestMove = move.moveP1;
                }
            }
            Debug.Log("mmmmmmmh");

            map.updateMap(Player, bestMove);
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
