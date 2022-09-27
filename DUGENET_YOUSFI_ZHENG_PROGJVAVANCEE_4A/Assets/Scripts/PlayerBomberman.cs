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
        obj.transform.rotation = Quaternion.Euler(0,90,0);
        obj.transform.Translate(new Vector3(-speed,0,0) * Time.deltaTime);
    }
    public void MovePlayerDown(GameObject obj)
    {    
        obj.transform.rotation = Quaternion.Euler(0,-90,0);
        obj.transform.Translate(new Vector3(-speed,0,0) * Time.deltaTime);
    }
    public void MovePlayerRight(GameObject obj)
    {
        obj.transform.rotation = Quaternion.Euler(0,180,0);
        obj.transform.Translate(new Vector3(-speed,0,0) * Time.deltaTime);
    }
    public void MovePlayerLeft(GameObject obj)
    {    
        obj.transform.rotation = new Quaternion(0,0,0,0);
        obj.transform.Translate(new Vector3(-speed,0,0) * Time.deltaTime);
    }
    
    public bool collisionPlayer(Vector3 checkPos)
    {
        foreach (var wall in map.walls)
        {
            if (Mathf.Pow(wall.transform.position.x - checkPos.x, 2) + Mathf.Pow(wall.transform.position.z - checkPos.z, 2) < .1)
                return true;
        }
        return false;
    }
}

