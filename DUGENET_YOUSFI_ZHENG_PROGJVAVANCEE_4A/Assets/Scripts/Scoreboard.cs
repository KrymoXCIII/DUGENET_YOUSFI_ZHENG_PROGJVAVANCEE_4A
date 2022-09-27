using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Scoreboard : MonoBehaviour
{
    public static bool isGameEnd = false;
    [SerializeField] GameObject ScoreBoard;
    public Text RankText1;
    public Text RankText2;
    public Text ScoreText1;
    public Text ScoreText2;
    public Text NameText1;
    public Text NameText2;

    
    


    
    public void PlayAgain()
    {
       
        ScoreBoard.SetActive(false);
        Time.timeScale = 1f;
    }

    public void BackMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }


}
