using System;
using UnityEngine;

public class ChooseMode : MonoBehaviour
{
    public static ChooseMode instance;
    public int CM1;
    public int CM2;

    private void Awake()
    {
        if (instance != null)
        {
            return;
        }

        instance = this;
        DontDestroyOnLoad(instance);
    }

    public void choose1Random()
    {
        CM1= 0;
    }
    public void choose1Player()
    {
        CM1 = 1;
    }
    public void choose1MCTS()
    {
        CM1= 2;
    }
    public void choose2Random()
    {
        CM2= 0;
    }
    public void choose2Player()
    {
        CM2= 1;
    }
    public void choose2MCTS()
    {
        CM2= 2;
    }
}
