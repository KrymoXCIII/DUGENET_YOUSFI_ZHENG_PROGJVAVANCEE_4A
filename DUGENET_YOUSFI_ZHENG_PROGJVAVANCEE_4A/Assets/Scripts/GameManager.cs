using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor.UIElements;
using UnityEngine;

public enum move
{
    UP,DOWN,RIGHT,LEFT,BOMB
}

public class GameManager  
{
    
}

public class PlayerSim
{
    public Vector2 pos;
    public bool isAlive;
    public int nbBomb;
    public float speed;
    public int power;

    public PlayerSim(PlayerBomberman pb)
    {
        pos = new Vector2(pb.transform.position.x, pb.transform.position.z);
        isAlive = pb.isAlive;
        nbBomb = pb.nbBombes;
        speed = pb.speed;
        power = pb.bombPower;
    }
    
    public Vector2 MovePlayerUp()
    {
        return new Vector2(speed, 0);
    }
    public Vector2 MovePlayerDown()
    {
        return new Vector2(-speed, 0);
    }
    public Vector2 MovePlayerRight()
    {
        return new Vector2(0, speed);
    }
    public Vector2 MovePlayerLeft()
    {
        return new Vector2(0, -speed);
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
    public Vector2 pos;
    public int power;
    public int timer = 10;
    //private float radius = 1.5f;
    
    public BombSim(Bomb b)
    {
        pos = new Vector2(b.transform.position.x, b.transform.position.z);
        power = b.power;
    }
    
    public BombSim(Vector2 position, int pow)
    {
        pos = position;
        power = pow;
    }

    public bool decreaseTimer()
    {
        timer -= 1;
        return (timer == 0);
    }
}

public class WallSim
{
    public Vector2 pos;
    public bool destructible;
    public float percentItem = 0.5f;
    
    public WallSim(Wall w)
    {
        pos = new Vector2(w.transform.position.x, w.transform.position.z);
        destructible = w.destructible;
        percentItem = w.percentItem;
    }
}

public class mapSimulation
{
    public List<PlayerSim> players = new List<PlayerSim>();
    public List<BombSim> bombs1 = new List<BombSim>();
    public List<BombSim> bombs2 = new List<BombSim>();
    public List<WallSim> walls = new List<WallSim>();
    public float collisionRadius = 1f;
    public float deltaTime = .1f;

    mapSimulation(Map map)
    {
        foreach (var player in map.players)
        {
            players.Add(new PlayerSim(player));
        }

        foreach (var wall in map.walls)
        {
            walls.Add(new WallSim(wall));
        }

        foreach (var bomb in map.bombs1)
        {
            bombs1.Add(new BombSim(bomb));
        }
        
        foreach (var bomb in map.bombs1)
        {
            bombs1.Add(new BombSim(bomb));
        }
    }

    public void updateMap(move m1, move m2)
    {
        PlayerSim ps1 = players.GetRange(0, 1).First();
        PlayerSim ps2 = players.GetRange(1, 1).First();

        foreach (var b in bombs1)
        {
            if (b.decreaseTimer())
            {
                explodeBomb(b);
                ps1.nbBomb++;
                bombs1.Remove(b);
            }
        }
        
        foreach (var b in bombs2)
        {
            if (b.decreaseTimer())
            {
                explodeBomb(b);
                ps2.nbBomb++;
                bombs1.Remove(b);
            }
        }
        
        switch (m1)
        {
            case move.UP:
                ps1.pos += ps1.MovePlayerUp() * deltaTime;
                break;
            case move.DOWN:
                ps1.pos += ps1.MovePlayerDown() * deltaTime;
                break;
            case move.RIGHT:
                ps1.pos += ps1.MovePlayerRight() * deltaTime;
                break;
            case move.LEFT:
                ps1.pos += ps1.MovePlayerLeft() * deltaTime;
                break;
            case move.BOMB:
                var bomb = ps1.PlantBomb();
                bombs1.Add(bomb);
                break;
        }

        switch (m2)
        {
            case move.UP:
                ps2.pos += ps2.MovePlayerUp() * deltaTime;
                break;
            case move.DOWN:
                ps2.pos += ps2.MovePlayerDown() * deltaTime;
                break;
            case move.RIGHT:
                ps2.pos += ps2.MovePlayerRight() * deltaTime;
                break;
            case move.LEFT:
                ps2.pos += ps2.MovePlayerLeft() * deltaTime;
                break;
            case move.BOMB:
                var bomb = ps2.PlantBomb();
                bombs2.Add(bomb);
                break;
        }
    }

    public bool checkPossibleMove(move m, int indexP)
    {
        PlayerSim ps = players.GetRange(indexP, 1).First();
        Vector2 dir;
        switch (m)
        {
            case move.UP:
                dir = ps.MovePlayerUp()*deltaTime;
                return (!collisionPlayerWalls(ps.pos));
            case move.DOWN:
                dir = ps.MovePlayerDown()*deltaTime;
                return (!collisionPlayerWalls(ps.pos));                break;
            case move.RIGHT:
                dir = ps.MovePlayerRight()*deltaTime;
                return (!collisionPlayerWalls(ps.pos));                break;
            case move.LEFT:
                dir = ps.MovePlayerLeft()*deltaTime;
                return (!collisionPlayerWalls(ps.pos));                break;
            case move.BOMB:
                return (ps.nbBomb > 0);
        }
        return false;
    }
    
    public bool collisionPlayerWalls(Vector2 checkPos)
    {
        foreach (var wall in walls)
        {
            if (wall.pos.x + collisionRadius > checkPos.x &&
                wall.pos.x - collisionRadius < checkPos.x &&
                wall.pos.y + collisionRadius > checkPos.y &&
                wall.pos.y - collisionRadius < checkPos.y)
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
            if (collisionBomb(wall.pos, pos))
            {
                if (wall.destructible)
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
                var newPos = new Vector2(pos.x, pos.y);
                newPos.Set(pos.x+delta, pos.y);
                if (collisionBomb(wall.pos, newPos))
                {
                    if (wall.destructible)
                    {
                        wallToRemove.Add(wall);
                        wall.destructible = false;
                    }
                }
                newPos.Set(pos.x, pos.y+delta);                
                if (collisionBomb(wall.pos, newPos))
                {
                    if (wall.destructible)
                    {
                        wallToRemove.Add(wall);
                        wall.destructible = false;
                    }
                }
                newPos.Set(pos.x, pos.y-delta);
                if (collisionBomb(wall.pos, newPos))
                {
                    if (wall.destructible)
                    {
                        wallToRemove.Add(wall);
                        wall.destructible = false;
                    }
                }
                newPos.Set(pos.x-delta, pos.y);
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
                var newPos = new Vector2(pos.x, pos.y);
                newPos.Set(pos.x+delta, pos.y);
                if (collisionBomb(player.pos, newPos))
                {
                    player.isAlive = false;
                }

                newPos.Set(pos.x, pos.y+ delta);
                if (collisionBomb(player.pos, newPos))
                {
                    player.isAlive = false;
                }

                newPos.Set(pos.x, pos.y- delta);
                if (collisionBomb(player.pos, newPos))
                {
                    player.isAlive = false;
                }

                newPos.Set(pos.x - delta, pos.y);
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
    
    public bool collisionBomb(Vector2 targetPos, Vector2 checkPos)
    {
        if (Mathf.Pow(targetPos.x - checkPos.x, 2) + Mathf.Pow(targetPos.x - checkPos.x, 2) < 1)
            return true;
        return false;
    }

    public int checkWinner()
    {
        //return 0 si pas de gagnant, 1 si j1 est mort, 2 si j2 est mort 
        if (!players.GetRange(0, 1).First().isAlive)
            return 1;
        if (!players.GetRange(2, 1).First().isAlive)
            return 2;
        return 0;
    }
 }

