using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImBOX : MonoBehaviour
{
    [SerializeField]
    bool stoneHere=false;
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Stone")
        {
            stoneHere = true;
        }else if((other.tag== "RightLeg" || other.tag == "LeftLeg") && stoneHere)
        {
            Debug.Log("levelFailRestart");
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
