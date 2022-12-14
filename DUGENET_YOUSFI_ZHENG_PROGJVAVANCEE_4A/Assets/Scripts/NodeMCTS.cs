using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeMCTS 
{
    public MapSimulation currrentGameState;

    public int nbWin = 0;
    public int nbMove = 0;

    
    public NodeMCTS parent;

    public move moveP1, moveP2;

    public bool end;

    public NodeMCTS(MapSimulation map, NodeMCTS p = null, move m1=move.NULL, move m2=move.NULL)
    {
        currrentGameState = map;
        parent = p;
        moveP1 = m1;
        moveP2 = m2;
        end = false;
    }
}
