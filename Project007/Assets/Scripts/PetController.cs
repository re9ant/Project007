using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PetController : MonoBehaviour
{
    [SerializeField] private Transform target;
    private NavMeshAgent navAgent;
    private Animator myAnimator;

    private int speed_Hash;

    private void Awake()
    {
        try
        {
            navAgent = GetComponent<NavMeshAgent>();
        }
        catch
        {
            Debug.LogError("NO NAV MESH AGENT COMP IN PET");
        }
        
        try
        {
            myAnimator = GetComponent<Animator>();
        }
        catch
        {
            Debug.LogError("NO Animator COMP IN PET");
        }
    }

    private void Start()
    {
        speed_Hash = Animator.StringToHash("Speed");
    }

    private void Update()
    {
        ManageAnimation();
        ManageNavigation();
    }

    private void ManageAnimation()
    {
        myAnimator.SetFloat(speed_Hash, navAgent.velocity.magnitude);
    }

    private void ManageNavigation()
    {
        float remainingDistance = Vector3.Distance(transform.position, target.position);
        if (remainingDistance < 0.5f)
        {
            navAgent.isStopped = true;
        }
        else
        {
            navAgent.isStopped = false;
        }
        navAgent.SetDestination(target.position);
    }
}
