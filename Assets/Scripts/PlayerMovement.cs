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

    public int NoToMoveOn;
    public static bool isHopping;

    
    [SerializeField]
    public static int HopNo;
    public bool oneLegHop, twoLegHop;
    public bool ascend, descend;
    public bool activateBar;

    public Transform startPos;

    public MovingBar bar;
    BarColor jumpType;
    public List<Transform> boxes;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<Transform>();
        anim = GetComponentInChildren<Animator>();
        HopNo = 1;
        NoToMoveOn = 1;
        ascend = true;
        descend = false;
    }

  
    // Update is called once per frame
    void Update()
    {
        
        if (StopMovement)
        {
            return;
        }
            
        if(isHopping)
        {
            if (!activateBar)
            {
                bar.StartMoving();
                activateBar = true;
            }
            if (Input.GetKey(KeyCode.Space))
            {
                jumpType = bar.currentBar;
            }
        }
        
        if (jumpType == BarColor.OneLegJump && !oneLegHop && isHopping)
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
                    controller.DOLocalJump(boxes[NoToMoveOn - 1].position, 0.5f, 1, 0.8f).OnComplete(() =>
                    {
                        NoToMoveOn++;
                        if (NoToMoveOn > boxes.Count)
                        {
                            ascend = false;
                            descend = true;

                            NoToMoveOn--;
                            controller.DORotate(controller.transform.rotation.eulerAngles + new Vector3(0, 180, 0),0.1f);
                        }

                        jumpType = BarColor.None;
                        oneLegHop = false;
                        Debug.Log(NoToMoveOn);
                    });
                }
                else if (descend)
                {
                    controller.DOLocalJump(boxes[NoToMoveOn - 1].position, 0.5f, 1, 0.8f).OnComplete(() =>
                    {
                        NoToMoveOn--;
                        if (NoToMoveOn < 1)
                        {
                            ascend = true;
                            descend = false;
                            isHopping = false;
                            bar.KillNow();
                            activateBar = false;
                            controller.DOLocalJump(startPos.position, 0.5f, 1, 0.8f).OnComplete(() =>
                            {
                                controller.DORotate(controller.transform.rotation.eulerAngles + new Vector3(0, -180, 0), 0.1f);
                                HopNo++;
                                anim.SetTrigger("Idle");
                            });
                            NoToMoveOn++;
                        }
                        jumpType = BarColor.None;
                        oneLegHop = false;
                        Debug.Log(NoToMoveOn);
                    });
                }

            }
        }

        if (jumpType == BarColor.TwoLegJump && !twoLegHop && isHopping)
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
                    controller.DOLocalJump((boxes[NoToMoveOn].position + boxes[NoToMoveOn - 1].position) / 2, 0.5f, 1, 0.8f).OnComplete(() =>
                    {
                        NoToMoveOn += 2;
                        if (NoToMoveOn > boxes.Count)
                        {
                            ascend = false;
                            descend = true;
                            NoToMoveOn -= 3;
                           
                            controller.DORotate(controller.transform.rotation.eulerAngles + new Vector3(0, 180, 0), 0.1f);
                        }
                        jumpType = BarColor.None;
                        twoLegHop = false;
                        Debug.Log(NoToMoveOn);
                    });
                }
                else if (descend)
                {
                    
                    controller.DOLocalJump((boxes[NoToMoveOn - 1].position + boxes[NoToMoveOn - 2].position) / 2, 0.5f, 1, 0.8f).OnComplete(() =>
                    {

                        NoToMoveOn -= 2;
                        if (NoToMoveOn - 1 < 0)
                        {
                            ascend = true;
                            descend = false;
                            controller.DORotate(controller.transform.rotation.eulerAngles + new Vector3(0, -180, 0), 0.1f);
                        }

                        jumpType = BarColor.None;
                        twoLegHop = false;
                        Debug.Log(NoToMoveOn);
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