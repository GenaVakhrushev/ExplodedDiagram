using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DetailPanel : TemplateOwner<DetailComponentPanel>
{
    [SerializeField] private float switchStateTime;
    [SerializeField] private float spacing;
    [Space] 
    [SerializeField] private Button disassembleButton;
    [SerializeField] private TMP_Text detailNameText;
    [SerializeField] private Detail detail;
    [Space] 
    [SerializeField] private Outline selectedOutline;

    protected override void Awake()
    {
        base.Awake();

        disassembleButton.onClick.AddListener(SwitchDetailState);
        detailNameText.text = detail.detailName;
        
        DetailCameraController.Instance.OnFocus.AddListener(DetailCameraController_OnFocus);
    }

    private void Start()
    {
        foreach (DetailComponent detailComponent in detail.Components)
        {
            DetailComponentPanel detailComponentPanel = SpawnTemplate();
            detailComponentPanel.SetDetailComponent(detailComponent);
        }
    }

    private void DetailCameraController_OnFocus(Transform target, float time)
    {
        selectedOutline.enabled = target == detail.transform;
    }

    private void SwitchDetailState()
    {
        switch (detail.State)
        {
            case DetailState.Assembled:
                detail.SetState(DetailState.Disassembled, switchStateTime);
                ShowPanels();
                break;
            case DetailState.Disassembled:
                detail.SetState(DetailState.Assembled, switchStateTime);
                HidePanels();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        DetailCameraController.Instance.Focus(detail.transform, switchStateTime);
    }

    private void ShowPanels()
    {
        float accumulatedHeight = 0;
        foreach (RectTransform rectTransform in objects.Select(detailComponentPanel => (RectTransform)detailComponentPanel.transform))
        {
            accumulatedHeight += rectTransform.rect.height;
            rectTransform.DOAnchorPosY(-accumulatedHeight, switchStateTime);
            accumulatedHeight += spacing;
        }
    }

    private void HidePanels()
    {
        foreach (RectTransform rectTransform in objects.Select(detailComponentPanel => (RectTransform)detailComponentPanel.transform))
        {
            rectTransform.DOAnchorPosY(0, switchStateTime);
        }
    }
}
