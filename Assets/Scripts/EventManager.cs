using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;

    private void Awake()
    {
        instance = this;
    }

    public delegate void OnGameStart();
    public  event OnGameStart onStart;

    public delegate void OnGameFail();
    public  event OnGameFail onFail;

    public delegate void OnGameWon();
    public  event OnGameWon onWin;


    public delegate void DataAdded();
    public event DataAdded onAddData;

    public  void GameStarted()
    {
        onStart?.Invoke();

    }

    public void DataAdded_()
    {
        onAddData?.Invoke();
    }

    public  void GameFailed()
    {
        GameManager.instance.gameFailed = true;
        onFail?.Invoke();
    }

    public  void GameWin()
    {
        GameManager.instance.gameWon = true;
        onWin?.Invoke();
    }

}
