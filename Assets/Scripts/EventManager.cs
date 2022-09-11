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

    public delegate void OnFoul();
    public  event OnFoul onFoul;

    public  void GameStarted()
    {
        onStart?.Invoke();

    }

    public  void GameFailed()
    {
        onFail?.Invoke();
    }

    public  void GameWin()
    {
        onWin?.Invoke();
    }


    public  void Foul()
    {
        onFoul?.Invoke();
    }


}
