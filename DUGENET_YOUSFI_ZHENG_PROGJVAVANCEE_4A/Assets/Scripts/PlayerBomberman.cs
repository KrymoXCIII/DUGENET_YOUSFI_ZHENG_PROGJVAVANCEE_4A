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
    
    public Map map;

    public Bomb playerBomb;
    public Transform model;

    public int playerNb;
    
    public float collisionRadius =1f;

    public Bomb PlantBomb()
    {
        playerBomb.transform.position = new Vector3(transform.position.x, transform.position.y,transform.position.z);
        var bomb = Instantiate(playerBomb).gameObject.GetComponent<Bomb>();
        bomb.setPower(bombPower);
        return bomb;
    }

    public Vector3 MovePlayerUp(GameObject obj, Transform model)
    {
        model.transform.rotation = Quaternion.Euler(0, 90, 0);

<<<<<<< HEAD
            Vector3 dir = (new Vector3(0, 0, speed)* Time.deltaTime);
            if (!collisionPlayer(transform.position+dir))
                return dir;
            else
                return new Vector3(0, 0, 0);
=======
            Vector3 dir = (new Vector3(0, 0, speed)* Time.deltaTime)+transform.position;
            if (!collisionPlayer(dir))
                return dir;
            else
                return new Vector3(0, 0, 0);

>>>>>>> parent of 97f5032 (fix movement)
    }
    public Vector3 MovePlayerDown(GameObject obj, Transform model)
    {
        model.transform.rotation = Quaternion.Euler(0,-90,0);
   
<<<<<<< HEAD
            var dir = (new Vector3(0, 0, -speed)* Time.deltaTime);
            if (!collisionPlayer(transform.position+dir))
=======
            var dir = (new Vector3(0, 0, -speed)* Time.deltaTime)+transform.position;
            if (!collisionPlayer(dir))
>>>>>>> parent of 97f5032 (fix movement)
                return dir;
            else
                return new Vector3(0, 0, 0);

    }
    public Vector3 MovePlayerRight(GameObject obj, Transform model)
    {
        model.transform.rotation = Quaternion.Euler(0, 180, 0);
<<<<<<< HEAD
        var dir = (new Vector3(speed, 0, 0)* Time.deltaTime);
        if (!collisionPlayer(transform.position+dir))
=======
        var dir = (new Vector3(speed, 0, 0)* Time.deltaTime)+transform.position;
        if (!collisionPlayer(dir))
>>>>>>> parent of 97f5032 (fix movement)
            return dir;
        else
            return new Vector3(0, 0, 0);

    }
    public Vector3 MovePlayerLeft(GameObject obj, Transform model)
    {
        model.transform.rotation = Quaternion.Euler(0, 0, 0);
 
<<<<<<< HEAD
            var dir = (new Vector3(-speed, 0, 0)* Time.deltaTime);
            if (!collisionPlayer(transform.position+dir))
=======
            var dir = (new Vector3(-speed, 0, 0)* Time.deltaTime)+transform.position;
            if (!collisionPlayer(dir))
>>>>>>> parent of 97f5032 (fix movement)
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
        }
        return false;
    }
}

