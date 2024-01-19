using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DetailCameraController : Singleton<DetailCameraController>
{
    [SerializeField] private FocusCamera focusCamera;
    [SerializeField] private Transform defaultTarget;

    public Transform CurrentTarget { get; private set; }
    
    public UnityEvent<Transform, float> OnFocus;
    
    private IEnumerator Start()
    {
        yield return null;
        
        Focus(defaultTarget, 0);
    }

    public void Focus(Transform target, float time)
    {
        focusCamera.Focus(target, time);

        CurrentTarget = target;
        
        OnFocus?.Invoke(target, time);
    }
}
