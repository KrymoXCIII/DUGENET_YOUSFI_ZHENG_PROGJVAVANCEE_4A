using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private int power = 2;
    private float timer;
    public Map map;
    private float radius = 1.5f;

    private string Agent = "";
    // Start is called before the first frame update
    void Start()
    {
        timer = 3;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if(timer<0)
            explode();
    }
    
    public void setPower(int i)
    {
        power = i;
    }
    
    public void setMap(Map m)
    {
        map = m;
    }

    public void setAgentBomb(string j)
    {
        Agent = j;
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

    public bool checkCollision(Vector3 wallPos, Vector3 checkPos)
    {
        if (Mathf.Pow(wallPos.x - checkPos.x, 2) + Mathf.Pow(wallPos.z - checkPos.z, 2) < 1)
            return true;
        return false;
    }
}
