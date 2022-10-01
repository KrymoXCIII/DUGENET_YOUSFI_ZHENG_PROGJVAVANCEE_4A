using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class CharacterMovement : MonoBehaviour
{
    public MapSimulation map;
    public PlayerSim Player1;
    public PlayerSim Player2;
    public BombSim bombPrefab;

    public int AgentMode1;
    public int AgentMode2;

    private int randomControls;
    private float randomTimer;
    private bool randomReady;

    // Start is called before the first frame update
    void Start()
    {
        //Player = FindObjectOfType<PlayerBomberman>();
        randomControls = Random.Range(0, 5);
        AgentMode1 = ChooseMode.instance.CM1;
        AgentMode2 = ChooseMode.instance.CM2;
    }

    private void FixedUpdate()
    {
        map.updateMapSingleMove(getMoveJ1());
        map.updateMapSingleMove(getMoveJ2());
        map.updateBombs();
        updateScene();
        if (map.firstPlayer.isAlive == false)
        {
            
        }
        else if (map.secondPlayer.isAlive == false)
        {
            
        }
            
    }

    public void updateScene()
    {
        Player1.transform.position = Player1.pos;
        Player2.transform.position = Player2.pos;
        
        foreach (var wall in map.wallsToDestroy)
        {
            Destroy(wall.gameObject);
        }
        map.wallsToDestroy.Clear();
        foreach (var bomb in map.bombs1ToDestroy)
        {
            Destroy(bomb.gameObject);
        }
        map.bombs1ToDestroy.Clear();
        foreach (var bomb in map.bombs2ToDestroy)
        {
            Destroy(bomb.gameObject);
        }
        map.bombs2ToDestroy.Clear();

        foreach (var bomb in map.bombsToCreate)
        {
            var bombScene = Instantiate(bombPrefab).gameObject.GetComponent<BombSim>();
            bombScene.transform.position = bomb.pos;
            bombScene.timer = bomb.timer;
            bombScene.power = bomb.power;
        }
        map.bombsToCreate.Clear();
    }
    
    public move getMoveJ1()
    {
        if (AgentMode1 == 0) // Random
        {
            RandomController();
        }
        else if (AgentMode1 == 1) // Humain
        {
            if (Input.GetKey(KeyCode.Z))
                return move.UP;
            if (Input.GetKey(KeyCode.S))
                return move.DOWN;
            if (Input.GetKey(KeyCode.Q))
                return move.LEFT;
            if (Input.GetKey(KeyCode.D))
                return move.RIGHT;
            if (Input.GetKey(KeyCode.Space))
                return move.BOMB;
            return move.NOMOVE;
        }
        else if (AgentMode1 == 2) // MCTS
        {
            return MCTS(Player1);
        }
        return move.NULL;
    }

    move RandomController()
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
            case 0:
                return move.UP;
            case 1:
                return move.DOWN;
            case 2:
                return move.LEFT;
            case 3:
                return move.RIGHT;
            case 4:
                return move.BOMB;
        }
        return move.NULL;
    }
    
    public move getMoveJ2()
    {
        if (AgentMode2 == 0) // Random
        {
            return RandomController();
        }
        else if (AgentMode2 == 1) // Humain
        {
            if (Input.GetKey(KeyCode.O))
                return move.UP;
            if (Input.GetKey(KeyCode.L))
                return move.DOWN;
            if (Input.GetKey(KeyCode.K))
                return move.LEFT;
            if (Input.GetKey(KeyCode.M))
                return move.RIGHT;
            if (Input.GetKey(KeyCode.KeypadEnter))
                return move.BOMB;
            return move.NOMOVE;
        }
        else if (AgentMode2 == 2) // MCTS
        {
            return MCTS(Player1);
        }
        return move.NULL;
    }

    public move MCTS(PlayerSim pb)
    {
        MCTS mcts = new MCTS(map, pb);
        var listMoves = mcts.computeMCTS(1);
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
        return bestMove;
    }
}
