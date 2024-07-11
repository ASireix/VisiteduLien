using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrickMovement : MonoBehaviour
{
    [Header("Positions")]
    [SerializeField] Vector3 upPosition;
    [SerializeField] Vector3 downPosition;
    [SerializeField] Vector3 leftPosition;
    [SerializeField] Vector3 rightPosition;
    [SerializeField] Vector3 customPosition;

    [Header("References")]
    [SerializeField] Transform transformRef;
    [SerializeField] float speed = 2f;

    Vector3 startPos;
    bool init;
    // Start is called before the first frame update
    void Start()
    {
        if (!init)
        {
            startPos = transformRef.localPosition;
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

    private void OnEnable()
    {
        if (init)
        {
            Move(startPos, false);
        }
    }
}
