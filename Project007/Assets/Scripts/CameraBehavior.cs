using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    [SerializeField] float followSpeed = 5.0f;
    [SerializeField] Transform player;
    [SerializeField] Animator camAnimator;

    private int idleHash;
    private Vector3 offSet;

    private Vector3 lastPos;
    private float idletimmer;

    private void Start()
    {
        offSet = transform.position - player.position;
        idleHash = Animator.StringToHash("Idle");
        lastPos = player.position;
    }

    private void Update()
    {
        Vector3 currPos = transform.position;
        Vector3 finalPos = Vector3.Lerp(currPos, player.position + offSet, Time.deltaTime * followSpeed);
        transform.position = finalPos;
        transform.LookAt(player);
        //if (lastPos == player.position)
        //{
        //    idletimmer += Time.deltaTime;
        //    if (idletimmer >= 10.0f)
        //    {
        //        camAnimator.SetBool(idleHash, true);
        //        idletimmer = 10.0f;
        //    }
        //}
        //else
        //{
        //    idletimmer = 0;
        //    camAnimator.SetBool(idleHash, false);
        //    lastPos = player.position;
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (other.TryGetComponent<CameraPositions>(out CameraPositions camPos))
        {
            Debug.Log("Hi");
            ChangePos(camPos);
        }
    }

    void ChangePos(CameraPositions closestCam)
    {
        Debug.Log("Hi");
        //Collider[] colls = Physics.OverlapSphere(transform.position, 5.0f);
        //CameraPositions[] camPos = new CameraPositions[3];
        //int count = 0;
        //float distance = 999.0f;
        //CameraPositions closestCam;
        //foreach (Collider coll in colls)
        //{
        //    if (coll.TryGetComponent<CameraPositions>(out CameraPositions cam))
        //    {
        //        if (count < 4)
        //        {
        //            camPos[count] = cam;
        //            count++;
        //        }
        //    }
        //}
        //closestCam = camPos[0];
        //distance = Vector3.Distance(transform.position, closestCam.transform.position);
        //for (int i = 0; i < count + 1; i++)
        //{
        //    if (distance > Vector3.Distance(transform.position, camPos[i].transform.position))
        //    {
        //        distance = Vector3.Distance(transform.position, camPos[i].transform.position);
        //        closestCam = camPos[i];
        //    }
        //}
        offSet = closestCam.transform.position - player.position;
    }
}
