using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseMode : MonoBehaviour
{
    public static ChooseMode instance;
    public int CM1;
    public int CM2;
    public TextMeshProUGUI Text1;
    public TextMeshProUGUI Text2;
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
        Text1.text = "Random";
    }
    public void choose1Player()
    {
        CM1 = 1;
        Text1.text = "Player";
    }
    public void choose1MCTS()
    {
        CM1= 2;
        Text1.text = "MCTS";
    }
    public void choose2Random()
    {
        CM2= 0;
        Text2.text = "Random";
    }
    public void choose2Player()
    {
        CM2= 1;
        Text2.text = "Player";
    }
    public void choose2MCTS()
    {
        CM2= 2;
        Text2.text = "MCTS";
    }
}
