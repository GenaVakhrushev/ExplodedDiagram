using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Camera))]
public abstract class FocusCamera : MonoBehaviour
{
    protected Camera cameraComponent;
    
    public UnityEvent<Transform, float> OnFocus;

    private void Awake()
    {
        cameraComponent = GetComponent<Camera>();
    }

    public virtual void Focus(Transform target, float time)
    {
        OnFocus?.Invoke(target, time);
    }
}
