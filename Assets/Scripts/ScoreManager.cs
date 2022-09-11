using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ScoreType
{
    CorrectJump,
    CompleteRound,
    CompleteGame,
}

[System.Serializable]
public struct Score
{
    [SerializeField]
    public ScoreType scoreType;
    public int _score;
}
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public GameObject Texttoinstantiate;
    [SerializeField]
    List<Score> scores;

    public Transform RightLeg;

    private void Awake()
    {
        instance = this;
    }
    public void SetScore(ScoreType _type, Transform jumpedBox)
    {
        Debug.Log(jumpedBox.name);
        int scoreGained = 0;

        foreach (Score x in scores)
        {
            if (x.scoreType == _type)
                scoreGained = x._score;
        }

        Texttoinstantiate.transform.SetParent(jumpedBox);
        Texttoinstantiate.transform.localPosition = new Vector3(0, 0.19f,0);
        Texttoinstantiate.transform.localEulerAngles = new Vector3(0, -90,0);
        Texttoinstantiate.transform.localScale = new Vector3(1, 50,1);
        Texttoinstantiate.GetComponent<TextMesh>().text = scoreGained.ToString();
    }

}
