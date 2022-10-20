using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MCTS
{
    private List<NodeMCTS> listNode;

    public MCTS(MapSimulation map, PlayerSim pb1)
    {
<<<<<<< HEAD
        MapSimulation mapS = new MapSimulation(map, pb1);
=======
        mapSimulation mapS = new mapSimulation(map);
>>>>>>> parent of 874aa7f (wip mcts)
        NodeMCTS first = new NodeMCTS(mapS);
        listNode = new List<NodeMCTS>();
        listNode.Add(first);
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

            simulation(newNode, nbTest);
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
        float explo = Random.Range(0, 1); // Random (0,1)
        if (explo < .8) //80% exploration
        {
            int rand = Random.Range(0, listNode.Count); //  aléatoire un nombre entre le nombre de node
            var node = listNode.GetRange(rand, 1).First(); //Choisi le node aléatoire
<<<<<<< HEAD
            var childCount = listNode.Where(t => t.parent == node).Count();
            var moveCount = node.currrentGameState.getPossibleMove().Count;

            List<NodeMCTS> copyList = new List<NodeMCTS>(listNode.Count);
            foreach (var n in listNode)
=======
            int nodeCount = listNode.Count();
            while (!node.end && nodeCount != 0) // tant que le node n'est pas une feuille
>>>>>>> parent of 874aa7f (wip mcts)
            {
                rand = Random.Range(0, listNode.Count); //aléatoire un nombre entre le nombre de node
                node = listNode.GetRange(rand, 1).First(); //Choisi le node aléatoire
<<<<<<< HEAD
                childCount = listNode.Where(t => t.parent == node).Count();
                moveCount = node.currrentGameState.getPossibleMove().Count;

                if (node.end || childCount == moveCount)
                    copyList.Remove(node);
=======
                nodeCount--;
>>>>>>> parent of 874aa7f (wip mcts)
            }

            if (nodeCount == 0)
                return null;
            return node; // return la feuille de l'arbre
        }
        //20% explotation
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

    NodeMCTS expansion(NodeMCTS node) // Expension
    {
<<<<<<< HEAD
        var possibleMoves = node.currrentGameState.getPossibleMove(); //liste de move possible
        var nodeChild = listNode.Where(t=>t.parent == node);
        foreach (var child in nodeChild)
=======
        var possibleMoves = node.currrentGameState.getPossibleMove(firstPlayer, secondPlayer); //liste de move possible
        var rand = Random.Range(0, possibleMoves.Count); //aléatoire un node dans la liste de move possible
        int n = listNode.Where(t =>
            t.parent == node && t.moveP1 == possibleMoves[rand].Item1 && t.moveP1 == possibleMoves[rand].Item1).Count();
        //passe dans la listeNode où n prend la valeur du nombre de fois on est passer sur le même node
        while (n > 0) // tant qu'on a déjà passer une fois
>>>>>>> parent of 874aa7f (wip mcts)
        {
            rand = Random.Range(0, possibleMoves.Count); // choisie une random dans la liste possible move
            n = listNode.Where(t =>
                    t.parent == node && t.moveP1 == possibleMoves[rand].Item1 && t.moveP1 == possibleMoves[rand].Item1)
                .Count();
            // passe dans la listeNode où n prend la valeur du nombre de fois on est passer sur le même node
        }

<<<<<<< HEAD
        if (possibleMoves.Count == 0)
            return null;

        int rand = Random.Range(0, possibleMoves.Count);

        node.currrentGameState.updateMapSingleMove(possibleMoves[rand].Item1);
        node.currrentGameState.updateMapSingleMove(possibleMoves[rand].Item2);
        node.currrentGameState.updateBombs();
        
        var newNode = new NodeMCTS(node.currrentGameState, node, possibleMoves[rand].Item1, possibleMoves[rand].Item2);
=======
        var newGameState = node.currrentGameState.updateMap(possibleMoves[rand].Item1, possibleMoves[rand].Item2);
        var newNode = new NodeMCTS(newGameState, node, possibleMoves[rand].Item1, possibleMoves[rand].Item2);
>>>>>>> parent of 874aa7f (wip mcts)
        // créer une nouvelle NODE avec le prochaine étape de mouvement
        listNode.Add(newNode); //ajout dans la liste de node
        return newNode; //return le nouveau node
    }


    void simulation(NodeMCTS node, int itération) // Simulation itération = nombre de test 
    {
        int nbWin = 0; // initialise le nombre de win
        for (int i = 0; i < itération; i++) //pour chaque test 
        {
<<<<<<< HEAD
            int step = 100;
=======
            int step = 10000;
>>>>>>> parent of 874aa7f (wip mcts)
            var curMap = node.currrentGameState; // Temporaire Map
            while (curMap.checkWinner() == null && step > 0) //tant que il y a pas de victoire
            {
                step--;
<<<<<<< HEAD
                var listMove = curMap.getPossibleMove(); // liste de move posible
=======
                var listMove =
                    node.currrentGameState.getPossibleMove(firstPlayer, secondPlayer); // liste de move posible
>>>>>>> parent of 874aa7f (wip mcts)
                int rand = Random.Range(0, listMove.Count); //random dans la liste de move possible
                curMap.updateMapSingleMove(listMove[rand].Item1);
                curMap.updateMapSingleMove(listMove[rand].Item2);
                curMap.updateBombs();
            }
<<<<<<< HEAD
            
            if (curMap.checkWinner() == node.currrentGameState.firstPlayer || step == 0) // si il y a un winner
                nbWin++; // nb de win ++;
        }
        node.nbWin = nbWin;
        node.nbMove = itération;
=======

            if (curMap.checkWinner() == firstPlayer) // si il y a un winner
                nbWin++; // nb de win ++;
        }
>>>>>>> parent of 874aa7f (wip mcts)
    }

    void backPropagation(NodeMCTS node) // backPropagation
    {
        var curNode = node;
        while (curNode.parent != null) // tant que C'est pas le node parent
        {
            checkNodeEnd(curNode); // check tous les fils sont end et il a tous ces fils
            NodeMCTS parent = curNode.parent; // créer un parent
            parent.nbWin += node.nbWin; // incrémente le nombre de win
            parent.nbMove += node.nbMove; // incrémente le nombre de sumulation
            curNode = parent;
        }
<<<<<<< HEAD
    }
    
    void checkNodeEnd(NodeMCTS node) // check tous les fils sont end et il a tous ces fils
    {
        if (node.end) // si le node est le feuille alors en return
            return;
        //On récupère tous les mouvement possible pour ce noeud
        var Tmoves = node.currrentGameState.getPossibleMove();
        //On récupére tous ces enfants
        var childList = listNode.Where(t => t.parent == node);
        //Si il y a moins d'enfants que de movements possibles alors il y'a encore des noeuds inexploré
        if (childList.Count() < Tmoves.Count())
            return;
        //Si tous les enfants sont fini alors le noeud est aussi fini
        foreach (var childNode in childList)
=======

        void checkNodeEnd(NodeMCTS node) // check tous les fils sont end et il a tous ces fils
>>>>>>> parent of 874aa7f (wip mcts)
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
<<<<<<< HEAD
                return;
=======
                if (!childNode.end)
                {
                    isEnd = false;
                }
>>>>>>> parent of 874aa7f (wip mcts)
            }

            node.end = isEnd;
        }
<<<<<<< HEAD
        node.end = true;
=======

>>>>>>> parent of 874aa7f (wip mcts)
    }
}