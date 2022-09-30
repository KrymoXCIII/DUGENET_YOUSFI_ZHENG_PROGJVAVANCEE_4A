using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class MCTS
{
    private List<NodeMCTS> listNode;
    private PlayerSim firstPlayer;
    private PlayerSim secondPlayer;

    public MCTS(Map map, PlayerBomberman pb1)
    {
        mapSimulation mapS = new mapSimulation(map, pb1);
        NodeMCTS first = new NodeMCTS(mapS);
        listNode = new List<NodeMCTS>();
        listNode.Add(first);
        firstPlayer = new PlayerSim(pb1);
        foreach (var play in map.players)
        {
            if (play != pb1)
                secondPlayer = new PlayerSim(play);
        }
    }

    public List<NodeMCTS> computeMCTS(int nbTest) //do the MCTS
    {
        NodeMCTS root = listNode.First();

        for (int i = 0; i < nbTest; i++)
        {
            NodeMCTS selectedNode = selection();
            if (selectedNode == null)
                break;
            NodeMCTS newNode = expansion(selectedNode);
            simulation(newNode, 10);
            backPropagation(newNode);
        }

        List<NodeMCTS> res = new List<NodeMCTS>();
        foreach (var node in listNode)
        {
            if (node.parent == root)
                res.Add(node);
        }
        return res;
    }

    NodeMCTS selection() // Selection
    {
        float explo = Random.Range(0.0f, 1.0f); // Random (0,1)
        if (explo < .8) //80% exploration
        {
            int rand = Random.Range(0, listNode.Count); //  aléatoire un nombre entre le nombre de node
            var node = listNode.GetRange(rand, 1).First(); //Choisi le node aléatoire
            var childCount = listNode.Where(t => t.parent == node).Count();
            var moveCount = node.currrentGameState.getPossibleMove(firstPlayer, secondPlayer).Count;

            List<NodeMCTS> copyList = new List<NodeMCTS>(listNode.Count);
            foreach (var n in listNode)
            {
                copyList.Add(n);
            }
            while (node.end && copyList.Count > 0 && childCount == moveCount) // tant que le node n'est pas une feuille
            {
                rand = Random.Range(0, copyList.Count); //aléatoire un nombre entre le nombre de node
                node = listNode.GetRange(rand, 1).First(); //Choisi le node aléatoire
                childCount = listNode.Where(t => t.parent == node).Count();
                moveCount = node.currrentGameState.getPossibleMove(firstPlayer, secondPlayer).Count;

                if (node.end || childCount == moveCount)
                    copyList.Remove(node);
            }

            if (copyList.Count == 0)
                return null;
            return node; // return la feuille de l'arbre
        }
        else //20% explotation
        {
            NodeMCTS returnNode = null; // créer un NODE null
            float maxWinRate = 0; //initialise un valeur de max win rate
            foreach (var node in listNode) //pour chaque node dans la liste
            {
                float winRate;
                if (node.nbMove == 0)
                    winRate = 0;
                else
                    winRate = node.nbWin / (float) node.nbMove;
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
        var nodeChild = listNode.Where(t=>t.parent == node);
        foreach (var child in nodeChild)
        {
            possibleMoves.Remove(new Tuple<move, move>(child.moveP1,child.moveP2));
        }

        if (possibleMoves.Count == 0)
            return null;

        int rand = Random.Range(0, possibleMoves.Count);

        var newGameState = node.currrentGameState.updateMap(possibleMoves[rand].Item1, possibleMoves[rand].Item2);
        var newNode = new NodeMCTS(newGameState, node, possibleMoves[rand].Item1, possibleMoves[rand].Item2);
        // créer une nouvelle NODE avec le prochaine étape de mouvement
        listNode.Add(newNode); //ajout dans la liste de node
        return newNode; //return le nouveau node
    }


    void simulation(NodeMCTS node, int itération) // Simulation itération = nombre de test 
    {
        int nbWin = 0; // initialise le nombre de win
        for (int i = 0; i < itération; i++) //pour chaque test 
        {
            int step = 200;
            var curMap = node.currrentGameState; // Temporaire Map
            while (curMap.checkWinner() == null && step > 0) //tant que il y a pas de victoire
            {
                step--;
                var listMove =
                    curMap.getPossibleMove(firstPlayer, secondPlayer); // liste de move posible
                int rand = Random.Range(0, listMove.Count); //random dans la liste de move possible
                curMap = curMap.updateMap(listMove[rand].Item1,
                    listMove[rand].Item2); //Map avec un aléatoire move possible
            }
            if (curMap.checkWinner() == firstPlayer || step == 0) // si il y a un winner
                nbWin++; // nb de win ++;
        }
        node.nbWin = nbWin;
        node.nbMove = itération;
        //Debug.Log(node.nbWin + " / " + node.nbMove + " step: " + step);
    }

    void backPropagation(NodeMCTS node) // backPropagation
    {
        while (node.parent != null) // tant que C'est pas le node parent
        {
            checkNodeEnd(node); // check tous les fils sont end et il a tous ces fils
            NodeMCTS parent = node.parent; // créer un parent
            parent.nbWin += node.nbWin; // incrémente le nombre de win
            parent.nbMove += node.nbMove; // incrémente le nombre de sumulation
            node = parent;
        }
    }
    
    void checkNodeEnd(NodeMCTS node) // check tous les fils sont end et il a tous ces fils
    {
        if (node.end) // si le node est le feuille alors en return
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
}