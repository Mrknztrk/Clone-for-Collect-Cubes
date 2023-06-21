using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeAroundBase : MonoBehaviour
{
    //This is for the cube movement around the base.
    [SerializeField] private Transform magnetBase;
    [SerializeField] private float magnetPower;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponentInChildren<Rigidbody>();
    }
    private void FixedUpdate()
    {
        //Rotates several cubes around the base.
        rb.AddForce((magnetBase.transform.position - rb.position) * magnetPower * Time.fixedDeltaTime);
    }
}
