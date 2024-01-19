using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class DetailComponent : MonoBehaviour, IBoundable
{
    [SerializeField] private Transform disassemblePosition;
    public string detailComponentName;

    private Vector3 defaultPosition;
    private Quaternion defaultRotation;

    private Renderer rendererComponent;
    private static readonly int Dissolve = Shader.PropertyToID("_Dissolve");

    public Vector3 DisassemblePosition => disassemblePosition.position;
    public Detail Detail { get; private set; }
    
    private void Awake()
    {
        defaultPosition = transform.position;
        defaultRotation = transform.rotation;

        rendererComponent = GetComponent<Renderer>();
        rendererComponent.material = new Material(rendererComponent.material);
        
        DetailCameraController.Instance.OnFocus.AddListener(DetailCameraController_OnFocus);
    }
    

    public void AssignDetail(Detail detail)
    {
        Detail = detail;
    }

    public void Disassemble(float time)
    {
        transform.DOMove(disassemblePosition.position, time);
        transform.DORotateQuaternion(disassemblePosition.rotation, time);
    }

    public void Assemble(float time)
    {
        transform.DOMove(defaultPosition, time);
        transform.DORotateQuaternion(defaultRotation, time);
    }

    public Bounds GetBounds()
    {
        return rendererComponent.bounds;
    }
    
    private void DetailCameraController_OnFocus(Transform target, float time)
    {
        Material material = rendererComponent.material;
        
        if (target.TryGetComponent(out DetailComponent detailComponent))
        {
            material.DOFloat(detailComponent != this ? 1 : 0, Dissolve, time);
        }
        else
        {
            material.DOFloat(0, Dissolve, time);
        }
    }
}
