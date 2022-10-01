using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerSim : MonoBehaviour
{
    public Vector3 pos;
    public bool isAlive = true;
    public int nbBomb;
    public float speed;
    public int power;
    public int nbPlayer;

    public PlayerSim(PlayerSim pb)
    {
        pos = pb.pos;
        isAlive = pb.isAlive;
        nbBomb = pb.nbBomb;
        speed = pb.speed;
        power = pb.power;
        nbPlayer = pb.nbBomb;
    }
    
    public Vector3 MovePlayerUp()
    {
        return new Vector3(0, 0,speed);
    }
    public Vector3 MovePlayerDown()
    {
        return new Vector3(0, 0,-speed);
    }
    public Vector3 MovePlayerRight()
    {
        return new Vector3(speed,0, 0);
    }
    public Vector3 MovePlayerLeft()
    {
        return new Vector3(-speed,0, 0);
    }
    public BombSim PlantBomb()
    {
        var bombSim = new BombSim(pos, power);
        nbBomb--;
        return bombSim;
    }
}
