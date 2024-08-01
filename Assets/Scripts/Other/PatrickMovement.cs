using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class PatrickMovement : MonoBehaviour
{
    [Header("Positions")]
    [SerializeField] TransformData upPosition;
    [SerializeField] TransformData downPosition;
    [SerializeField] TransformData leftPosition;
    [SerializeField] TransformData rightPosition;
    [SerializeField] TransformData customPosition;

    [Header("References")]
    [SerializeField] Transform transformRef;
    [SerializeField] float speed = 2f;

    TransformData startPos;
    bool init;
    // Start is called before the first frame update
    void Start()
    {
        if (!init)
        {
            startPos = new TransformData(transformRef.localPosition, transformRef.localEulerAngles);
        }
        init = true;
    }

    public void MoveUp()
    {
        Move(upPosition);
    }

    public void MoveDown()
    {
        Move(downPosition);
    }

    public void MoveLeft()
    {
        Move(leftPosition);
    }

    public void MoveRight()
    {
        Move(rightPosition);
    }

    public void MoveCustom()
    {
        Move(customPosition);
    }

    public void Move(Vector3 pos, bool tween = true)
    {
        if (tween)
        {
            LeanTween.moveLocal(transformRef.gameObject, pos, speed);
        }
        else
        {
            transformRef.localPosition = pos;
        }
    }

    public void Move(TransformData transData, bool tween = true)
    {
        if (tween)
        {
            LeanTween.moveLocal(transformRef.gameObject, transData.Position, speed);
            LeanTween.rotateLocal(transformRef.gameObject, transData.Rotation, speed);
        }
        else
        {
            transformRef.SetLocalPositionAndRotation(transData.Position, Quaternion.Euler(transData.Rotation));
        }
    }

    private void OnEnable()
    {
        if (init)
        {
            Move(startPos, false);
        }
    }
}

[System.Serializable]
public class TransformData
{
    public Vector3 Position;
    public Vector3 Rotation;

    public TransformData (Vector3 Position, Vector3 Rotation)
    {
        this.Position = Position;
        this.Rotation = Rotation;
    }
}
