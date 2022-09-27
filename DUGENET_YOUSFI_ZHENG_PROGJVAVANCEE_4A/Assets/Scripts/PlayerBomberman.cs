using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBomberman : MonoBehaviour
{
    float speed = 1;
    int nbBombes = 1;
    private int bombPower = 1;
    bool isAlive = true;
    
    private bool _bombReady;
    private float _countDown;
    private Rigidbody _rb;

    // Start is called before the first frame update
    void Start()
    {
        _bombReady = true;
        _rb = GetComponent<Rigidbody>();
        
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

    public void MovePlayer(GameObject player)
    {
        if (Input.GetKey(KeyCode.Z))
        {
            _rb.transform.Translate(new Vector3(-speed, 0, 0) * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.S))
        {
            _rb.transform.Translate(new Vector3(speed, 0, 0) * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            _rb.transform.Translate(new Vector3(0, 0, -speed) * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D))
        {
            _rb.transform.Translate(new Vector3(-0, 0, speed) * Time.deltaTime);
        }

        
    }
}

