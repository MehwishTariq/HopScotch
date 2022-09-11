using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NayaTree : MonoBehaviour
{
    [SerializeField]
    bool start;
    [SerializeField]
    public int index;
    [SerializeField]
    public NayGrid grid;



    public Transform[] initialForwardLink;
    public Transform[] forwardLink;
    public List<Transform> backwardLink = new List<Transform>();
    public Transform[] sibling;

    void Awake()
    {
        initialForwardLink = forwardLink;
       

    }
    public void SetInitialLink()
    {
        forwardLink = initialForwardLink;
    }
    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("AI"))
        {
            StartCoroutine(MoveAI(other.gameObject.GetComponent<AIScript>()));

        }
    }
    void storeBackward(Transform t,Transform store)
    {
        if (store.GetComponent<NayaTree>().index != grid.j)
        {
            if(t.GetComponent<NayaTree>().backwardLink.Count<2)
                t.GetComponent<NayaTree>().backwardLink.Add(store);
        }
    }
    IEnumerator MoveAI(AIScript aIScript)
    {


        yield return new WaitUntil(() => aIScript.moving == false);
        if (aIScript.forward)
        {
           //trigger D
           // forward E
           // store 
            if (forwardLink.Length == 2)
            {
                

                storeBackward(forwardLink[0], this.transform);
                storeBackward(forwardLink[1], this.transform);
                if (sibling.Length > 0)
                {
                    storeBackward(forwardLink[0], this.transform);
                    storeBackward(forwardLink[1], sibling[0]);
                }

                Vector3 position = ((forwardLink[0].position + forwardLink[1].position) / 2);
                aIScript.MoveTo(position);
                aIScript.animator.SetTrigger("Twoleg");
                Debug.Log("twoLeg");
            }
            
            else if (forwardLink.Length > 0)
            {
                storeBackward(forwardLink[0], this.transform);
                //forwardLink[0].GetComponent<NayaTree>().backwardLink.Add(this.transform);
                if (sibling.Length > 0)
                {

                    storeBackward(forwardLink[0],sibling[0]);
                    //forwardLink[0].GetComponent<NayaTree>().backwardLink.Add(sibling[0]);
                }




                aIScript.MoveTo(forwardLink[0].position);

                aIScript.animator.SetTrigger("Oneleg");
                Debug.Log("OneLeg");
                //OneLegJump
            }
            if(forwardLink.Length ==2)
            {
                if (grid.j == forwardLink[0].GetComponent<NayaTree>().index)
                {
                    forwardLink[0].GetComponent<NayaTree>().sibling[0].GetComponent<NayaTree>().enabled = false;
                    forwardLink[0].GetComponent<NayaTree>().sibling[0].GetComponent<BoxCollider>().enabled = false;
                    
                }
                else
                {
                    forwardLink[0].GetComponent<NayaTree>().enabled = false;
                    forwardLink[0].GetComponent<BoxCollider>().enabled = false;
                }
                
            }
            



            //Disable SisterCollideer
            if (index == 9 || index == 10)
            {
                aIScript.forward = false;

                GetComponent<BoxCollider>().enabled = false;
                aIScript.Rotate(GetComponent<BoxCollider>(),false,true);

            } // Reset Backward
        }
        else if (aIScript.backward)
        {
           
            if (backwardLink.Count == 2)
            {

                Vector3 position = ((backwardLink[0].position + backwardLink[1].position) / 2);
                aIScript.MoveTo(position);

                aIScript.animator.SetTrigger("Twoleg");
                Debug.Log("twoLeg");
            }
            else if (backwardLink.Count > 0)
            {
                aIScript.MoveTo(backwardLink[0].position);

                aIScript.animator.SetTrigger("Oneleg");
                Debug.Log("OneLeg");
            }

            if (index == 0 )
            {
                aIScript.backward = false;
                GetComponent<BoxCollider>().enabled = false;
                grid.ResetGraph();
                aIScript.Rotate(GetComponent<BoxCollider>(), true, false);

            } // 
        }
        


       
    }
}
