using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MCTS 
{
    private List<NodeMCTS> listNode;
    private PlayerSim firstPlayer;
    private PlayerSim secondPlayer;

    public MCTS(Map map, PlayerBomberman pb1)
    {
        mapSimulation mapS = new mapSimulation(map);
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

    List<NodeMCTS> computeMCTS(int nbTest)
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


    void simulation(NodeMCTS node, int itération) // Simulation itération = nombre de test 
    {
        int nbWin = 0; // initialise le nombre de win
        for (int i = 0; i < itération; i++) //pour chaque test 

        {
            var curMap = node.currrentGameState; // Temporaire Map
            while (curMap.checkWinner() == null) //tant que il y a pas de victoire
            {
                var listMove = node.currrentGameState.getPossibleMove(firstPlayer,secondPlayer); // liste de move posible
                int rand = Random.Range(0, listMove.Count); //random dans la liste de move possible
                curMap = curMap.updateMap(listMove[rand].Item1, listMove[rand].Item2); //Map avec un aléatoire move possible
            }
            if (curMap.checkWinner() == firstPlayer)// si il y a un winner
                nbWin++;// nb de win ++;
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