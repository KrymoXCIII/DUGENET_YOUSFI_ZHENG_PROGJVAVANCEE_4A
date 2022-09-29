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
    [SerializeField]public GameObject ScoreBoard;
    
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
                explode(b);
                player.nbBombes++;
                bombs1.Remove(b);
            }
        }
        
        foreach (var b in bombs2)
        {
            if (b.decreaseTimer())
            {
                explode(b);
                player.nbBombes++;
                bombs2.Remove(b);
            }
        }

        switch (m)
        {
            case move.UP:
                player.transform.position += player.MovePlayerUp(player.gameObject,player.model) * deltaTime;
                break;
            case move.DOWN:
                player.transform.position += player.MovePlayerDown(player.gameObject,player.model) * deltaTime;
                break;
            case move.RIGHT:
                player.transform.position += player.MovePlayerRight(player.gameObject,player.model) * deltaTime;
                break;
            case move.LEFT:
                player.transform.position += player.MovePlayerLeft(player.gameObject,player.model) * deltaTime;
                break;
            case move.BOMB:
                var bomb = player.PlantBomb();
                bombs1.Add(bomb);
                break;
        }

        if (player.isAlive == false)
        {
            Time.timeScale = 0f;
            ScoreBoard.SetActive(true);
        }
        
        return this;
    }

    
    public void explode(Bomb b)
    {
        List<Wall> wallToRemove = new List<Wall>();
        var pos = transform.position;
        foreach (var wall in (this.walls))
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
        
        foreach(var player in this.players) 
        {
            if (checkCollision(player.transform.position, pos))
            {
                player.isAlive = false;

            }
        }

        for (int i = 1; i < b.power; i++)
        {
            var delta = +i * b.radius;
            foreach (var wall in (this.walls))
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
            foreach(var player in this.players) 
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

        this.removeWalls(wallToRemove);

        Destroy(gameObject);
    }
    
    public bool checkCollision(Vector3 wallPos, Vector3 checkPos)
    {
        if (Mathf.Pow(wallPos.x - checkPos.x, 2) + Mathf.Pow(wallPos.z - checkPos.z, 2) < 1)
            return true;
        return false;
    }
}
