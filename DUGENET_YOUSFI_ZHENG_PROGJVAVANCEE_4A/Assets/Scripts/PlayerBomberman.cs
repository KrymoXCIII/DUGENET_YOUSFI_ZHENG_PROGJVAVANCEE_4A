using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBomberman : MonoBehaviour
{
    float speed = 5;
    
    int nbBombes = 1;
    private int bombPower = 2;
    public bool isAlive = true;
    
    private bool _bombReady;
    private float _countDown;

    public Map map;

    public Bomb playerBomb;

    public float collisionRadius =.1f;

    
    // Start is called before the first frame update
    void Start()
    {
        _bombReady = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlantBomb()
    {
        if (_bombReady)
        {
            _countDown = Time.time;

            playerBomb.transform.position = new Vector3(transform.position.x, transform.position.y,transform.position.z);
            var bomb = Instantiate(playerBomb).gameObject.GetComponent<Bomb>();
            bomb.setPower(bombPower);
            bomb.setMap(map);
            _bombReady = false;
            nbBombes--;
        }
            
        // Compte à rebours
        if (nbBombes>0)
        {

            _bombReady = true;

        }
    }

    public void MovePlayerUp(GameObject obj, Transform model)
    {
        model.transform.rotation = Quaternion.Euler(0, 90, 0);
        if (gameObject.name == "EnemyBomberman")
        {
            
            var dir = new Vector3(speed, 0, 0) * Time.deltaTime;
            if(!collisionPlayer(obj.transform.position+dir))
                obj.transform.Translate(dir);
        }
        else if (gameObject.name == "CharacterBomberman")
        {
           
            var dir = new Vector3(-speed, 0, 0) * Time.deltaTime;
            if(!collisionPlayer(obj.transform.position+dir))
                obj.transform.Translate(dir);
        }

        
        
    }
    public void MovePlayerDown(GameObject obj, Transform model)
    {
        model.transform.rotation = Quaternion.Euler(0,-90,0);
        if (gameObject.name == "EnemyBomberman")
        {
            var dir = new Vector3(-speed,0,0) * Time.deltaTime;
            if(!collisionPlayer(obj.transform.position+dir))
                obj.transform.Translate(dir);
        }
        else if (gameObject.name == "CharacterBomberman")
        {
            var dir = new Vector3(speed,0,0) * Time.deltaTime;
            if(!collisionPlayer(obj.transform.position+dir))
                obj.transform.Translate(dir);
        }
    }
    public void MovePlayerRight(GameObject obj, Transform model)
    {
        model.transform.rotation = Quaternion.Euler(0, 180, 0);
        if (gameObject.name == "EnemyBomberman")
        {
            var dir = new Vector3(0, 0, -speed) * Time.deltaTime;
            if(!collisionPlayer(obj.transform.position+dir))
                obj.transform.Translate(dir);
        }
        else if (gameObject.name == "CharacterBomberman")
        {
            var dir = new Vector3(0, 0, speed) * Time.deltaTime;
            if(!collisionPlayer(obj.transform.position+dir))
                obj.transform.Translate(dir);
        }
    }
    public void MovePlayerLeft(GameObject obj, Transform model)
    {
        model.transform.rotation = Quaternion.Euler(0, 0, 0);
        if (gameObject.name == "EnemyBomberman")
        {
            var dir = new Vector3(0, 0, speed) * Time.deltaTime;
            if(!collisionPlayer(obj.transform.position+dir))
                obj.transform.Translate(dir);
        }
        else if (gameObject.name == "CharacterBomberman")
        {
            var dir = new Vector3(0, 0, -speed) * Time.deltaTime;
            if(!collisionPlayer(obj.transform.position+dir))
                obj.transform.Translate(dir);
        }
    }
    
    public bool collisionPlayer(Vector3 checkPos)
    {
        foreach (var wall in map.walls)
        {
            if (wall.transform.position.x + collisionRadius > checkPos.x - collisionRadius &&
                wall.transform.position.x - collisionRadius < checkPos.x + collisionRadius &&
                wall.transform.position.z + collisionRadius > checkPos.z - collisionRadius &&
                wall.transform.position.z - collisionRadius < checkPos.z + collisionRadius)
                return true;
        }
        return false;
    }
}

