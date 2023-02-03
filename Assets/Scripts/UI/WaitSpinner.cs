//-----------------------------------------------------------------------
// WaitSpinner.cs
//
// Copyright 2020 Social Point SL. All rights reserved.
//
//-----------------------------------------------------------------------
using UnityEngine;

public class WaitSpinner : MonoBehaviour
{
    [SerializeField] Vector3 _angularVelocity = Vector3.zero;

    void Update()
    {
        transform.rotation *= Quaternion.Euler(_angularVelocity * Time.deltaTime);
    }

}
