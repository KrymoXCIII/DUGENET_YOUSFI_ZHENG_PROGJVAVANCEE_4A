using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MCTS 
{
    private List<NodeMCTS> listNode;
    private PlayerSim firstPlayer;
    private PlayerSim secondPlayer;

    MCTS(Map map, PlayerBomberman pb1, PlayerBomberman pb2)
    {
        mapSimulation mapS = new mapSimulation(map);
        NodeMCTS first = new NodeMCTS(mapS);
        listNode = new List<NodeMCTS>();
        listNode.Add(first);
        firstPlayer = new PlayerSim(pb1);
        secondPlayer = new PlayerSim(pb2);

    }

    List<NodeMCTS> computeMCTS(int nbTest, PlayerSim ps)
    {
        NodeMCTS root = listNode.First();

        for (int i = 0; i < nbTest; i++)
        {
            NodeMCTS selectedNode = selection();
            NodeMCTS newNode = expansion(selectedNode);
            
            simulation(newNode, nbTest);
            backPropagation(newNode);
        }

        List<NodeMCTS> res = new List<NodeMCTS>();
        foreach (var node in listNode)
        {
            if(node.parent == root)
                res.Add(node);
        }
        return res;
    }
    
    NodeMCTS selection()  // Selection
    {
        float explo = Random.Range(0, 1); // Random (0,1)
        if (explo < .8) //80%
        {
            int rand = Random.Range(0, listNode.Count); //  aléatoire un nombre entre le nombre de node
            var node = listNode.GetRange(rand, 1).First(); //Choisi le node aléatoire
            while (!node.end) // tant que le node n'est pas une feuille
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
                float winRate;
                if (node.nbMove == 0)
                    winRate = 0;
                else
                    winRate = node.nbWin / (float)node.nbMove;
                if (maxWinRate < winRate) // si le winrate est plus grand que le maxwinrate
                {
                    returnNode = node; //on selectionne le node qui a le maxwinrate 
                    maxWinRate = winRate; // on afecte le maxwinrate par celui-ci
                }
            }
            return returnNode; // return le NODE qui a le max win rate
        }
    }
    NodeMCTS expansion(NodeMCTS node) // Expension
    {
        var possibleMoves = node.currrentGameState.getPossibleMove(firstPlayer, secondPlayer); //liste de move possible
        var rand = Random.Range(0, possibleMoves.Count); //aléatoire un node dans la liste de move possible
        int n = listNode.Where(t =>
            t.parent == node && t.moveP1 == possibleMoves[rand].Item1 && t.moveP1 == possibleMoves[rand].Item1).Count();
        //passe dans la listeNode où n prend la valeur du nombre de fois on est passer sur le même node
        while (n > 0) // tant qu'on a déjà passer une fois
        {
            rand = Random.Range(0, possibleMoves.Count); // choisie une random dans la liste possible move
            n = listNode.Where(t =>
                t.parent == node && t.moveP1 == possibleMoves[rand].Item1 && t.moveP1 == possibleMoves[rand].Item1).Count();
            // passe dans la listeNode où n prend la valeur du nombre de fois on est passer sur le même node
        }

        var newGameState = node.currrentGameState.updateMap(possibleMoves[rand].Item1, possibleMoves[rand].Item2);
        var newNode = new NodeMCTS(newGameState, node, possibleMoves[rand].Item1, possibleMoves[rand].Item2);
        // créer une nouvelle NODE avec le prochaine étape de mouvement
        listNode.Add(newNode); //ajout dans la liste de node
        return newNode; //return le nouveau node
    }

    void simulation(NodeMCTS node, int itération)
    {
        int nbWin = 0;
        if (node.currrentGameState.checkWinner() != null)
        {
            node.end = true;
            node.nbMove = itération;
            if (node.currrentGameState.checkWinner() == firstPlayer)
            {
                node.nbWin = itération;
                return;
            }
            else
            {
                node.nbWin = 0;
                return;
            }
        }
        for (int i = 0; i < itération; i++)
        {
            var curMap = node.currrentGameState;
            while (curMap.checkWinner() == null)
            {
                var listMove = curMap.getPossibleMove(firstPlayer, secondPlayer);
                int rand = Random.Range(0, listMove.Count);
                curMap = curMap.updateMap(listMove[rand].Item1, listMove[rand].Item2);
            }
            if (curMap.checkWinner() == firstPlayer)
                nbWin++;
        }
    }

    void backPropagation(NodeMCTS node)
    {
        while (node.parent != null)
        {
            checkNodeEnd(node);
            NodeMCTS parent = node.parent;
            parent.nbWin += node.nbWin;
            parent.nbMove += node.nbMove;
            node = parent;
        }
    }

    void checkNodeEnd(NodeMCTS node)
    {
        if (node.end)
            return;
        var Tmoves = node.currrentGameState.getPossibleMove(firstPlayer, secondPlayer);
        var childList = listNode.Where(t => t.parent == node);
        if (childList.Count() < Tmoves.Count())
            return;
        bool isEnd = true;
        foreach (var childNode in childList)
        {
            if (!childNode.end)
            {
                isEnd = false;
            }
        }
        node.end = isEnd;
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