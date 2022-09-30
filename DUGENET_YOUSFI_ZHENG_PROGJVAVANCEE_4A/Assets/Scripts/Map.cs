using System;
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
    public ParticleSystem explosionanimation;

    private void Awake()
    {
        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 60;
    }

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
        
        switch (m)
        {
            case move.UP:
                player.transform.position += player.MovePlayerUp(player.gameObject,player.model);
                break;
            case move.DOWN:
                player.transform.position += player.MovePlayerDown(player.gameObject,player.model);
                break;
            case move.RIGHT:
                player.transform.position += player.MovePlayerRight(player.gameObject,player.model);
                break;
            case move.LEFT:
                player.transform.position += player.MovePlayerLeft(player.gameObject,player.model);
                break;
            case move.BOMB:
                if (player.nbBombes > 0)
                {
                    var bomb = player.PlantBomb();
                    //Debug.Log(bomb);
                    player.nbBombes--;
                    if (player.playerNb == 1)
                        bombs1.Add(bomb);
                    else if (player.playerNb == 2)
                        bombs2.Add(bomb);
                }
                break;
            case move.NOMOVE :
            case move.NULL :
                break;
        }

        List<Bomb> bombToRemove = new List<Bomb>();

        if (player.playerNb == 1)
        {
            foreach (var b in bombs1)
            {
                if (b.decreaseTimer())
                {
                    explode(b);
                    player.nbBombes++;
                    bombToRemove.Add(b);
                }
            }
        }
        else
        {
            foreach (var b in bombs2)
            {
                if (b.decreaseTimer())
                {
                    explode(b);
                    player.nbBombes++;
                    bombToRemove.Add(b);
                }
            }
        }
        foreach (var b in bombToRemove)
        {
            bombs1.Remove(b);
            Destroy(b.gameObject);
        }
        
        if (!player.isAlive)
        {
            ScoreBoard.SetActive(true);
            Time.timeScale = 0f;
        }   
        return this;
    }

    
    public void explode(Bomb b)
    {
        List<Wall> wallToRemove = new List<Wall>();
        var pos = b.transform.position;
        //createExplosion(pos);
        foreach (var wall in (walls))
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
        
        foreach(var player in players) 
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
                //createExplosion(pos);
                if (checkCollision(wall.transform.position, newPos))
                {
                    if (wall.destructible)
                    {
                        wallToRemove.Add(wall);
                        wall.destructible = false;
                    }
                }
                newPos.Set(pos.x, pos.y, pos.z+delta);
                //createExplosion(newPos);

                if (checkCollision(wall.transform.position, newPos))
                {
                    if (wall.destructible)
                    {
                        wallToRemove.Add(wall);
                        wall.destructible = false;
                    }
                }
                newPos.Set(pos.x, pos.y, pos.z-delta);
                //createExplosion(newPos);

                if (checkCollision(wall.transform.position, newPos))
                {
                    if (wall.destructible)
                    {
                        wallToRemove.Add(wall);
                        wall.destructible = false;
                    }
                }
                newPos.Set(pos.x-delta, pos.y, pos.z);
                //createExplosion(newPos);

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
        removeWalls(wallToRemove);
    }
    
    public bool checkCollision(Vector3 wallPos, Vector3 checkPos)
    {
        if (Mathf.Pow(wallPos.x - checkPos.x, 2) + Mathf.Pow(wallPos.z - checkPos.z, 2) < 1)
            return true;
        return false;
    }
}
