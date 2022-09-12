using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NayGrid : MonoBehaviour
{
    [SerializeField]
    AIScript aiScript;
    [SerializeField]
    public NayaTree[] trees;
    [SerializeField]
    public NayaTree startTree;
    [SerializeField]
    public int j;
    //j pe stone hain
    [SerializeField]
    NayaTree temp ;

    void OnEnable()
    {
        EventManager.instance.onStart += AIStart;
        EventManager.instance.onWin += onGameDone;
        EventManager.instance.onFail += onGameDone;
    }

    private void OnDisable()
    {
        EventManager.instance.onStart -= AIStart;
        EventManager.instance.onWin -= onGameDone;
        EventManager.instance.onFail -= onGameDone;
    }


    public void onGameDone()
    {
        GameManager.instance.gameStarted = false;
        foreach (NayaTree x in trees)
        {
            x.enabled = false;
            x.gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }


    public void AIStart()
    {
        StartCoroutine(ThrowStone());
    }

    IEnumerator ThrowStone() {
        yield return new WaitForSecondsRealtime(1f);
        aiScript.Throw(trees[j - 1].transform);
        Debug.Log("here");
        yield return new WaitForSecondsRealtime(2f);
        SetTree();
        Debug.Log("her2e");
        aiScript.GetComponent<CapsuleCollider>().enabled = true;
    }


    [ContextMenu("Do Something")]
    public void SetTree()
    {
        int index = j - 1;
        trees[index].GetComponent<NayaTree>().enabled = false;
        if (index == 0)//Stone on A
        {
            startTree.forwardLink = trees[0].forwardLink;
        }
        else
        {
            if (trees[index].sibling.Length > 0)
            {
                if(trees[index].sibling[0].GetComponent<NayaTree>().index== trees[index - 1].index)
                {
                    trees[index - 2].forwardLink = trees[index].sibling;
                }
                else
                {
                    trees[index - 1].forwardLink = trees[index].sibling;
                }
                
            }
            else
            {
                trees[index-1].forwardLink = trees[index].forwardLink;
              
            }
        }

    }
    public void ResetGraph()
    {
        aiScript.GetComponent<CapsuleCollider>().enabled = false;
        startTree.SetInitialLink();
        foreach (var item in trees)
        {
            item.SetInitialLink();
            item.enabled = true;
            item.GetComponent<BoxCollider>().enabled = true;

            item.backwardLink.Clear();
        }
        j++;
        if(j<10)
        {
            Destroy(AIScript.thrownStone);
            aiScript.animator.SetTrigger("Idle");
            StartCoroutine(ThrowStone());
            //aiScript.GetComponent<CapsuleCollider>().enabled = true;
            //SetTree();
        }
        else
        {
            Debug.Log("GameLost");
            EventManager.instance.GameFailed();
        }
    }
}
