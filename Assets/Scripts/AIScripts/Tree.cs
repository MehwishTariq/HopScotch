using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[Serializable]
public class Tree : MonoBehaviour
{
    [SerializeField]
    bool start;

    [SerializeField]
    int index;


    public Transform initalforwardLinkLeft;
    public Transform initalforwardLinkRight;


    public Transform forwardLinkLeft;
    public Transform forwardLinkRight;
    void Start()
    {
        initalforwardLinkLeft = forwardLinkLeft;
        initalforwardLinkRight = forwardLinkRight;

    }
    public void SetInitialLink()
    {
       forwardLinkLeft = initalforwardLinkLeft ;
        forwardLinkRight= initalforwardLinkRight ;
    }
    void OnTriggerEnter(Collider other)
    {
       
        if (other.gameObject.CompareTag("AI"))
        {
            StartCoroutine(MoveAI(other.gameObject.GetComponent<AIScript>()));

        } 
    }
    IEnumerator MoveAI(AIScript aIScript)
    {
        
        
        yield return new WaitUntil(() => aIScript.moving ==false);
        
        if (forwardLinkLeft!=null && forwardLinkRight != null)
        {

           Vector3 position = ((forwardLinkLeft.position + forwardLinkRight.position) / 2);
           aIScript.MoveTo(position);
           
            aIScript.animator.SetTrigger("Twoleg");
           
            //TwoLegJump
        }
        else if (forwardLinkLeft != null)
        {
            aIScript.MoveTo(forwardLinkLeft.position);

            aIScript.animator.SetTrigger("Oneleg");
           
            //OneLegJump
        }
        if (forwardLinkLeft != null)
        {
            if (forwardLinkLeft.GetComponent<Tree>().forwardLinkLeft && forwardLinkLeft.GetComponent<Tree>().forwardLinkRight)
            {

                forwardLinkLeft.GetComponent<Tree>().forwardLinkRight.GetComponent<Tree>().enabled = false;
                forwardLinkLeft.GetComponent<Tree>().forwardLinkRight.GetComponent<BoxCollider>().enabled = false;
            }
        }
        
    }
}
