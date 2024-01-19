using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Detail : MonoBehaviour, IBoundable
{
    public string detailName;
    
    public DetailState State { get; private set; }
    public DetailComponent[] Components { get; private set; }
    public Bounds AssembledBounds { get; private set; }
    public Bounds DisassembledBounds { get; private set; }
    

    public UnityEvent<DetailState> OnStateChanged;

    private void Awake()
    {
        Components = GetComponentsInChildren<DetailComponent>();

        foreach (DetailComponent detailComponent in Components)
        {
            detailComponent.AssignDetail(this);
        }
    }

    private void Start()
    {
        UpdateBounds();
    }
    
    public void SetState(DetailState newState, float time)
    {
        State = newState;
        
        switch (State)
        {
            case DetailState.Disassembled:
                Disassemble(time);
                break;
            case DetailState.Assembled:
                Assemble(time);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        OnStateChanged?.Invoke(State);
    }
    
    public Bounds GetBounds()
    {
        return State switch
        {
            DetailState.Disassembled => DisassembledBounds,
            DetailState.Assembled => AssembledBounds,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private void Disassemble(float time)
    {
        foreach (DetailComponent detailComponent in Components)
        {
            detailComponent.Disassemble(time);
        }
    }
    
    private void Assemble(float time)
    {
        foreach (DetailComponent detailComponent in Components)
        {
            detailComponent.Assemble(time);
        }
    }

    private void UpdateBounds()
    {
        DetailComponent firstDetailComponent = Components[0];
        Bounds firstBounds = firstDetailComponent.GetBounds();
        Bounds temp = firstBounds;
        firstBounds.center += firstDetailComponent.DisassemblePosition - firstDetailComponent.transform.position;
        Bounds temp1 = firstBounds;
        
        foreach (DetailComponent detailComponent in Components)
        {
            Bounds componentBounds = detailComponent.GetBounds();
            temp.Encapsulate(componentBounds);
            
            Bounds componentDisassembledBounds = componentBounds;
            componentDisassembledBounds.center +=
                detailComponent.DisassemblePosition - detailComponent.transform.position;
            temp1.Encapsulate(componentDisassembledBounds);
        }

        AssembledBounds = temp;
        DisassembledBounds = temp1;
    }
}
