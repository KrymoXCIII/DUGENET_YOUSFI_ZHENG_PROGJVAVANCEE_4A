using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public int power = 2;
    private float timer;
    public Map map;
    public float radius = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        timer = 3;
        
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if(timer<0)
            explode();
        
    }
    
    public bool decreaseTimer()
    {
        timer -= 1;
        return (timer == 0);
    }
    
    public void setPower(int i)
    {
        power = i;
    }
    
    public void setMap(Map m)
    {
        map = m;
    }



    

    public bool checkCollision(Vector3 wallPos, Vector3 checkPos)
    {
        if (Mathf.Pow(wallPos.x - checkPos.x, 2) + Mathf.Pow(wallPos.z - checkPos.z, 2) < 1)
            return true;
        return false;
    }
}
