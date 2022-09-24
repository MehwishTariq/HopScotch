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

    [SerializeField]
    public bool disableInput = false;

    public TimeManager time;

    void OnEnable()
    {
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
    public void SetMaterial(Transform current,bool canSet)
    {
        foreach (var item in boxes)
        {
            item.gameObject.GetComponent<MeshRenderer>().material = GameManager.instance.defaultMat;
        }
        if(canSet)
            current.gameObject.GetComponent<MeshRenderer>().material = GameManager.instance.selectedMat;
    }
    void SetPlayer()
    {
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
        ascend = true;
        descend = false;
        isHopping = false;
        oneLegHop = false;
        twoLegHop = false;
        HasReset = true;
        jumpType = BarColor.None;
        GameManager.instance.wrongJump = false;
        controller.DORotate(new Vector3(0, 90, 0), 0.1f).OnComplete(() =>
        {
            controller.DOMove(startPos.position, 0.5f).OnComplete(() =>
            {
                controller.DORotate(new Vector3(0, -90, 0), 0.1f);
                Destroy(StoneMovement.currentStone);
                
                 SetMaterial(null, false);
                RemoveBool();
                anim.SetBool("FailWalk", false);
                NoToMoveOn = 1;
            });
        });
        
    }

    bool canSkip;
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

                if (NoToMoveOn == HopNo)
                    canSkip = true;
                else
                    canSkip = false;
                //if (!activateBar)
                //{
                //    bar.StartMoving();
                //    activateBar = true;
                //}
                if (Input.GetKey(KeyCode.Space))
                {
                    
                    if (canSkip)
                    {
                        disableInput = true;
                        
                        jumpType = BarColor.SkipJump;// bar.currentBar;
                        if (jumpType == BarColor.SkipJump && temp == true)
                        {
                            if (ascend)
                                NoToMoveOn++;
                            if (descend)
                                NoToMoveOn--;

                            AudioManager.instance.Play("Hop");

                            if (HopNo == 5 || HopNo == 8)
                                jumpType = BarColor.TwoLegJump;
                            else
                                jumpType = tempJumpType;
                            temp = false;

                            GameManager.instance.SetRemarks(false, true);
                        }
                    }
                    else
                    {
                            if (ascend)
                                NoToMoveOn++;
                            if (descend)
                                NoToMoveOn--;
                            disableInput = true;
                            anim.SetTrigger("oneLeg");
                            controller.DOLocalJump(boxes[NoToMoveOn - 1].position, 0.5f, 1, 0.8f).OnComplete(() =>
                            {
                                disableInput = false;
                                if (!GameManager.instance.wrongJump)
                                {
                                    AudioManager.instance.Play("Wrong");
                                    GameManager.instance.wrongJump = true;
                                    time.AddTimeOnFoul();
                                    GameManager.instance.SetRemarks(true, true);
                                    anim.SetBool("FailWalk", true);
                                    ResetPlayerToStart();
                                   
                                   
                                }
                            });
                            return;
                        //LevelFail
                    }
                    
                }
            }

            if ((NoToMoveOn == 6 || NoToMoveOn == 3 || NoToMoveOn == 9))
            {
                if (!boxes[NoToMoveOn].GetComponent<ImBOX>().stoneHere)
                {
                    JumpCheck = BarColor.TwoLegJump;

                    
                }
                else
                {
                    
                    JumpCheck = BarColor.OneLegJump;
                }
            } 
            else if (NoToMoveOn == 1 || NoToMoveOn == 2 || NoToMoveOn == 5 || NoToMoveOn ==8)
            {
                if (!boxes[NoToMoveOn - 1].GetComponent<ImBOX>().stoneHere)
                {
                    
                    JumpCheck = BarColor.OneLegJump;
                }
                
            }
            else if (NoToMoveOn == 7 || NoToMoveOn == 4 || NoToMoveOn==10)
            {
                if (!boxes[NoToMoveOn - 2].GetComponent<ImBOX>().stoneHere)
                {
                    JumpCheck = BarColor.TwoLegJump;

                }
                else
                {

                    JumpCheck = BarColor.OneLegJump;
                }
            }
               



            if (Input.GetKey(KeyCode.Alpha1))
            {
                disableInput = true;
                twoLegHop = false;
                jumpType = BarColor.OneLegJump;
                if (jumpType != JumpCheck && !GameManager.instance.wrongJump)
                {
                    AudioManager.instance.Play("Wrong");
                    GameManager.instance.wrongJump = true;
                    time.AddTimeOnFoul();
                    GameManager.instance.SetRemarks(true, true);
                    anim.SetBool("FailWalk", true);
                    ResetPlayerToStart();
                    
                }
            }
            if (Input.GetKey(KeyCode.Alpha2))
            {
                disableInput = true;
                oneLegHop = false;
                jumpType = BarColor.TwoLegJump;
                if (jumpType != JumpCheck && !GameManager.instance.wrongJump)
                {
                    AudioManager.instance.Play("Wrong");

                    GameManager.instance.wrongJump = true;
                    anim.SetBool("FailWalk", true);
                    ResetPlayerToStart();
                    
                    time.AddTimeOnFoul();
                    GameManager.instance.SetRemarks(true, true);
                }
            }

            
            if (jumpType == BarColor.OneLegJump && !oneLegHop && isHopping)
            {
                IsAiming = false;
                oneLegHop = true;

                
                if (oneLegHop)
                {
                    anim.SetTrigger("oneLeg");
                    GameManager.instance.SetRemarks(false, true);
                    if (ascend)
                    {
                        controller.DORotate(new Vector3(0, -90, 0), 0.1f);
                        controller.DOLocalJump(boxes[NoToMoveOn - 1].position, 0.5f, 1, 0.8f).OnComplete(() =>
                        {
                            AudioManager.instance.Play("Hop");
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
                            
                            
                        });
                    }
                    else if (descend)
                    {
                        if (NoToMoveOn < 1)
                        {
                            ascend = true;
                            descend = false;
                            isHopping = false;
                            controller.DOLocalJump(startPos.position, 0.5f, 1, 0.8f).OnComplete(() =>
                            {
                                AudioManager.instance.Play("Hop");
                                controller.DORotate(controller.transform.rotation.eulerAngles + new Vector3(0, -180, 0), 0.1f);
                                AudioManager.instance.Play("RoundComplete");
                                HopNo++;
                                Destroy(StoneMovement.currentStone);
                                SetMaterial(startPos, false);
                                RemoveBool();
                                anim.SetTrigger("Idle");
                            });
                            
                            NoToMoveOn++;
                            jumpType = BarColor.None;
                            oneLegHop = false;
                            disableInput = false;
                           
                        }
                        else
                        {
                            controller.DOLocalJump(boxes[NoToMoveOn - 1].position, 0.5f, 1, 0.8f).OnComplete(() =>
                            {
                                AudioManager.instance.Play("Hop");
                                NoToMoveOn--;
                                if (NoToMoveOn < 1 && !HasReset)
                                {
                                    ascend = true;
                                    descend = false;
                                    isHopping = false;
                                    controller.DOLocalJump(startPos.position, 0.5f, 1, 0.8f).OnComplete(() =>
                                    {
                                        controller.DORotate(controller.transform.rotation.eulerAngles + new Vector3(0, -180, 0), 0.1f).OnComplete(()=> {

                                            jumpType = BarColor.None;
                                            oneLegHop = false;

                                            
                                            disableInput = false;
                                            Destroy(StoneMovement.currentStone);
                                            SetMaterial(startPos.transform, false);
                                            RemoveBool();
                                            if (HopNo >= boxes.Count)
                                            {
                                                

                                                AudioManager.instance.Play("Win");
                                                EventManager.instance.GameWin();

                                            }
                                            else
                                            {
                                                AudioManager.instance.Play("RoundComplete");
                                                HopNo++;
                                            }
                                            anim.SetTrigger("Idle");
                                        });
                                       

                                        
                                    });
                                    NoToMoveOn++;
                                }
                                else
                                {
                                    jumpType = BarColor.None;
                                    oneLegHop = false;

                                    
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
                GameManager.instance.SetRemarks(false, true);
               
                if (twoLegHop)
                {

                    anim.SetTrigger("twoLeg");
                    if (ascend)
                    {
                        controller.DORotate(new Vector3(0, -90, 0), 0.1f);
                        controller.DOLocalJump((boxes[NoToMoveOn].position + boxes[NoToMoveOn - 1].position) / 2, 0.5f, 1, 0.8f).OnComplete(() =>
                        {
                            AudioManager.instance.Play("Hop");
                            NoToMoveOn += 2;
                            

                            if (NoToMoveOn > boxes.Count)
                            {
                                ascend = false;
                                descend = true;
                                NoToMoveOn -= 3;

                                controller.DORotate(controller.transform.rotation.eulerAngles + new Vector3(0, 180, 0), 0.1f).OnComplete(()=>
                                {
                                    
                                    jumpType = BarColor.None;
                                    twoLegHop = false;
                                    disableInput = false;
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
                            AudioManager.instance.Play("Hop");

                            NoToMoveOn -= 2;
                            if (NoToMoveOn - 1 < 0)
                            {
                                ascend = true;
                                descend = false;
                                controller.DORotate(controller.transform.rotation.eulerAngles + new Vector3(0, -180, 0), 0.1f).OnComplete(()=>{

                                    jumpType = BarColor.None;
                                    twoLegHop = false;
                                    disableInput = false;
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

                }
            }

            else if (IsAiming)
            {
                Plane p = new Plane(Vector3.up, 0f);
                float Dist;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition * 1.5f);
                //Debug.DrawRay(ray.origin, ray.direction, Color.red);
                if (p.Raycast(ray, out Dist) && Input.GetMouseButton(1))
                {
                    Vector3 Dir = ray.GetPoint(Dist) - transform.position;
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Dir), Time.deltaTime * 3f);
                }
            }

        }
    }
}