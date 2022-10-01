using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSim : MonoBehaviour {

    void Start()
    {
        pos = transform.position;
    }
    public Vector3 pos;
    public bool destructible;
    public float percentItem = 0.5f;

    public WallSim(WallSim w)
    {
        pos = w.pos;
        destructible = w.destructible;
    }
}