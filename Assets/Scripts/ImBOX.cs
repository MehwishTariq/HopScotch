using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImBOX : MonoBehaviour
{
    [SerializeField]
    internal string number;
    [SerializeField]
    bool stoneHere=false;
    [SerializeField]
    BarColor ascendJumpType;
    [SerializeField]
    BarColor descendJumpType;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Stone")
        {
            stoneHere = true;
        }
        else if((other.tag== "RightLeg" || other.tag == "LeftLeg") && stoneHere)
        {
            Debug.Log("levelFailRestart");
        }
        else if((other.tag == "RightLeg" || other.tag == "LeftLeg"))
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
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Stone")
        {
            stoneHere = false;
        }
    }
}
