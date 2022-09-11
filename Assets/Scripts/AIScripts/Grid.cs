using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField]
    public Tree[] trees;
    [SerializeField]
    public Tree startTree;
    [SerializeField]
    int j;
    //j pe stone hain

    [ContextMenu("Do Something")]
    public void SetTree()
    {
        int index = j - 1;
        trees[index].GetComponent<Tree>().enabled = false;
        if (index == 0)//Stone on A
        {
            startTree.forwardLinkLeft = trees[index].forwardLinkLeft;
        }else if(index ==1)//Stone on B
        {
            trees[index-1].forwardLinkLeft = trees[index].forwardLinkLeft;
            if(trees[index].forwardLinkRight!=null)
                trees[index - 1].forwardLinkRight = trees[index].forwardLinkRight;

        }else if(index ==2 || index==5)//Stone on C,F
        {
            trees[index - 1].forwardLinkLeft = trees[index - 1].forwardLinkRight;
            trees[index - 1].forwardLinkRight = null;
        }
        else if(index == 3 || index ==6)//Stone on D,G
        {

            trees[index - 2].forwardLinkRight = null;
           
        }else if (index == 4 || index ==7)//Stone on E
        {
            trees[index - 2].forwardLinkLeft = trees[index].forwardLinkLeft;
            trees[index - 2].forwardLinkRight = trees[index].forwardLinkRight;

        }
        
    }
}
