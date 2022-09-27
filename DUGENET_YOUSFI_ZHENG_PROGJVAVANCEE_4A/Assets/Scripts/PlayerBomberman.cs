using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBomberman : MonoBehaviour
{
    float speed = 50;
    private float maxSpeed = 5;
    int nbBombes = 1;
    private int bombPower = 1;
    bool isAlive = true;
    
    private bool _bombReady;
    private float _countDown;
   

    // Start is called before the first frame update
    void Start()
    {
        _bombReady = true;
        
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlantBomb(GameObject bomb, GameObject player)
    {
        
        if (_bombReady)
        {
            _countDown = Time.time;

            bomb.transform.position = new Vector3(player.transform.position.x, player.transform.position.y,player.transform.position.z);
            Instantiate(bomb);
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
        if (rb.velocity.magnitude <= maxSpeed)
        {
            if (Input.GetKey(KeyCode.Z))
            {
                rb.AddForce(transform.right * speed);
            }

            if (Input.GetKey(KeyCode.S))
            {
                rb.AddForce(transform.right * -speed);
            }

            if (Input.GetKey(KeyCode.Q))
            {
                rb.AddForce(transform.forward * speed);
            }

            if (Input.GetKey(KeyCode.D))
            {
                rb.AddForce(transform.forward * -speed);
            }

        }
       
       
        
        


    }
}

