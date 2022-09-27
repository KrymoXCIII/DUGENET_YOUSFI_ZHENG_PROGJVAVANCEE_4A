using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBomberman : MonoBehaviour
{
    float speed = 1;
    int nbBombes = 1;
    private int bombPower = 2;
    bool isAlive = true;
    
    private bool _bombReady;
    private float _countDown;
    private Rigidbody _rb;

    public Bomb playerBomb;
    
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

