using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DetailComponentPanel : MonoBehaviour
{
    [SerializeField] private float focusTime;
    [Space]
    [SerializeField] private Button focusButton;
    [SerializeField] private TMP_Text detailNameText;
    [Space] 
    [SerializeField] private Outline selectedOutline;
    
    private DetailComponent detailComponent;
    
    private void Awake()
    {
        focusButton.onClick.AddListener(ChangeFocus);
        
        DetailCameraController.Instance.OnFocus.AddListener(DetailCameraController_OnFocus);
    }

    public void SetDetailComponent(DetailComponent newDetailComponent)
    {
        detailComponent = newDetailComponent;
        detailNameText.text = detailComponent.detailComponentName;
    }
    
    private void ChangeFocus()
    {
        DetailCameraController cameraController = DetailCameraController.Instance;
        Transform detailComponentTransform = detailComponent.transform;
        Detail detail = detailComponent.Detail;
        bool focusDetail = cameraController.CurrentTarget == detailComponentTransform;
        Transform focusTransform = focusDetail
            ? detail.transform
            : detailComponentTransform;

        cameraController.Focus(focusTransform, focusTime);
    }
    
    private void DetailCameraController_OnFocus(Transform target, float time)
    {
        if (detailComponent == null)
        {
            return;
        }
        
        selectedOutline.enabled = target == detailComponent.transform;
    }
}
