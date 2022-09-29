using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBomberman : MonoBehaviour
{
    public float speed = 5;
    public int nbBombes = 1;
    public int bombPower = 2;
    public bool isAlive = true;
    
    private bool _bombReady = true;
    
    public Map map;

    public Bomb playerBomb;
    public Transform model;

    public int playerNb;
    
    [SerializeField]public GameObject ScoreBoard;
    public float collisionRadius =1f;

    public Bomb PlantBomb()
    {
        playerBomb.transform.position = new Vector3(transform.position.x, transform.position.y,transform.position.z);
        var bomb = Instantiate(playerBomb).gameObject.GetComponent<Bomb>();
        bomb.setPower(bombPower);
        //bomb.setMap(map);
        return bomb;
    }

    public Vector3 MovePlayerUp(GameObject obj, Transform model)
    {
        model.transform.rotation = Quaternion.Euler(0, 90, 0);

            Vector3 dir = (new Vector3(0, 0, speed)* Time.deltaTime);
            if (!collisionPlayer(transform.position+dir))
                return dir;
            else
                return new Vector3(0, 0, 0);
    }
    public Vector3 MovePlayerDown(GameObject obj, Transform model)
    {
        model.transform.rotation = Quaternion.Euler(0,-90,0);
   
            var dir = (new Vector3(0, 0, -speed)* Time.deltaTime);
            if (!collisionPlayer(transform.position+dir))
                return dir;
            else
                return new Vector3(0, 0, 0);

    }
    public Vector3 MovePlayerRight(GameObject obj, Transform model)
    {
        model.transform.rotation = Quaternion.Euler(0, 180, 0);
        var dir = (new Vector3(speed, 0, 0)* Time.deltaTime);
        if (!collisionPlayer(transform.position+dir))
            return dir;
        else
            return new Vector3(0, 0, 0);

    }
    public Vector3 MovePlayerLeft(GameObject obj, Transform model)
    {
        model.transform.rotation = Quaternion.Euler(0, 0, 0);
 
            var dir = (new Vector3(-speed, 0, 0)* Time.deltaTime);
            if (!collisionPlayer(transform.position+dir))
                return dir;
            else
                return new Vector3(0, 0, 0);

    }
    
    
    
    public bool collisionPlayer(Vector3 checkPos)
    {
        foreach (var wall in map.walls)
        {
            if (wall.transform.position.x + collisionRadius > checkPos.x &&
                wall.transform.position.x - collisionRadius < checkPos.x &&
                wall.transform.position.z + collisionRadius > checkPos.z &&
                wall.transform.position.z - collisionRadius < checkPos.z)
                return true;
            /*
            if (Mathf.Pow(wall.transform.position.x - checkPos.x, 2) + Mathf.Pow(wall.transform.position.z - checkPos.z, 2) < collisionRadius)
                return true;*/
        }
        return false;
    }
}

