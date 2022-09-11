using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public void StartGame()
    {
        Debug.Log("StartGame");
        EventManager.instance.GameStarted();
    }

}
