using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NayGrid : MonoBehaviour
{
    [SerializeField]
    GameObject[] All_aiScript;

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

    [SerializeField]
    int randomNum;

    void Start()
    {
        All_aiScript[Random.Range(0, All_aiScript.Length)].SetActive(true);
        
        foreach (GameObject x in All_aiScript)
        {
            if (x.activeInHierarchy)
                aiScript = x.GetComponent<AIScript>();
            else
                GameManager.instance.otherAi.Add(x.GetComponent<AIScript>().AI_timeData);
        }
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
        GameManager.instance.AIHopNo.text = "Hop No : " + j.ToString();

    }

    List<int> idk = new List<int>();


    public void SetMaterial(Transform current, bool canSet)
    {
        foreach (var item in trees)
        {
            item.gameObject.GetComponent<MeshRenderer>().material = GameManager.instance.defaultMat;
        }
        if (canSet)
            current.gameObject.GetComponent<MeshRenderer>().material = GameManager.instance.selectedMat;
    }
    public IEnumerator ThrowStone() {
        idk.Clear();

        for (int i = 0; i < trees.Length; i++)
        {
            if(j!=i+1)
                idk.Add(i + 1);
        }
       
        yield return new WaitForSecondsRealtime(1f);
        
        if(Random.Range(0,100)<aiScript.difficulty.chancesOfWin && Random.Range(0, 100) > 0)
        {
            aiScript.ResetDifficulty();
            aiScript.Throw(trees[j - 1].transform,false);
            SetMaterial(trees[j - 1].transform, true);
            aiScript.currentStoneLocation = j;
            yield return new WaitForSecondsRealtime(2f);
            if (Random.Range(0, 100) < aiScript.difficulty.chancesOfWin)
            {
              
                SetTree(j);

            }
            else
            {
                
                SetTree(idk[Random.Range(0, idk.Count)]);
            }
            aiScript.GetComponent<CapsuleCollider>().enabled = true;
        }
        else
        {
            aiScript.difficulty.chancesOfWin++;
            int i = Random.Range(0, idk.Count);
            SetMaterial(trees[idk[i] - 1].transform, true);
            aiScript.Throw(trees[idk[i]-1].transform,true);;
            yield return new WaitForSecondsRealtime(5f);
            StartCoroutine(ThrowStone());
        }
       
    }


    [ContextMenu("Do Something")]
    public void SetTree(int value)
    {
        int index = value - 1;
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
        ResetGridToDefault();
        j++;
        if(j<=10)
        {
          
            aiScript.animator.SetTrigger("Idle");
            StartCoroutine(ThrowStone());
            GameManager.instance.AIHopNo.text = "Hop No : " + j.ToString();
            //aiScript.GetComponent<CapsuleCollider>().enabled = true;
            //SetTree();
        }
        else
        {
            
            AudioManager.instance.Play("Lose");
            EventManager.instance.GameFailed();
        }
    }
    public void ResetGridToDefault()
    {
        startTree.SetInitialLink();
        foreach (var item in trees)
        {
            item.SetInitialLink();
            item.enabled = true;
            item.GetComponent<BoxCollider>().enabled = true;

            item.backwardLink.Clear();
        }
    }
}
