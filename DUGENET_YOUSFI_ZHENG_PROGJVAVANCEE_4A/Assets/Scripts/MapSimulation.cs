using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSimulation : MonoBehaviour
{
    public PlayerSim firstPlayer;
    public PlayerSim secondPlayer;
    public List<BombSim> bombs1 = new List<BombSim>();
    public List<BombSim> bombs2 = new List<BombSim>();
    public List<BombSim> bombs1ToDestroy = new List<BombSim>();
    public List<BombSim> bombs2ToDestroy = new List<BombSim>();
    public List<BombSim> bombsToCreate = new List<BombSim>();
    public List<WallSim> walls = new List<WallSim>();
    public List<WallSim> wallsToDestroy = new List<WallSim>();
    public float collisionRadius = 1f;
    public float deltaTime = .016f;

    public MapSimulation(MapSimulation map, PlayerSim pb1)
    {
        firstPlayer = new PlayerSim(pb1); //On set le joueur qui doit jouer un coup
        //On peut donc déterminer qui est le second joueur 
        secondPlayer = map.firstPlayer == firstPlayer ? map.secondPlayer : firstPlayer;

        walls.AddRange(map.walls);

        //playerNb est toujours égale à 1 pour le premier agent
        if (pb1.nbPlayer == 1)
        {
            //Cela permet de synchroniser la map avec la scène
            //(le firstPlayer n'est pas toujours le premier agent)
            bombs1.AddRange(map.bombs1);
            bombs2.AddRange(map.bombs2);
        }
        else
        {
            bombs1.AddRange(map.bombs2);
            bombs2.AddRange(map.bombs1);
        }
    }
    public MapSimulation(MapSimulation map)
    {
        firstPlayer = map.firstPlayer;
        secondPlayer = map.secondPlayer;
        bombs1 = map.bombs1;
        bombs2 = map.bombs2;
        walls = map.walls;
    }

    public void updateMapSingleMove(move m)
    {
        switch (m)
        {
            case move.UP:
                if(checkPossibleMove(move.UP,firstPlayer))
                    firstPlayer.pos += firstPlayer.MovePlayerUp() * deltaTime;
                break;
            case move.DOWN:
                if(checkPossibleMove(move.DOWN,firstPlayer))
                    firstPlayer.pos += firstPlayer.MovePlayerDown() * deltaTime;
                break;
            case move.RIGHT:
                if(checkPossibleMove(move.RIGHT,firstPlayer))
                    firstPlayer.pos += firstPlayer.MovePlayerRight() * deltaTime;
                break;
            case move.LEFT:
                if(checkPossibleMove(move.LEFT,firstPlayer))
                    firstPlayer.pos += firstPlayer.MovePlayerLeft() * deltaTime;
                break;
            case move.BOMB:
                if (checkPossibleMove(move.BOMB, firstPlayer))
                {
                    var bomb = firstPlayer.PlantBomb();
                    if (firstPlayer.nbPlayer == 1)
                        bombs1.Add(bomb);
                    else
                        bombs2.Add(bomb);
                    bombsToCreate.Add(bomb);
                }
                break;
        }

        //Le first player à joué, on inverse donc les premier et deuxième joueurs
        (firstPlayer, secondPlayer) = (secondPlayer, firstPlayer);
    }

    public void updateBombs()
    {
        List<BombSim> bombToDelete = new List<BombSim>();
        
        foreach (var b in bombs1)
        {
            if (b.decreaseTimer()) //On décrémente le timer des bombes de 1
            {
                //Si le timer est égale à 0 alors la bombe explose
                explodeBomb(b);
                ExplosionSound.ExplosionInstance.Audio.PlayOneShot(ExplosionSound.ExplosionInstance.Click);
                if (firstPlayer.nbPlayer == 1)
                    firstPlayer.nbBomb++;
                else
                    secondPlayer.nbBomb++;
                //On retire la bombe de la liste
                bombToDelete.Add(b);
            }
        }
        foreach (var b in bombToDelete)
        {
            bombs1.Remove(b);
        }

        bombs1ToDestroy = bombToDelete;
        bombToDelete.Clear();

        foreach (var b in bombs2) 
        {
            if (b.decreaseTimer())
            {
                explodeBomb(b);
                ExplosionSound.ExplosionInstance.Audio.PlayOneShot(ExplosionSound.ExplosionInstance.Click);
                if (firstPlayer.nbPlayer == 1)
                    secondPlayer.nbBomb++;
                else
                    firstPlayer.nbBomb++;                
                bombToDelete.Add(b);
            }
        }
        foreach (var b in bombToDelete)
        {
            bombs2.Remove(b);
        }
        bombs2ToDestroy = bombToDelete;
    }
    
    public bool checkPossibleMove(move m, PlayerSim ps)
    {
        Vector3 dir;
        switch (m)
        {
            case move.UP:
                dir = ps.MovePlayerUp()*deltaTime;
                return (!collisionPlayerWalls(ps.pos+dir));
            case move.DOWN:
                dir = ps.MovePlayerDown()*deltaTime;
                return (!collisionPlayerWalls(ps.pos+dir));                
            case move.RIGHT:
                dir = ps.MovePlayerRight()*deltaTime;
                return (!collisionPlayerWalls(ps.pos+dir));
            case move.LEFT:
                dir = ps.MovePlayerLeft()*deltaTime;
                return (!collisionPlayerWalls(ps.pos+dir));
            case move.BOMB:
                return (ps.nbBomb > 0);
            case move.NOMOVE:
                return true;
            case move.NULL:
                return false;
        }
        return false;
    }
    
    public bool collisionPlayerWalls(Vector3 checkPos)
    {
        foreach (var wall in walls)
        {
            if (wall.pos.x + collisionRadius > checkPos.x &&
                wall.pos.x - collisionRadius < checkPos.x &&
                wall.pos.z + collisionRadius > checkPos.z &&
                wall.pos.z - collisionRadius < checkPos.z)
                return true;
        }
        return false;
    }

    public void explodeBomb(BombSim b)
    {
        List<WallSim> wallToRemove = new List<WallSim>();
        var pos = b.pos;
        //On vérifie si la bombe est en collision avec un mur sur sa position
        foreach (var wall in walls) 
        {
            if (wall.destructible)
            {
                //Si le mur n'est pas destructible on ne fait pas les calculs
                if (collisionBomb(wall.pos, pos))
                {
                    //Si le mur est touché on le retire de la liste
                    wallToRemove.Add(wall);
                    wall.destructible = false;
                    //On set destructible à faux pour ne pas avoir à refaire les calculs dessus
                }
            }
        }

        //On vérifie si la bombe est en collision avec un joueur sur sa position
        if (collisionBomb(firstPlayer.pos, pos))
        {
            firstPlayer.isAlive = false;
        }

        if (collisionBomb(secondPlayer.pos, pos))
        {
            secondPlayer.isAlive = false;
        }


        for (int i = 1; i < b.power; i++)
        {
            var delta = i * 1.5f;
            var newPos = new Vector3(pos.x, pos.y, pos.z);

            //On check les collisions de la bombe avec un décalage de la position
            foreach (var wall in (this.walls))
            {
                //Vers le haut
                newPos.Set(pos.x + delta, pos.y, pos.z);
                //createExplosion(pos);
                if (wall.destructible)
                {
                    if (collisionBomb(wall.pos, newPos))
                    {
                        wallToRemove.Add(wall);
                        wall.destructible = false;
                    }
                }

                //Vers la droite
                newPos.Set(pos.x, pos.y, pos.z + delta);
                //createExplosion(newPos);
                if (wall.destructible)
                {
                    if (collisionBomb(wall.pos, newPos))
                    {
                        wallToRemove.Add(wall);
                        wall.destructible = false;
                    }
                }

                //Vers la gauche
                newPos.Set(pos.x, pos.y, pos.z - delta);
                //createExplosion(newPos);
                if (wall.destructible)
                {
                    if (collisionBomb(wall.pos, newPos))
                    {
                        wallToRemove.Add(wall);
                        wall.destructible = false;
                    }
                }

                //Vers le bas
                newPos.Set(pos.x - delta, pos.y, pos.z);
                //createExplosion(newPos);
                if (wall.destructible)
                {
                    if (collisionBomb(wall.pos, newPos))
                    {
                        wallToRemove.Add(wall);
                        wall.destructible = false;
                    }
                }
            }

            foreach (var r in wallToRemove)
            {
                walls.Remove(r);
                wallsToDestroy = wallToRemove;
            }
            newPos.Set(pos.x + delta, pos.y, pos.z);
            if (collisionBomb(firstPlayer.pos, newPos))
            {
                firstPlayer.isAlive = false;
            }
            if (collisionBomb(secondPlayer.pos, newPos))
            {
                secondPlayer.isAlive = false;
            }
            
            newPos.Set(pos.x, pos.y, pos.z + delta);
            if (collisionBomb(firstPlayer.pos, newPos))
            {
                firstPlayer.isAlive = false;
            }
            if (collisionBomb(secondPlayer.pos, newPos))
            {
                secondPlayer.isAlive = false;
            }
            
            newPos.Set(pos.x, pos.y, pos.z + delta);
            if (collisionBomb(firstPlayer.pos, newPos))
            {
                firstPlayer.isAlive = false;
            }
            if (collisionBomb(secondPlayer.pos, newPos))
            {
                secondPlayer.isAlive = false;
            }
            
            newPos.Set(pos.x - delta, pos.y, pos.z);
            if (collisionBomb(firstPlayer.pos, newPos))
            {
                firstPlayer.isAlive = false;
            }
            if (collisionBomb(secondPlayer.pos, newPos))
            {
                secondPlayer.isAlive = false;
            }
        }
    }

    public bool collisionBomb(Vector3 targetPos, Vector3 checkPos)
    {
        if (Mathf.Pow(targetPos.x - checkPos.x, 2) + Mathf.Pow(targetPos.z - checkPos.z, 2) < 1)
            return true;
        return false;
    }

    public PlayerSim checkWinner()
    {
        if (!firstPlayer.isAlive)
            return secondPlayer;
        if (!secondPlayer.isAlive)
            return firstPlayer;
        return null;
    }

    public List<Tuple<move, move>> getPossibleMove()
    {
        List<Tuple<move, move>> returnList = new List<Tuple<move, move>>();
        
        List<move> listMove = new List<move>();
        listMove.Add(move.UP);
        listMove.Add(move.DOWN);
        listMove.Add(move.RIGHT);
        listMove.Add(move.LEFT);
        listMove.Add(move.BOMB);
        listMove.Add(move.NOMOVE);

        foreach (var m1 in listMove)
        {
            if (checkPossibleMove(m1, firstPlayer))
            {
                foreach (var m2 in listMove)
                {
                    if (checkPossibleMove(m2, secondPlayer))
                        returnList.Add(new Tuple<move, move>(m1,m2));
                }
            }
        }
        return returnList;
    }
 }

