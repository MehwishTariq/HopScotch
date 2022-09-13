using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum DifficultyType
{
    VeryEasy,
    Easy,
    Normal,
    Hard,
    VeryHard
}
[Serializable]
public class Difficulty
{
    
    public DifficultyType difficulyType;
    [Range(1, 100)]
    public int chancesOfWin;

}
public class AIScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Difficulty difficulty;
    Difficulty resetDifficulty = new Difficulty();
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

    [SerializeField]
    public GameObject Stone;
    public static GameObject thrownStone;

    [SerializeField]
    public Transform ShotPoint;

    public int currentStoneLocation;

    //public void Throw(Transform BoxToJump)
    //{
    //    animator.Play("Throw", 0);
    //    GameObject CreatedStone = Instantiate(Stone, ShotPoint.transform.position, Quaternion.identity);
    //    CreatedStone.transform.DOMove(BoxToJump.position, 0.3f);
    //    thrownStone = CreatedStone;
    public void ResetDifficulty()
    {
        difficulty.difficulyType = resetDifficulty.difficulyType;
        difficulty.chancesOfWin = resetDifficulty.chancesOfWin;
    }
    void Start()
    {
        resetDifficulty.difficulyType = difficulty.difficulyType;
        resetDifficulty.chancesOfWin = difficulty.chancesOfWin ;
    }
    public void Throw(Transform BoxToJump,bool destroy)
    {
        animator.Play("Throw", 0);
        GameObject CreatedStone = Instantiate(Stone, ShotPoint.transform.position, Quaternion.identity);
        CreatedStone.transform.DOMove(BoxToJump.position, 0.3f);
        if (destroy)
            Destroy(CreatedStone, 2f);
        else
            thrownStone = CreatedStone;
        
    }

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
    public void WalkToStart(NayaTree tree)
    {
        boxCollider.enabled = false;
        transform.DORotate(transform.eulerAngles + new Vector3(0, 180, 0), 0.5f).OnComplete(() =>
        {
            animator.SetTrigger("FailWalk");
            if(thrownStone!=null)
                Destroy(thrownStone);
            transform.DOMove(tree.transform.position, 1.5f).OnComplete(() =>
            {
                transform.DORotate(transform.eulerAngles + new Vector3(0, 180, 0), 0.5f).OnComplete(()=>{
                    backward = false;
                    forward = true;
                    animator.SetTrigger("Idle");
                    tree.grid.ResetGridToDefault();
                    StartCoroutine(tree.grid.ThrowStone());
                    //boxCollider.enabled = true;
                });
               
                
            });
        });
    }

    
}
