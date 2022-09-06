using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StoneCollision : MonoBehaviour
{
    private Tweener transitionTweener;
    [SerializeField]
    float time;
    [SerializeField]
    float increment;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8 && other.bounds.Intersects(gameObject.GetComponent<BoxCollider>().bounds) && other.gameObject.name == PlayerMovement.NoToThrowOn.ToString())
        {
            //transitionTweener = DOTween.To(() => GetComponent<Rigidbody>().drag, x => GetComponent<Rigidbody>().drag = x, 2, 0.3f);
            //transitionTweener.OnUpdate(() => Debug.Log(GetComponent<Rigidbody>().drag));
            Debug.Log("farigh");
            StartCoroutine(Delay(GetComponent<Rigidbody>()));
            //GetComponent<Rigidbody>().drag = 50;
        }
    }
    IEnumerator Delay(Rigidbody rb)
    {
        yield return new WaitForSeconds(time);
        if(GetComponent<Rigidbody>().drag< 50)
        {
            GetComponent<Rigidbody>().drag += increment;
            StartCoroutine(Delay(rb));
        }
    }
}