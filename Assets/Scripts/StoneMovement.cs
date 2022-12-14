using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneMovement : MonoBehaviour
{
    public Animator anim;
    public float rotationSpeed = 1;
    public float BlastPower = 5;

    public GameObject Stone;
    public Transform ShotPoint;
    public static GameObject currentStone;

    void Update()
    {
        //if (GetComponentInParent<PlayerMovement>().StopMovement)
        //{
        //    return;
        //}
        if (GameManager.instance.gameStarted)
        {
            if (Input.GetMouseButton(1) && (!GetComponentInParent<PlayerMovement>().oneLegHop || !GetComponentInParent<PlayerMovement>().twoLegHop) && !PlayerMovement.isHopping)
            {
                GetComponentInParent<PlayerMovement>().IsAiming = true;
                float HorizontalRotation = Input.GetAxis("Mouse X");
                float VericalRotation = Input.GetAxis("Mouse Y");

                ShotPoint.rotation = Quaternion.Euler(ShotPoint.rotation.eulerAngles +
                    new Vector3(0, 0, VericalRotation * rotationSpeed));
                BlastPower = Mathf.Clamp(BlastPower + HorizontalRotation + VericalRotation * rotationSpeed, 0.05f, 0.07f);
                if (Input.GetMouseButtonDown(0))
                {
                    anim.Play("Throw", 0);
                    if (currentStone != null)
                        Destroy(currentStone.gameObject);
                    GameObject CreatedStone = Instantiate(Stone, ShotPoint.position, ShotPoint.rotation);
                    CreatedStone.GetComponent<Rigidbody>().velocity = GetComponent<ArcCreator>().Dir * BlastPower;
                    currentStone = CreatedStone;
                }
            }
            else
            {
                GetComponentInParent<PlayerMovement>().IsAiming = false;
            }
        }
    }
}