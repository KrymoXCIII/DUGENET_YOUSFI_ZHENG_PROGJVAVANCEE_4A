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
    
    private bool _bombReady;
    private float _countDown;
    
    public Map map;

    public Bomb playerBomb;
    public Transform model;


    [SerializeField]public GameObject ScoreBoard;
    public float collisionRadius =1f;

    
    // Start is called before the first frame update
    void Start()
    {
        _bombReady = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive == false)
        {
            Time.timeScale = 0f;
            ScoreBoard.SetActive(true);
        }
    }

    public Bomb PlantBomb()
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
        if (Time.time - _countDown>=3)
        {

            _bombReady = true;

        }
        return playerBomb;
    }

    public Vector3 MovePlayerUp(GameObject obj, Transform model)
    {
        model.transform.rotation = Quaternion.Euler(0, 90, 0);

            Vector3 dir = (new Vector3(0, 0, speed)* Time.deltaTime);
            if (!collisionPlayer(dir))
                return dir;
            else
                return new Vector3(0, 0, 0);
            
            

    }
    public Vector3 MovePlayerDown(GameObject obj, Transform model)
    {
        model.transform.rotation = Quaternion.Euler(0,-90,0);
   
            var dir = (new Vector3(0, 0, -speed)* Time.deltaTime);
            if (!collisionPlayer(dir))
                return dir;
            else
                return new Vector3(0, 0, 0);

    }
    public Vector3 MovePlayerRight(GameObject obj, Transform model)
    {
        model.transform.rotation = Quaternion.Euler(0, 180, 0);
        var dir = (new Vector3(speed, 0, 0)* Time.deltaTime);
        if (!collisionPlayer(dir))
            return dir;
        else
            return new Vector3(0, 0, 0);

    }
    public Vector3 MovePlayerLeft(GameObject obj, Transform model)
    {
        model.transform.rotation = Quaternion.Euler(0, 0, 0);
 
            var dir = (new Vector3(-speed, 0, 0)* Time.deltaTime);
            if (!collisionPlayer(dir))
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

