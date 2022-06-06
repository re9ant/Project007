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
        if (lastPos == player.position)
        {
            idletimmer += Time.deltaTime;
            if (idletimmer >= 10.0f)
            {
                camAnimator.SetBool(idleHash, true);
                idletimmer = 10.0f;
            }
        }
        else
        {
            idletimmer = 0;
            camAnimator.SetBool(idleHash, false);
            lastPos = player.position;
        }
    }
}
