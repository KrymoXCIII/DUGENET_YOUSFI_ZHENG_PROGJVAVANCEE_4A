using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeMCTS 
{
    public mapSimulation currrentGameState;

    public float winRate;

    public NodeMCTS parent;

    public move moveP1, moveP2;

    public bool end;

    public NodeMCTS(mapSimulation map, NodeMCTS p = null, move m1=move.NULL, move m2=move.NULL)
    {
        currrentGameState = map;
        parent = p;
        winRate = 0;
        moveP1 = m1;
        moveP2 = m2;
        end = false;
    }
}
