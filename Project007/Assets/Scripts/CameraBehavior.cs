using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    [SerializeField] float followSpeed = 5.0f;
    [SerializeField] Transform player;

    private Vector3 offSet;

    private void Start()
    {
        offSet = transform.position - player.position;
    }

    private void Update()
    {
        Vector3 currPos = transform.position;
        Vector3 finalPos = Vector3.Lerp(currPos, player.position + offSet, Time.deltaTime * followSpeed);
        transform.position = finalPos;
    }
}
