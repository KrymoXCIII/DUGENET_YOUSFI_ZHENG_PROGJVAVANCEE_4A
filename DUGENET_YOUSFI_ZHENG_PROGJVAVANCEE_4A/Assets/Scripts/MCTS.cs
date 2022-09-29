using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MCTS 
{
    private List<NodeMCTS> listNode;
    private PlayerSim curPlayer;

    MCTS(Map map, PlayerBomberman pb)
    {
        mapSimulation mapS = new mapSimulation(map);
        NodeMCTS first = new NodeMCTS(mapS);
        listNode = new List<NodeMCTS>();
        listNode.Add(first);
        curPlayer = new PlayerSim(pb);
    }

    NodeMCTS selection()
    {
        float explo = Random.Range(0, 1);
        if (explo < .8)
        {
            int rand = Random.Range(0, listNode.Count);
            var node = listNode.GetRange(rand, 1).First();
            while (node.end)
            {
                rand = Random.Range(0, listNode.Count);
                node = listNode.GetRange(rand, 1).First();
            }

            return node;
        }
        else
        {
            NodeMCTS returnNode = null;
            float maxWinRate = 0;
            foreach (var node in listNode)
            {
                float winRate = node.nbWin / (float)node.nbMove;
                if (maxWinRate < winRate)
                {
                    returnNode = node;
                    maxWinRate = winRate;
                }
            }

            return returnNode;
        }
    }
    NodeMCTS expansion(NodeMCTS node)
    {
        var possibleMoves = node.currrentGameState.getPossibleMove();
        var rand = Random.Range(0, possibleMoves.Count);
        var newGameState = node.currrentGameState.updateMap(possibleMoves[rand].Item1,possibleMoves[rand].Item2);
        var newNode = new NodeMCTS(newGameState, node, possibleMoves[rand].Item1, possibleMoves[rand].Item2);
        listNode.Add(newNode);
        return newNode;
    }

    float simulation(NodeMCTS node, int itération, PlayerSim ps)
    {
        int nbWin = 0;
        for (int i = 0; i < itération; i++)
        {
            var curMap = node.currrentGameState;
            while (curMap.checkWinner() == null)
            {
                var listMove = node.currrentGameState.getPossibleMove();
                int rand = Random.Range(0, listMove.Count);
            }

            if (curMap.checkWinner() == ps)
                nbWin++;
        }
        return nbWin / itération;
    }

    void backPropagation(NodeMCTS node)
    {
        while (node.parent != null)
        {
            NodeMCTS parent = node.parent;
            parent.winRate += node.winRate;
            node = parent;
        }
    }
    
    void startState()
    {
        
    }

    void nextState()
    {
        
    }

    void possiblePlays()
    {
        
    }

    void winner()
    {
        
    }
    
}