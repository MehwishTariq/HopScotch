using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StoneCollision : MonoBehaviour
{
    [SerializeField]
    float time;
    [SerializeField]
    float increment;
    [SerializeField]
    bool xyz;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8 && xyz == false)
        {
            GetComponent<Rigidbody>().isKinematic = true;
            if (other.GetComponent<ImBOX>())
            {
                if (other.GetComponent<ImBOX>().number == PlayerMovement.HopNo.ToString())
                {
                    
                    PlayerMovement p = FindObjectOfType<PlayerMovement>();
                    p.disableInput = false;
                    p.HasReset = false;
                    p.SetMaterial(other.GetComponent<ImBOX>().transform, true);
                    AudioManager.instance.Play("StoneThrow");
                    PlayerMovement.isHopping = true;

                }
                else
                {
                    Destroy(StoneMovement.currentStone);
                    FindObjectOfType<PlayerMovement>().RemoveBool();
                }
            }

        }
    }
    //IEnumerator Delay(Rigidbody rb,Collider other)
    //{
    //    yield return new WaitForSeconds(time);
    //    if(GetComponent<Rigidbody>().drag< 50)
    //    {
    //        GetComponent<Rigidbody>().drag += increment;
    //        StartCoroutine(Delay(rb,other));
    //    }
    //    else
    //    {
    //        if (other.gameObject.layer == 8 /*&& other.bounds.Intersects(gameObject.GetComponent<BoxCollider>().bounds)*/ && (other.GetComponent<ImBOX>().number == PlayerMovement.HopNo.ToString()))
    //        {
    //            PlayerMovement.isHopping = true;
    //        }
    //        else
    //        {
    //            Destroy(StoneMovement.currentStone);

    //        }
           
    //    }
    //}


}