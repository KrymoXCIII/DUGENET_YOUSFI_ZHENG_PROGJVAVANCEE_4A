using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public List<PlayerBomberman> players = new List<PlayerBomberman>();
    public List<Bomb> bombs1 = new List<Bomb>();
    public List<Bomb> bombs2 = new List<Bomb>();

    public List<Wall> walls = new List<Wall>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void removeWalls(List<Wall> list)
    {
        foreach (var wall in list)
        {
            walls.Remove(wall);
            Destroy(wall.gameObject);
        }
    }
}
