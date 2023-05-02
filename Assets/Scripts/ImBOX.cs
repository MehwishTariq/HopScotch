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

    public IEnumerator coroutine;
    public bool continueCoroutine;
    private void Start()
    {
        mat = GetComponent<Renderer>();
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (GameManager.instance.gameStarted)
        { 
            if (other.CompareTag("Stone") && number != "0")
            {
                stoneHere = true;
            }
            else if ((other.CompareTag("RightLeg") || other.CompareTag("LeftLeg")) && stoneHere)
            {
                other.GetComponentInParent<Animator>().SetBool("FailWalk", true);
                other.GetComponentInParent<PlayerMovement>().ResetPlayerToStart();
                other.GetComponentInParent<PlayerMovement>().time.AddTimeOnFoul();
                GameManager.instance.SetRemarks(true, true);
                //EventManager.instance.Foul();
            }
            else if ((other.CompareTag("RightLeg")|| other.CompareTag("LeftLeg")))
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
    public void StartMaterialBlink()
    {
        continueCoroutine = true;
        coroutine = BlinkMaterial();
        StartCoroutine(coroutine);
    }
    public void StopMaterialBlink()
    {
        if (coroutine != null)
        {

            continueCoroutine = false;
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }
    public IEnumerator BlinkMaterial()
    {
        yield return new WaitForSeconds(0.01f);     
        mat.material.color = Color.green;
        mat.material.DOFade(0.3f, 0.2f);
        yield return new WaitForSeconds(0.2f);
        mat.material.DOFade(1f, 0.2f);
        yield return new WaitForSeconds(0.2f);
        if (continueCoroutine)
            StartMaterialBlink();
        else
            StopMaterialBlink();
    }
    //void OnTriggerExit(Collider other)
    //{
        
    //    if (other.tag == "Stone")
    //    {
    //        stoneHere = false;
    //    }
    //}
}
