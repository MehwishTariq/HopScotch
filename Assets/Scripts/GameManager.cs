using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public GameObject leaderBoard, content, resultPanel, resultLine, confetti_Player, confetti_Ai;
    public bool gameStarted, gameFailed, gameWon;
    public bool wrongJump;
    public Material defaultMat;
    public Material selectedMat;

    public Text PlayerRemarks;
    public Text AIRemarks;

    public List<TimeManager> otherAi = new List<TimeManager>();

    private void Awake()
    {
        instance = this;

    }
   
    public void SetTimeofOtherAi()
    {
        foreach (TimeManager x in otherAi)
        {
            LeaderBoardData.instance.SetData(x.PlayerName, Random.Range(200, 300), x.maxScore);
        }
    }
    private void Start()
    {
        SetTimeofOtherAi();
    }
    public void SetRemarks(bool wrong, bool forPlayer)
    {
        string[] wrongs = { "Aww!", "Ah Man!", ":(", ":'(" };
        string[] rights = { "Cool!", "Yes!", "Awesome!", "Nice!" };
        if (forPlayer)
        {
            if(wrong)
                PlayerRemarks.text = wrongs[Random.Range(0, wrongs.Length)];
            else
                PlayerRemarks.text = rights[Random.Range(0, rights.Length)];

            PlayerRemarks.GetComponent<RectTransform>().DOScale(1.3f, 0.2f).OnComplete(() => {
                PlayerRemarks.GetComponent<RectTransform>().DOScale(1f, 0.2f).OnComplete(() =>
                {
                    int i = 0;
                    while (i < 3)
                        i++;
                    PlayerRemarks.text = "";
                });
            });
        }
        else
        {
            if (wrong)
                AIRemarks.text = wrongs[Random.Range(0, wrongs.Length)];
            else
                AIRemarks.text = rights[Random.Range(0, rights.Length)];

            AIRemarks.GetComponent<RectTransform>().DOScale(1.3f, 0.2f).OnComplete(() => {
                AIRemarks.GetComponent<RectTransform>().DOScale(1f, 0.2f).OnComplete(() =>
                {
                    int i = 0;
                    while (i < 3)
                        i++;
                    AIRemarks.text = "";
                });
            });
        }
    }

    public void StartGame()
    {
       
        EventManager.instance.GameStarted();
        gameStarted = true;
    }

    public IEnumerator ShowLeaderBoard()
    {
        yield return new WaitForSecondsRealtime(1f);
        resultPanel.SetActive(false);
        if (!leaderBoard.activeInHierarchy)
            leaderBoard.SetActive(true);
        
    }

    public void ShowResultScreen()
    {
        resultPanel.SetActive(true);
        if (gameWon)
        {
            confetti_Player.SetActive(true);
            resultLine.GetComponent<TextMeshProUGUI>().text = "YOU WON!";
        }
        else
        {
            confetti_Ai.SetActive(true);
            resultLine.GetComponent<TextMeshProUGUI>().text = "YOU LOST!";
        }

        resultLine.transform.DOScale(1.2f, 0.4f).OnComplete(() =>
        {
            resultLine.transform.DOScale(1f, 0.3f).OnComplete(() =>
            {
                StartCoroutine(ShowLeaderBoard());
            });
        });
    }

    public void PlayAgain()
    {
        EventManager.instance.DataAdded_();
        SceneManager.LoadScene(0);
    }
}
