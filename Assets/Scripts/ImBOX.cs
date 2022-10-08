using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImBOX : MonoBehaviour
{
    [SerializeField]
    internal string number;
    [SerializeField]
    public bool stoneHere=false;
    [SerializeField]
    BarColor ascendJumpType;
    [SerializeField]
    BarColor descendJumpType;

    [SerializeField]
    BarColor jumpTypeforMe;

    Renderer mat;

    public Coroutine coroutine;

    private void Start()
    {
        mat = GetComponent<Renderer>();
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (GameManager.instance.gameStarted)
        { 
            if (other.tag == "Stone" && number != "0")
            {
                stoneHere = true;
            }
            else if ((other.tag == "RightLeg" || other.tag == "LeftLeg") && stoneHere)
            {
                other.GetComponentInParent<Animator>().SetBool("FailWalk", true);
                other.GetComponentInParent<PlayerMovement>().ResetPlayerToStart();
                other.GetComponentInParent<PlayerMovement>().time.AddTimeOnFoul();
                GameManager.instance.SetRemarks(true, true);
                //EventManager.instance.Foul();
            }
            else if ((other.tag == "RightLeg" || other.tag == "LeftLeg"))
            {
                if (other.GetComponentInParent<PlayerMovement>().ascend)
                {
                    other.GetComponentInParent<PlayerMovement>().tempJumpType = ascendJumpType;
                }
                else if (other.GetComponentInParent<PlayerMovement>().descend)
                {
                    other.GetComponentInParent<PlayerMovement>().tempJumpType = descendJumpType;
                }
            }
           
        }
    }
    
    public IEnumerator BlinkMaterial()
    {
        yield return new WaitForSeconds(0.01f);     
        mat.material.color = Color.green;
        mat.material.DOFade(0.3f, 0.2f).OnComplete(() => {
            mat.material.DOFade(1f, 0.2f).OnComplete(() => {
                coroutine = StartCoroutine(BlinkMaterial());
            });
        });
    }
    //void OnTriggerExit(Collider other)
    //{
        
    //    if (other.tag == "Stone")
    //    {
    //        stoneHere = false;
    //    }
    //}
}
