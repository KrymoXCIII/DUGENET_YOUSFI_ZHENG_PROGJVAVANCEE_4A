using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public int power = 2;
    private float timer;
    public float radius = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        timer = 4*60;
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
}
