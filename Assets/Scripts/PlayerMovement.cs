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
    public BarColor JumpCheck = BarColor.None;

    [SerializeField]
    bool testing;
    [SerializeField]
    public bool HasReset;

    public List<int> twoNumbers = new List<int>();//{ 3, 4, 6, 7, 9, 10 };

    [SerializeField]
    public bool disableInput = false;

    void OnEnable()
    {

        twoNumbers.Add(3);
        twoNumbers.Add(4);
        twoNumbers.Add(6);
        twoNumbers.Add(7);
        twoNumbers.Add(9);
        twoNumbers.Add(10);
        EventManager.instance.onStart += SetPlayer;
        EventManager.instance.onWin += onGameDone;
        EventManager.instance.onFail += onGameDone;
    }

    private void OnDisable()
    {
        EventManager.instance.onStart -= SetPlayer;
        EventManager.instance.onWin -= onGameDone;
        EventManager.instance.onFail -= onGameDone;
    }

    public void onGameDone()
    {
        GameManager.instance.gameStarted = false;
        ResetPlayerToStart();
    }

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<Transform>();
        anim = GetComponentInChildren<Animator>();
    }

    void SetPlayer()
    {
        Debug.Log("setPlayer");
        HopNo = 1;
        NoToMoveOn = 1;
        ascend = true;
        descend = false;
        JumpCheck = BarColor.OneLegJump;
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
        GameManager.instance.wrongJump = false;
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
        if (testing)
            HopNo = tempHopNumber;

        if (disableInput)
            return; 


        if (GameManager.instance.gameStarted)
        {
            


            if (isHopping)
            {
                Debug.Log("hoppingtrue");
                //if (!activateBar)
                //{
                //    bar.StartMoving();
                //    activateBar = true;
                //}
                if (Input.GetKey(KeyCode.Space))
                {
                    disableInput = true;
                    Debug.Log("skipp");
                    jumpType = BarColor.SkipJump;// bar.currentBar;
                    if (jumpType == BarColor.SkipJump && temp == true)
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

            if ((NoToMoveOn == 6 || NoToMoveOn == 3 || NoToMoveOn == 9))
            {
                if (!boxes[NoToMoveOn].GetComponent<ImBOX>().stoneHere)
                {
                    JumpCheck = BarColor.TwoLegJump;

                    Debug.Log("SettingTwoLeg");
                }
                else
                {
                    Debug.Log("SettingOneLeg");
                    JumpCheck = BarColor.OneLegJump;
                }
            } 
            else if (NoToMoveOn == 1 || NoToMoveOn == 2 || NoToMoveOn == 5 || NoToMoveOn ==8)
            {
                if (!boxes[NoToMoveOn - 1].GetComponent<ImBOX>().stoneHere)
                {
                    Debug.Log("SettingOneLeg---Else");
                    JumpCheck = BarColor.OneLegJump;
                }
                
            }
            else if (NoToMoveOn == 7 || NoToMoveOn == 4 || NoToMoveOn==10)
            {
                if (!boxes[NoToMoveOn - 2].GetComponent<ImBOX>().stoneHere)
                {
                    JumpCheck = BarColor.TwoLegJump;

                    Debug.Log("SettingTwoLeg-----elseIF");
                }
                else
                {
                    Debug.Log("SettingOneLeg-----elseIF");
                    JumpCheck = BarColor.OneLegJump;
                }
            }
                //if (boxes[NoToMoveOn - 1].GetComponent<ImBOX>().stoneHere)
                //{
                //    if (twoNumbers.Contains(NoToMoveOn))
                //    {
                //        Debug.Log("CheckJump");
                //        JumpCheck = BarColor.OneLegJump;
                //    }
                //}

                //if (ascend)
                //{
                //    if (NoToMoveOn == 3 || 
                //        NoToMoveOn == 4 ||
                //        NoToMoveOn == 7 ||
                //        NoToMoveOn == 10 ||
                //        NoToMoveOn == 6 ||
                //        NoToMoveOn == 9)
                //    {
                //        if (HopNo == 4)
                //            JumpCheck = BarColor.OneLegJump;
                //        else
                //        {
                //            Debug.Log("CheckJump: " + NoToMoveOn);
                //            if (boxes[NoToMoveOn - 1].GetComponent<ImBOX>().stoneHere)
                //                JumpCheck = BarColor.OneLegJump;
                //            else
                //                JumpCheck = BarColor.TwoLegJump;
                //        }
                //    }
                //    else
                //        JumpCheck = BarColor.OneLegJump;
                //}




            if (Input.GetKey(KeyCode.Alpha1))
            {
                disableInput = true;
                twoLegHop = false;
                jumpType = BarColor.OneLegJump;
                if (jumpType != JumpCheck && !GameManager.instance.wrongJump)
                {
                    GameManager.instance.wrongJump = true;
                    anim.SetBool("FailWalk", true);
                    ResetPlayerToStart();
                    Debug.Log("wrongjump");
                }
            }
            if (Input.GetKey(KeyCode.Alpha2))
            {
                disableInput = true;
                oneLegHop = false;
                jumpType = BarColor.TwoLegJump;
                if (jumpType != JumpCheck && !GameManager.instance.wrongJump)
                {
                    GameManager.instance.wrongJump = true;
                    anim.SetBool("FailWalk", true);
                    ResetPlayerToStart();
                    Debug.Log("wrongjump2");
                }
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
                            if (NoToMoveOn >= boxes.Count)
                            {
                                ascend = false;
                                descend = true;

                                NoToMoveOn = NoToMoveOn - 2;
                                controller.DORotate(controller.transform.rotation.eulerAngles + new Vector3(0, 180, 0), 0.1f).OnComplete(()=> {

                                    temp = true;
                                    jumpType = BarColor.None;
                                    oneLegHop = false;
                                    disableInput = false;

                                });
                            }
                            else
                            {
                                temp = true;
                                jumpType = BarColor.None;
                                oneLegHop = false;
                                disableInput = false;
                            }
                            
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
                            disableInput = false;
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
                                        controller.DORotate(controller.transform.rotation.eulerAngles + new Vector3(0, -180, 0), 0.1f).OnComplete(()=> {

                                            jumpType = BarColor.None;
                                            oneLegHop = false;

                                            Debug.Log(NoToMoveOn);
                                            disableInput = false;
                                            Destroy(StoneMovement.currentStone);
                                            RemoveBool();
                                            if (HopNo > boxes.Count)
                                            {
                                                Debug.Log("GameWon");
                                                EventManager.instance.GameWin();

                                            }
                                            else
                                                HopNo++;
                                            anim.SetTrigger("Idle");
                                        });
                                       

                                        
                                    });
                                    NoToMoveOn++;
                                }
                                else
                                {
                                    jumpType = BarColor.None;
                                    oneLegHop = false;

                                    Debug.Log(NoToMoveOn);
                                    disableInput = false;
                                }
                                
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
                            Debug.Log("NoToMoveOn:" + NoToMoveOn);
                            Debug.Log("boxes.Count:" + boxes.Count);

                            if (NoToMoveOn > boxes.Count)
                            {
                                ascend = false;
                                descend = true;
                                NoToMoveOn -= 3;

                                controller.DORotate(controller.transform.rotation.eulerAngles + new Vector3(0, 180, 0), 0.1f).OnComplete(()=>
                                {
                                    Debug.Log("goBack");
                                    jumpType = BarColor.None;
                                    twoLegHop = false;
                                    disableInput = false;
                                    Debug.Log(NoToMoveOn);
                                    temp = true;

                                });
                            }
                            else
                            {
                                jumpType = BarColor.None;
                                twoLegHop = false;

                                disableInput = false;
                            }

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
                                controller.DORotate(controller.transform.rotation.eulerAngles + new Vector3(0, -180, 0), 0.1f).OnComplete(()=>{

                                    jumpType = BarColor.None;
                                    twoLegHop = false;
                                    Debug.Log(NoToMoveOn);
                                    disableInput = false;
                                });
                            }
                            else
                            {
                                jumpType = BarColor.None;
                                twoLegHop = false;
                                Debug.Log(NoToMoveOn);
                                disableInput = false;
                            }

                          
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
}