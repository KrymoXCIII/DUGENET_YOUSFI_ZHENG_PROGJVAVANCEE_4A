using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.TestTools;

public enum move
{
    UP,DOWN,RIGHT,LEFT,BOMB,NOMOVE,NULL
}

public class GameManager  
{
    
}

public class PlayerSim
{
    public Vector3 pos;
    public bool isAlive;
    public int nbBomb;
    public float speed;
    public int power;
    public int nbPlayer;

    public PlayerSim(PlayerBomberman pb)
    {
        pos = new Vector3(pb.transform.position.x, pb.transform.position.y,pb.transform.position.z);
        isAlive = pb.isAlive;
        nbBomb = pb.nbBombes;
        speed = pb.speed;
        power = pb.bombPower;
        nbPlayer = pb.playerNb;
    }
    
    public Vector3 MovePlayerUp()
    {
        return new Vector3(speed, 0,0);
    }
    public Vector3 MovePlayerDown()
    {
        return new Vector3(-speed, 0,0);
    }
    public Vector3 MovePlayerRight()
    {
        return new Vector3(0,0, speed);
    }
    public Vector3 MovePlayerLeft()
    {
        return new Vector3(0,0, -speed);
    }
    public BombSim PlantBomb()
    {
        var bomb = new BombSim(pos, power);
        nbBomb--;
        return bomb;
    }
}

public class BombSim
{
    public Vector3 pos;
    public int power;
    public float timer;
    //private float radius = 1.5f;
    
    public BombSim(Bomb b)
    {
        pos = new Vector3(b.transform.position.x,b.transform.position.y, b.transform.position.z);
        power = b.power;
        timer = b.timer;
    }
    
    public BombSim(Vector3 position, int pow)
    {
        pos = position;
        power = pow;
        timer = 60;
    }

    public bool decreaseTimer()
    {
        timer -= 1;
        return (timer == 0);
    }
}

public class WallSim
{
    public Vector3 pos;
    public bool destructible;
    public float percentItem = 0.5f;
    
    public WallSim(Wall w)
    {
        pos = new Vector3(w.transform.position.x, w.transform.position.y,w.transform.position.z);
        destructible = w.destructible;
        percentItem = w.percentItem;
    }
}

public class mapSimulation
{
    public PlayerSim firstPlayer;
    public PlayerSim secondPlayer;
    public List<BombSim> bombs1 = new List<BombSim>();
    public List<BombSim> bombs2 = new List<BombSim>();
    public List<WallSim> walls = new List<WallSim>();
    public float collisionRadius = 1f;
    public float deltaTime = .016f;

    public mapSimulation(Map map, PlayerBomberman pb1)
    {
        firstPlayer = new PlayerSim(pb1);
        secondPlayer = new PlayerSim(map.players.Where(t => t != pb1).First());

        foreach (var wall in map.walls)
        {
            walls.Add(new WallSim(wall));
        }

        if (pb1.playerNb == 1)
        {
            foreach (var bomb in map.bombs1)
            {
                bombs1.Add(new BombSim(bomb));
            }
            foreach (var bomb in map.bombs2)
            {
                bombs2.Add(new BombSim(bomb));
            }
        }
        else
        {
            foreach (var bomb in map.bombs2)
            {
                bombs1.Add(new BombSim(bomb));
            }
            foreach (var bomb in map.bombs1)
            {
                bombs2.Add(new BombSim(bomb));
            }
        }
    }
    public mapSimulation(mapSimulation map)
    {
        firstPlayer = map.firstPlayer;
        secondPlayer = map.secondPlayer;
        bombs1 = map.bombs1;
        bombs2 = map.bombs2;
        walls = map.walls;
    }

    public void updateMap(move m1, move m2)
    {
        switch (m1)
        {
            case move.UP:
                firstPlayer.pos += firstPlayer.MovePlayerUp() * deltaTime;
                break;
            case move.DOWN:
                firstPlayer.pos += firstPlayer.MovePlayerDown() * deltaTime;
                break;
            case move.RIGHT:
                firstPlayer.pos += firstPlayer.MovePlayerRight() * deltaTime;
                break;
            case move.LEFT:
                firstPlayer.pos += firstPlayer.MovePlayerLeft() * deltaTime;
                break;
            case move.BOMB:
                var bomb = firstPlayer.PlantBomb();
                bombs1.Add(bomb);
                break;
        }

        switch (m2)
        {
            case move.UP:
                secondPlayer.pos += secondPlayer.MovePlayerUp() * deltaTime;
                break;
            case move.DOWN:
                secondPlayer.pos += secondPlayer.MovePlayerDown() * deltaTime;
                break;
            case move.RIGHT:
                secondPlayer.pos += secondPlayer.MovePlayerRight() * deltaTime;
                break;
            case move.LEFT:
                secondPlayer.pos += secondPlayer.MovePlayerLeft() * deltaTime;
                break;
            case move.BOMB:
                var bomb = secondPlayer.PlantBomb();
                bombs2.Add(bomb);
                break;
        }
        
        List<BombSim> bombToDelete = new List<BombSim>();
        
        foreach (var b in bombs2)
        {
            if (b.decreaseTimer())
            {
                explodeBomb(b);
                //Debug.Log("explosion ptn " + firstPlayer.isAlive);
                secondPlayer.nbBomb++;
                bombToDelete.Add(b);
            }
        }
        foreach (var b in bombToDelete)
        {
            bombs2.Remove(b);
        }
        
        bombToDelete.Clear();

        foreach (var b in bombs1)
        {
            if (b.decreaseTimer())
            {
                explodeBomb(b);
                firstPlayer.nbBomb++;
                bombToDelete.Add(b);
            }
        }
        foreach (var b in bombToDelete)
        {
            bombs1.Remove(b);
        }
    }

    public bool checkPossibleMove(move m, PlayerSim ps)
    {
        Vector3 dir;
        switch (m)
        {
            case move.UP:
                dir = ps.MovePlayerUp()*deltaTime;
                return (!collisionPlayerWalls(ps.pos+dir));
            case move.DOWN:
                dir = ps.MovePlayerDown()*deltaTime;
                return (!collisionPlayerWalls(ps.pos+dir));                
            case move.RIGHT:
                dir = ps.MovePlayerRight()*deltaTime;
                return (!collisionPlayerWalls(ps.pos+dir));
            case move.LEFT:
                dir = ps.MovePlayerLeft()*deltaTime;
                return (!collisionPlayerWalls(ps.pos+dir));
            case move.BOMB:
                return (ps.nbBomb > 0);
            case move.NOMOVE:
                return true;
            case move.NULL:
                return false;
        }
        return false;
    }
    
    public bool collisionPlayerWalls(Vector3 checkPos)
    {
        foreach (var wall in walls)
        {
            if (wall.pos.x + collisionRadius > checkPos.x &&
                wall.pos.x - collisionRadius < checkPos.x &&
                wall.pos.z + collisionRadius > checkPos.z &&
                wall.pos.z - collisionRadius < checkPos.z)
                return true;
        }
        return false;
    }

    public void explodeBomb(BombSim b)
    {
        List<WallSim> wallToRemove = new List<WallSim>();
        var pos = b.pos;
        foreach (var wall in (walls))
        {
            if (wall.destructible)
            {
                if (collisionBomb(wall.pos, pos))
                {
                    wallToRemove.Add(wall);
                    wall.destructible = false;
                }
            }
        }

        for (int i = 1; i < b.power; i++)
        {
            float delta = i * 1.5f;
            foreach (var wall in (walls))
            {
                if (wall.destructible)
                {
                    var newPos = new Vector3(pos.x, pos.y, pos.z);
                    newPos.Set(pos.x + delta, pos.y, pos.z);
                    if (collisionBomb(wall.pos, newPos))
                    {

                        wallToRemove.Add(wall);
                        wall.destructible = false;
                    }

                    newPos.Set(pos.x, pos.y, pos.z + delta);
                    if (collisionBomb(wall.pos, newPos))
                    {
                        if (wall.destructible)
                        {
                            wallToRemove.Add(wall);
                            wall.destructible = false;
                        }
                    }

                    newPos.Set(pos.x, pos.y, pos.z - delta);
                    if (collisionBomb(wall.pos, newPos))
                    {
                        if (wall.destructible)
                        {
                            wallToRemove.Add(wall);
                            wall.destructible = false;
                        }
                    }

                    newPos.Set(pos.x - delta, pos.y, pos.z);
                    if (collisionBomb(wall.pos, newPos))
                    {
                        if (wall.destructible)
                        {
                            wallToRemove.Add(wall);
                            wall.destructible = false;
                        }
                    }
                }
            }
        }

        var players = new List<PlayerSim>(2);
        players.Add(firstPlayer);
        players.Add(secondPlayer);

        foreach (var player in players)
        {
            if (collisionBomb(player.pos, pos))
            {
                player.isAlive = false;
            }
        }
        for (int i = 1; i < b.power; i++)
        {
            float delta = i * 1.5f;
            foreach (var player in players)
            {
                var newPos = new Vector3(pos.x, pos.y, pos.z);
                newPos.Set(pos.x+delta, pos.y, pos.z);
                if (collisionBomb(player.pos, newPos))
                {
                    player.isAlive = false;
                }

                newPos.Set(pos.x, pos.y, pos.z+delta);
                if (collisionBomb(player.pos, newPos))
                {
                    player.isAlive = false;
                }

                newPos.Set(pos.x, pos.y, pos.z-delta);
                if (collisionBomb(player.pos, newPos))
                {
                    player.isAlive = false;
                }

                newPos.Set(pos.x-delta, pos.y, pos.z);
                if (collisionBomb(player.pos, newPos))
                {
                    player.isAlive = false;
                }
            }
        }

        foreach (var r in wallToRemove)
        {
            walls.Remove(r);
        }
    }
    
    public bool collisionBomb(Vector3 targetPos, Vector3 checkPos)
    {
        if (Mathf.Pow(targetPos.x - checkPos.x, 2) + Mathf.Pow(targetPos.z - checkPos.z, 2) < 1)
            return true;
        return false;
    }

    public PlayerSim checkWinner()
    {
        if (!firstPlayer.isAlive)
            return secondPlayer;
        if (!secondPlayer.isAlive)
            return firstPlayer;
        return null;
    }

    public List<Tuple<move, move>> getPossibleMove(PlayerSim first, PlayerSim second)
    {
        List<Tuple<move, move>> returnList = new List<Tuple<move, move>>();
        
        List<move> listMove = new List<move>();
        listMove.Add(move.UP);
        listMove.Add(move.DOWN);
        listMove.Add(move.RIGHT);
        listMove.Add(move.LEFT);
        listMove.Add(move.BOMB);
        listMove.Add(move.NOMOVE);

        foreach (var m1 in listMove)
        {
            if (checkPossibleMove(m1, first))
            {
                foreach (var m2 in listMove)
                {
                    if (checkPossibleMove(m2, second))
                        returnList.Add(new Tuple<move, move>(m1,m2));
                }
            }
        }
        return returnList;
    }
 }

