using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIScript : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    public bool moving;
    [SerializeField]
    float time;
    [SerializeField]
    public Animator animator;
    [SerializeField]
    CapsuleCollider boxCollider;
    [SerializeField]
    public bool forward;
    [SerializeField]
    public bool backward;




   
    public void MoveTo(Vector3 obj)
    {
        transform.DOLocalJump(obj,0.2f,1, time).OnComplete(() =>
        {
            moving = false;
            boxCollider.enabled = true;
        }).OnUpdate(()=>
        {
            boxCollider.enabled = false;
            moving = true;
        });
    }

    public void Rotate(BoxCollider collider, bool fd,bool bd)
    {
        transform.DORotate(transform.eulerAngles +  new Vector3(0, 180, 0), time).OnComplete(() =>
        {
            moving = false;
            forward = fd; 
            backward = bd;
            collider.enabled = true;
        });
    }
}
