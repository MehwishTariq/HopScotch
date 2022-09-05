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
    public bool isJumping;
    float jumpAmount = 4;

    public List<Transform> boxes;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<Transform>();
        anim = GetComponentInChildren<Animator>();
        NoToThrowOn = 1;
    }

    IEnumerator Jump()
    {
        isJumping = true;
        //controller.GetComponent<Transform>().DOLocalJump(new Vector3(transform.localPosition.x, transform.localPosition.y + 0.2f, transform.localPosition.z), 0.3f, 1, 0.2f);
        yield return new WaitForSeconds(0.2f);
        //controller.GetComponent<Transform>().DOLocalMove(new Vector3(transform.localPosition.x - 0.8f, transform.localPosition.y - 0.2f, transform.localPosition.z), 0.2f).OnComplete(() =>
        //{
        //    isJumping = false;
        //});

       
    }
    // Update is called once per frame
    void Update()
    {
        if (StopMovement)
        {
            return;
        }
     
        if (Input.GetKey(KeyCode.O))
        {
            IsAiming = false;
            anim.Play("OneLegJump");
            //controller.AddForce(new Vector3(controller.position.x, Vector2.up.y * jumpAmount, 0), ForceMode.Impulse);
            isJumping = true;
        }
        if (isJumping)
        {
            isJumping = false;
            controller.DOLocalJump(boxes[NoToThrowOn - 1].position, 0.3f, 1, 0.6f).OnComplete(() =>
            {
                NoToThrowOn++;
                Debug.Log(NoToThrowOn);
            });
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