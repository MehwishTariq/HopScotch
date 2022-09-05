using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StoneCollision : MonoBehaviour
{
    private Tweener transitionTweener;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8 && other.bounds.Intersects(gameObject.GetComponent<SphereCollider>().bounds) && other.gameObject.name == PlayerMovement.NoToThrowOn.ToString())
        {
            //transitionTweener = DOTween.To(() => GetComponent<Rigidbody>().drag, x => GetComponent<Rigidbody>().drag = x, 2, 0.3f);
            //transitionTweener.OnUpdate(() => Debug.Log(GetComponent<Rigidbody>().drag));
            GetComponent<Rigidbody>().drag = 50;
        }
    }
}