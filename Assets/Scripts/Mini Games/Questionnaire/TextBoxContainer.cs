using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBoxContainer : MonoBehaviour
{
    [SerializeField]
    protected TextBox textBox;
    // Start is called before the first frame update
    void Start()
    {
        OnStart();
    }

    protected virtual void OnStart() {}

    public void UpdateSmokeVFX(GameObject vfx){
        textBox.smokeVFX = vfx;
    }

    public void UpdatePosition(Vector3 pos, bool worldSpace = true){
        if (worldSpace){
            textBox.transform.position = pos;
        }else{
            textBox.transform.localPosition = pos;
        }
    }

    public void UpdateRotation(Quaternion quat, bool worldSpace = true){
        if (worldSpace){
            textBox.transform.rotation = quat;
        }else{
            textBox.transform.localRotation = quat;
        }
    }

    public void UpdateTransform(Vector3 pos, Quaternion rot, Vector3 scale, bool worldSpace = true){
        transform.localScale = Vector3.one;
        if (worldSpace){
            textBox.transform.SetPositionAndRotation(pos, rot);
        }
        else{
            textBox.transform.SetLocalPositionAndRotation(pos, rot);
        }
        textBox.transform.localScale = scale;
    }

    public void CopyContainer(TextBoxContainer container){
        textBox.transform.SetPositionAndRotation(container.textBox.transform.position, container.textBox.transform.rotation);
        textBox.transform.localScale = container.textBox.transform.localScale;
    }
}
