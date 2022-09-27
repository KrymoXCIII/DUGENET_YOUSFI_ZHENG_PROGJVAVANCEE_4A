using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBomberman : MonoBehaviour
{
    float speed = 5;
    
    int nbBombes = 1;
    private int bombPower = 2;
    bool isAlive = true;
    
    private bool _bombReady;
    private float _countDown;

    public Map map;

    public Bomb playerBomb;

    public Transform model;
    
    // Start is called before the first frame update
    void Start()
    {
        _bombReady = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlantBomb(Map map)
    {
        if (_bombReady)
        {
            _countDown = Time.time;

            playerBomb.transform.position = new Vector3(transform.position.x, transform.position.y,transform.position.z);
            var bomb = Instantiate(playerBomb).gameObject.GetComponent<Bomb>();
            bomb.setPower(bombPower);
            bomb.setMap(map);
            _bombReady = false;
        }
            
        // Compte à rebours
        if (Time.time - _countDown >= 5)
        {

            _bombReady = true;

        }
    }

    public void MovePlayerUp(GameObject obj)
    {    
        model.transform.rotation = Quaternion.Euler(0,90,0);
        var dir = new Vector3(-speed, 0, 0) * Time.deltaTime;
        if(!collisionPlayer(obj.transform.position+dir))
            obj.transform.Translate(dir);
    }
    public void MovePlayerDown(GameObject obj)
    {    
        model.transform.rotation = Quaternion.Euler(0,-90,0);
        var dir = new Vector3(speed,0,0) * Time.deltaTime;
        if(!collisionPlayer(obj.transform.position+dir))
            obj.transform.Translate(dir);
    }
    public void MovePlayerRight(GameObject obj)
    {
        model.transform.rotation = Quaternion.Euler(0,180,0);
        var dir = new Vector3(0, 0, speed) * Time.deltaTime;
        if(!collisionPlayer(obj.transform.position+dir))
            obj.transform.Translate(dir);
    }
    public void MovePlayerLeft(GameObject obj)
    {    
        model.transform.rotation = new Quaternion(0,0,0,0);
        var dir = new Vector3(0, 0, -speed) * Time.deltaTime;
        if(!collisionPlayer(obj.transform.position+dir))
            obj.transform.Translate(dir);
    }
    
    public bool collisionPlayer(Vector3 checkPos)
    {
        foreach (var wall in map.walls)
        {
            if (Mathf.Pow(wall.transform.position.x - checkPos.x, 2) + Mathf.Pow(wall.transform.position.z - checkPos.z, 2) < 1)
                return true;
        }
        return false;
    }
}

