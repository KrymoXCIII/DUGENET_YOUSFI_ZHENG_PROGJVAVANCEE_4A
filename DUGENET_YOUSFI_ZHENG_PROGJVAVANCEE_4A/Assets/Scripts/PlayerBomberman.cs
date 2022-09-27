using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBomberman : MonoBehaviour
{
    float speed = 10;
    int nbBombes = 1;
    private int bombPower = 2;
    bool isAlive = true;
    
    private bool _bombReady;
    private float _countDown;
   

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

    public void PlantBomb(GameObject player, Map map)
    {
        
        if (_bombReady)
        {
            _countDown = Time.time;

            playerBomb.transform.position = new Vector3(player.transform.position.x, player.transform.position.y,player.transform.position.z);
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

    public void MovePlayer(Rigidbody rb)
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            rb.velocity = new Vector3(0, 0, speed);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            rb.velocity = new Vector3(0, 0, -speed);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            rb.velocity = new Vector3(-speed, 0, 0);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            rb.velocity = new Vector3(speed, 0, 0);
        }
        
       
        
        


    }
}

