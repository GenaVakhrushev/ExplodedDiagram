using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class OrbitalCamera : FocusCamera
{
    [SerializeField] private float sensitivity;

    private Vector2 previousMousePosition;
    private bool canRotate;
    
    private Transform focusTarget;
    private Bounds? targetBounds;

    private void Update()
    {
        bool mouseOverUI = EventSystem.current.IsPointerOverGameObject();
        Vector2 mousePosition = Input.mousePosition;

        if (!mouseOverUI && Input.GetMouseButtonDown(0))
        {
            canRotate = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            canRotate = false;
        }

        if (canRotate)
        {
            Vector2 deltaPixels = mousePosition - previousMousePosition;
            Rotate(deltaPixels);
        }

        previousMousePosition = mousePosition;
    }

    public override void Focus(Transform target, float time)
    {
        focusTarget = target;
        targetBounds = target.GetComponent<IBoundable>()?.GetBounds();
        
        MoveToTargetBounds(time);

        Vector3 focusPosition = GetFocusPosition();
        transform.DOLookAt(focusPosition, time);
        
        base.Focus(target, time);
    }

    private void MoveToTargetBounds(float time)
    {
        if (targetBounds == null)
        {
            return;
        }
        
        Bounds bounds = targetBounds.Value;
        float halfDiagonal = bounds.size.magnitude / 2;
        float aspect = cameraComponent.aspect;
        float fieldOfView = cameraComponent.fieldOfView;
        float minFieldOfView = aspect >= 1 ? fieldOfView : fieldOfView * aspect;
        float distance = halfDiagonal / Mathf.Sin(Mathf.Deg2Rad * minFieldOfView / 2);
        
        Vector3 focusPosition = GetFocusPosition();
        Vector3 toCamera = (transform.position - focusPosition).normalized;
        Vector3 targetPosition = focusPosition + toCamera * distance;

        transform.DOMove(targetPosition, time);
    }

    private Vector3 GetFocusPosition() => targetBounds?.center ?? focusTarget.position;
    
    private void Rotate(Vector2 deltaPixels)
    {
        Vector2 deltaNormalized = new Vector2(deltaPixels.x / Screen.width, deltaPixels.y / Screen.height);
        Vector2 angles = deltaNormalized * (sensitivity * 360);
        Vector3 focusPosition = GetFocusPosition();
        Vector3 targetToThis = transform.position - focusPosition;
        float angleToUp = Vector3.Angle(Vector3.up, targetToThis);
        
        if (angles.y < 0 && -angles.y > angleToUp)
        {
            angles.y = -angleToUp;
        }
        else if (angles.y > 0 && angleToUp + angles.y > 180)
        {
            angles.y = 180 - angleToUp;
        }

        Vector3 verticalAxis = Vector3.Cross(Vector3.up, targetToThis);
        verticalAxis = verticalAxis.magnitude < 0.01f ? -transform.right : verticalAxis;
        transform.RotateAround(focusPosition, Vector3.up, angles.x);
        transform.RotateAround(focusPosition, verticalAxis, angles.y);
    }
}
