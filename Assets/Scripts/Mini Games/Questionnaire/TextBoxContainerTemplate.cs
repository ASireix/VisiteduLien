using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBoxContainerTemplate : TextBoxContainer
{
    public void UpdateContainer(TextBoxContainer container)
    {
        List<TextBoxContainer> containersToChanges = new List<TextBoxContainer>();
        List<TextBoxContainer> myContainer = new List<TextBoxContainer>();
        GetAllContainers(myContainer, this);
        GetAllContainers(containersToChanges, container);

        for(int i = 0; i < myContainer.Count; i++){
            if (containersToChanges[i]){
                containersToChanges[i].CopyContainer(myContainer[i]);
            }
        }
    }
    

    void GetAllContainers(List<TextBoxContainer> list, TextBoxContainer parent)
    {
        TextBoxContainer[] childs = parent.GetComponentsInChildren<TextBoxContainer>();

        if (childs.Length == 0)
        {
            list.Add(parent);
        }

        for (int i = 0; i < childs.Length; i++)
        {
            list.Add(childs[i]);
        }
    }
}
