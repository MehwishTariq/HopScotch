using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Transform controller;
    Animator anim;
    public float speed = 3f;
    public bool StopMovement = false;
    public bool IsAiming = false;

    public static int NoToThrowOn;
    [SerializeField]
    int tempNo;
    public bool oneLegHop, twoLegHop;
    public bool ascend, descend;
    float jumpAmount = 4;

    public List<Transform> boxes;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<Transform>();
        anim = GetComponentInChildren<Animator>();
        NoToThrowOn = 1;
        ascend = true;
        descend = false;
    }

  
    // Update is called once per frame
    void Update()
    {
        tempNo = NoToThrowOn;
        if (StopMovement)
        {
            return;
        }
     
        if (Input.GetKey(KeyCode.O) && !oneLegHop)
        {
            IsAiming = false;
            //controller.AddForce(new Vector3(controller.position.x, Vector2.up.y * jumpAmount, 0), ForceMode.Impulse);
            oneLegHop = true;

            Debug.Log("Jumpp");
            if (oneLegHop)
            {
                anim.SetTrigger("oneLeg");
                //anim.Play("OneLegJump");

                if (ascend)
                {
                    controller.DOLocalJump(boxes[NoToThrowOn - 1].position, 0.5f, 1, 0.8f).OnComplete(() =>
                    {
                        NoToThrowOn++;
                        if (NoToThrowOn > boxes.Count)
                        {
                                ascend = false;
                                descend = true;

                                NoToThrowOn--;
                                controller.DORotate(controller.transform.rotation.eulerAngles + new Vector3(0, 180, 0),0.1f);
                         }
                        
                        oneLegHop = false;
                        Debug.Log(NoToThrowOn);
                    });
                }
                else if (descend)
                {
                    controller.DOLocalJump(boxes[NoToThrowOn - 1].position, 0.5f, 1, 0.8f).OnComplete(() =>
                    {
                        NoToThrowOn--;
                        if (NoToThrowOn < 1)
                        {
                            ascend = true;
                            descend = false;
                            controller.DORotate(controller.transform.rotation.eulerAngles + new Vector3(0, -180, 0), 0.1f);
                            NoToThrowOn++;
                        }

                        oneLegHop = false;
                        Debug.Log(NoToThrowOn);
                    });
                }

            }
        }

        if (Input.GetKey(KeyCode.T) && !twoLegHop)
        {
            IsAiming = false;
            //controller.AddForce(new Vector3(controller.position.x, Vector2.up.y * jumpAmount, 0), ForceMode.Impulse);
            twoLegHop = true;

            Debug.Log("Jumpp");
            if (twoLegHop)
            {

                anim.SetTrigger("twoLeg");
                //anim.Play("TwoLegJump");
                if (ascend)
                {
                    controller.DOLocalJump((boxes[NoToThrowOn].position + boxes[NoToThrowOn - 1].position) / 2, 0.5f, 1, 0.8f).OnComplete(() =>
                    {
                        NoToThrowOn += 2;
                        if (NoToThrowOn > boxes.Count)
                        {
                                ascend = false;
                                descend = true;
                                NoToThrowOn -= 3;
                                controller.DORotate(controller.transform.rotation.eulerAngles + new Vector3(0, 180, 0), 0.1f);
                        }
                       
                        twoLegHop = false;
                        Debug.Log(NoToThrowOn);
                    });
                }
                else if (descend)
                {
                    
                    controller.DOLocalJump((boxes[NoToThrowOn-1].position + boxes[NoToThrowOn-2].position) / 2, 0.5f, 1, 0.8f).OnComplete(() =>
                    {
                       
                            NoToThrowOn -= 2;
                            if (NoToThrowOn - 1 < 0)
                            {
                                ascend = true;
                                descend = false;
                                controller.DORotate(controller.transform.rotation.eulerAngles + new Vector3(0, -180, 0), 0.1f);
                            }
                        

                        twoLegHop = false;
                        Debug.Log(NoToThrowOn);
                    });
                }

            }
        }

        else if (IsAiming)
        {
            Plane p = new Plane(Vector3.up, 0f);
            float Dist;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction, Color.red);
            if (p.Raycast(ray, out Dist) && Input.GetMouseButton(1))
            {
                Vector3 Dir = ray.GetPoint(Dist) - transform.position;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Dir), Time.deltaTime * 5f);
            }
        }
    }
}