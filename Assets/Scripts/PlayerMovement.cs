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
    [SerializeField]
    int tempHopNumber;
    // Update is called once per frame
    [SerializeField]
    bool temp = false;
    [SerializeField]
    public BarColor tempJumpType = BarColor.None;
    [SerializeField]
    bool testing;

    bool HasReset;

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

    public void RemoveBool()
    {
        foreach(Transform x in boxes)
        {
            x.gameObject.GetComponent<ImBOX>().stoneHere = false;
        }
    }

    public void ResetPlayerToStart()
    {
        Debug.Log("Reset");
        ascend = true;
        descend = false;
        isHopping = false;
        bar.KillNow();
        activateBar = false;
        oneLegHop = false;
        twoLegHop = false;
        HasReset = true;
        jumpType = BarColor.None;
        controller.DORotate(new Vector3(0, 90, 0), 0.1f).OnComplete(() =>
        {
            controller.DOMove(startPos.position, 0.5f).OnComplete(() =>
            {
                Debug.Log(HopNo + " hopNo");
                controller.DORotate(new Vector3(0, -90, 0), 0.1f);
                Destroy(StoneMovement.currentStone);
                RemoveBool();
                anim.SetBool("FailWalk", false);
                NoToMoveOn = 1;
            });
        });
        
    }
   
    void Update()
    {
        if(testing)
            HopNo = tempHopNumber;

        if (StopMovement)
        {
            return;
        }
            
        if(isHopping)
        {
            //if (!activateBar)
            //{
            //    bar.StartMoving();
            //    activateBar = true;
            //}
            if (Input.GetKey(KeyCode.Space))
            {
                jumpType = bar.currentBar;
                if(jumpType == BarColor.SkipJump && temp==true )
                {
                    if (ascend)
                        NoToMoveOn++;
                    if (descend)
                        NoToMoveOn--;


                    if (HopNo == 5 || HopNo == 8)
                        jumpType = BarColor.TwoLegJump;
                    else
                        jumpType = tempJumpType;
                    temp = false;
                }
            }
        }
        
        if ( Input.GetKey(KeyCode.Alpha1))
        {
            jumpType = BarColor.OneLegJump;
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            jumpType = BarColor.TwoLegJump;
        }

        if (jumpType == BarColor.OneLegJump && !oneLegHop && isHopping)
        {
            IsAiming = false;
            oneLegHop = true;

            Debug.Log("Jumpp");
            if (oneLegHop)
            {
                anim.SetTrigger("oneLeg");

                if (ascend)
                {
                    controller.DORotate(new Vector3(0, -90, 0), 0.1f);
                    controller.DOLocalJump(boxes[NoToMoveOn - 1].position, 0.5f, 1, 0.8f).OnComplete(() =>
                    {
                        NoToMoveOn++;
                        if (NoToMoveOn > boxes.Count)
                        {
                            ascend = false;
                            descend = true;

                            NoToMoveOn--;
                            controller.DORotate(controller.transform.rotation.eulerAngles + new Vector3(0, 180, 0), 0.1f);
                        }
                        temp = true;
                        jumpType = BarColor.None;
                        oneLegHop = false;
                       
                        Debug.Log(NoToMoveOn);
                    });
                }
                else if (descend)
                {
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
                            Destroy(StoneMovement.currentStone);
                            RemoveBool();
                            anim.SetTrigger("Idle");
                        });
                        NoToMoveOn++;
                        jumpType = BarColor.None;
                        oneLegHop = false;

                        Debug.Log(NoToMoveOn);
                    }
                    else
                    {
                        controller.DOLocalJump(boxes[NoToMoveOn - 1].position, 0.5f, 1, 0.8f).OnComplete(() =>
                        {
                            NoToMoveOn--;
                            if (NoToMoveOn < 1 && !HasReset)
                            {
                                ascend = true;
                                descend = false;
                                isHopping = false;
                                bar.KillNow();
                                activateBar = false;
                                controller.DOLocalJump(startPos.position, 0.5f, 1, 0.8f).OnComplete(() =>
                                {
                                    controller.DORotate(controller.transform.rotation.eulerAngles + new Vector3(0, -180, 0), 0.1f);
                                    Destroy(StoneMovement.currentStone);
                                    RemoveBool();
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
        }

        
        if (jumpType == BarColor.TwoLegJump && !twoLegHop && isHopping)
        {
            IsAiming = false;
            twoLegHop = true;

            Debug.Log("Jumpp");
            if (twoLegHop)
            {

                anim.SetTrigger("twoLeg");
                if (ascend)
                {
                    controller.DORotate(new Vector3(0, -90, 0), 0.1f);
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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition * 1.5f);
            Debug.DrawRay(ray.origin, ray.direction, Color.red);
            if (p.Raycast(ray, out Dist) && Input.GetMouseButton(1))
            {
                Vector3 Dir = ray.GetPoint(Dist) - transform.position;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Dir), Time.deltaTime * 3f);
            }
        }
       

    }
}