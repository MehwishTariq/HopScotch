using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public GameObject leaderBoard;
    public bool gameStarted;
    public bool wrongJump;
    public Material defaultMat;
    public Material selectedMat;


    private void Awake()
    {
        instance = this;
    }
    public void StartGame()
    {
        Debug.Log("StartGame");
        EventManager.instance.GameStarted();
        gameStarted = true;
    }

    public void ShowLeaderBoard()
    {
        leaderBoard.SetActive(true);
    }

}
