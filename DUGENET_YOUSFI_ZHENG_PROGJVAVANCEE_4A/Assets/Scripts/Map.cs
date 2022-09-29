using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public List<PlayerBomberman> players = new List<PlayerBomberman>();
    public List<Bomb> bombs1 = new List<Bomb>();
    public List<Bomb> bombs2 = new List<Bomb>();
    public List<Wall> walls = new List<Wall>();
    public float collisionRadius = 1f;
    public float deltaTime = .1f;

    

    public void removeWalls(List<Wall> list)
    {
        foreach (var wall in list)
        {
            walls.Remove(wall);
            Destroy(wall.gameObject);
        }
    }

    public Map updateMap(PlayerBomberman player,move m)
    {
        

        foreach (var b in bombs1)
        {
            if (b.decreaseTimer())
            {
                bombs1.(b);
                player.nbBomb++;
                bombs1.Remove(b);
            }
        }
        
        foreach (var b in bombs2)
        {
            if (b.decreaseTimer())
            {
                explodeBomb(b);
                ps1.nbBomb++;
                bombs2.Remove(b);
            }
        }
        
        
        
        switch (m)
        {
            case move.UP:
                player.transform.position += new Vector3(player.MovePlayerUp(player.gameObject, player.model) * deltaTime);
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

        
        
        return this;
    }

    
    public void explode()
    {
        List<Wall> wallToRemove = new List<Wall>();
        var pos = transform.position;
        foreach (var wall in (map.walls))
        {
            if (checkCollision(wall.transform.position, pos))
            {
                if (wall.destructible)
                {
                    wallToRemove.Add(wall);
                    wall.destructible = false;
                }
            }
        }
        
        foreach(var player in map.players) 
        {
            if (checkCollision(player.transform.position, pos))
            {
                player.isAlive = false;

            }
        }

        for (int i = 1; i < power; i++)
        {
            var delta = +i * radius;
            foreach (var wall in (map.walls))
            {
                var newPos = new Vector3(pos.x, pos.y, pos.z);
                newPos.Set(pos.x+delta, pos.y, pos.z);
                if (checkCollision(wall.transform.position, newPos))
                {
                    if (wall.destructible)
                    {
                        wallToRemove.Add(wall);
                        wall.destructible = false;
                    }
                }
                newPos.Set(pos.x, pos.y, pos.z+delta);
                if (checkCollision(wall.transform.position, newPos))
                {
                    if (wall.destructible)
                    {
                        wallToRemove.Add(wall);
                        wall.destructible = false;
                    }
                }
                newPos.Set(pos.x, pos.y, pos.z-delta);
                if (checkCollision(wall.transform.position, newPos))
                {
                    if (wall.destructible)
                    {
                        wallToRemove.Add(wall);
                        wall.destructible = false;
                    }
                }
                newPos.Set(pos.x-delta, pos.y, pos.z);
                if (checkCollision(wall.transform.position, newPos))
                {
                    if (wall.destructible)
                    {
                        wallToRemove.Add(wall);
                        wall.destructible = false;
                    }
                }
            }
            foreach(var player in map.players) 
            {
                var newPos = new Vector3(pos.x, pos.y, pos.z);
                newPos.Set(pos.x+delta, pos.y, pos.z);
                if (checkCollision(player.transform.position, newPos))
                {
                    player.isAlive = false;

                }
                newPos.Set(pos.x, pos.y, pos.z+delta);
                if (checkCollision(player.transform.position, newPos))
                {
                    player.isAlive = false;

                }
                newPos.Set(pos.x, pos.y, pos.z-delta);
                if (checkCollision(player.transform.position, newPos))
                {
                    player.isAlive = false;

                }
                newPos.Set(pos.x-delta, pos.y, pos.z);
                if (checkCollision(player.transform.position, newPos))
                {
                    player.isAlive = false;

                }
            }
        }

        map.removeWalls(wallToRemove);

        Destroy(gameObject);
    }
}
