using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBomberman : MonoBehaviour
{
    float speed = 50;
    private float maxSpeed = 5;
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

    public void MovePlayerUp(Rigidbody rb)
    {    if (rb.velocity.magnitude <= maxSpeed)
        rb.AddForce(transform.right * speed);

    }
    public void MovePlayerDown(Rigidbody rb)
    {    if (rb.velocity.magnitude <= maxSpeed)
        rb.AddForce(-transform.right * speed);

    }
    public void MovePlayerRight(Rigidbody rb)
    {    if (rb.velocity.magnitude <= maxSpeed)
        rb.AddForce(transform.forward * speed);

    }
    public void MovePlayerLeft(Rigidbody rb)
    {    if (rb.velocity.magnitude <= maxSpeed)
        rb.AddForce(-transform.forward * speed);

    }

}

