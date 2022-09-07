using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;

public enum BarColor { None, OneLegJump, TwoLegJump, SkipJump }
public class MovingBar : MonoBehaviour
{

    [SerializeField]
    public BarColor currentBar;
    [SerializeField]
    RectTransform myTransform;
    [SerializeField]
    bool move;
    [SerializeField]
    RectTransform topTarget,BottomTarget;
    [SerializeField]
    float timeMin, timeMax;
    [SerializeField]
    Ease type;
    [SerializeField]
    Text demoText;



    void GoTop()
    {
        myTransform.DOAnchorPosY(topTarget.anchoredPosition.y, UnityEngine.Random.Range(timeMin, timeMax)).SetEase(type).OnComplete(() => GoBottom());
       
    }
    void GoBottom()
    {
       
        myTransform.DOAnchorPosY(BottomTarget.anchoredPosition.y, UnityEngine.Random.Range(timeMin, timeMax)).SetEase(type).OnComplete(() => GoTop());
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        currentBar = (BarColor)Enum.Parse(typeof(BarColor), collision.tag.ToString());
        demoText.text = collision.tag.ToString();
    }
    public void KillNow()
    {
        myTransform.DOKill();
       // demoText.text = "Stop position: "+ currentBar.ToString();
    }
    public void StartMoving()
    {
        if (UnityEngine.Random.Range(0, 100) > 50)
        {
            myTransform.anchoredPosition = new Vector2(myTransform.anchoredPosition.x, BottomTarget.anchoredPosition.y);
            GoTop();

        }
        else
        {
            myTransform.anchoredPosition = new Vector2(myTransform.anchoredPosition.x, topTarget.anchoredPosition.y);
            GoBottom();
        }
    }
}
