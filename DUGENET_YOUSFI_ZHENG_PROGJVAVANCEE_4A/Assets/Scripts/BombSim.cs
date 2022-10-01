using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombSim : MonoBehaviour
{
    public Vector3 pos;
    public int power;
    public float timer;
    public bool toDelete = false;
    public bool toCreate = false;
    //private float radius = 1.5f;
    
    public BombSim(BombSim b)
    {
        pos = b.pos;
        power = b.power;
        timer = b.timer;
        toDelete = b.toDelete;
    }
    
    public BombSim(Vector3 position, int pow)
    {
        pos = position;
        power = pow;
        timer = 120;
    }

    public bool decreaseTimer()
    {
        timer -= 1;
        return (timer == 0);
    }
}

