using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateOwner<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private T template;
    [SerializeField] private Transform container;

    protected List<T> objects = new List<T>();

    protected virtual void Awake()
    {
        ClearContainer();
        
        template.gameObject.SetActive(false);
    }

    public void ClearContainer()
    {
        foreach (T obj in objects)
        {
            Destroy(obj.gameObject);
        }
        
        objects.Clear();
    }

    public T SpawnTemplate()
    {
        T spawnedTemplate = Instantiate(template, container);
        spawnedTemplate.gameObject.SetActive(true);
        objects.Add(spawnedTemplate);

        return spawnedTemplate;
    }

    public void RemoveObject(T obj)
    {
        objects.Remove(obj);
        
        Destroy(obj.gameObject);
    }

    public void ReplaceObject(T old, T newObj)
    {
        int oldIndex = objects.IndexOf(old);
        int oldSiblingIndex = old.transform.GetSiblingIndex();
        objects.RemoveAt(oldIndex);
        objects.Remove(newObj);
        Destroy(old.gameObject);
        objects.Insert(oldIndex, newObj);
        newObj.transform.SetSiblingIndex(oldSiblingIndex);
    }
}
