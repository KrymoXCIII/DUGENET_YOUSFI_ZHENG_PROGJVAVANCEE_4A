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

    NodeMCTS selection()  // Selection
    {
        float explo = Random.Range(0, 1); // Random (0,1)
        if (explo < .8) //80%
        {
            int rand = Random.Range(0, listNode.Count); //  aléatoire un nombre entre le nombre de node
            var node = listNode.GetRange(rand, 1).First(); //Choisi le node aléatoire
            while (node.end) // tant que le node n'est pas une feuille
            {
                rand = Random.Range(0, listNode.Count); //aléatoire un nombre entre le nombre de node
                node = listNode.GetRange(rand, 1).First(); //Choisi le node aléatoire
            }

            return node; // return la feuille de l'arbre
        }
        else //20%
        {
            NodeMCTS returnNode = null; // créer un NODE null
            float maxWinRate = 0; //initialise un valeur de max win rate
            foreach (var node in listNode) //pour chaque node dans la liste
            {
                float winRate = node.nbWin / (float)node.nbMove; // initialise le winrate par le nombre de win sur le nombe total de simulation
                if (maxWinRate < winRate) // si le winrate est plus grand que le maxwinrate
                {
                    returnNode = node; //on selectionne le node qui a le maxwinrate 
                    maxWinRate = winRate; // on afecte le maxwinrate par celui-ci
                }
            }

            return returnNode; // return le NODE qui a le max win rate
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