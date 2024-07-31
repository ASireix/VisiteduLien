using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(Rigidbody))]
public class TouchRotation : PressInputBase
{
    [SerializeField] float speed;
    [SerializeField] float maxRotationZ;
    bool _isPressed;
    public int xFactor;
    public int yFactor;

    Rigidbody rb;
    Vector3 startRot = Vector3.zero;
    RectTransform rect;
    Vector3 startPos;
    private void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isPressed)
        {
            rb.AddRelativeTorque(_axisAction.ReadValue<Vector2>().x * speed * Time.deltaTime * Vector3.down);
            //rb.AddTorque(Vector3.right * _axisAction.ReadValue<Vector2>().y * speed * Time.deltaTime);
        }

        Vector3 localVelocity = transform.InverseTransformDirection(rb.velocity);
        localVelocity.x = 0;
        localVelocity.z = 0;

        rb.velocity = transform.TransformDirection(localVelocity);
    }

    protected override void OnPress(Vector3 position)
    {
        base.OnPress(position);
        _isPressed = true;
        rb.angularVelocity = Vector3.zero;
    }

    protected override void OnPressCancel()
    {
        base.OnPressCancel();
        _isPressed = false;
    }
    
    protected override void OnEnable(){
        base.OnEnable();

        Init();
        
        //rb.AddTorque(Vector3.right * initialTorque * Time.deltaTime);
        //rb.AddRelativeTorque(initialTorque * Time.deltaTime * Vector3.down);
    }

    void Init()
    {
        if (!rb) rb = GetComponent<Rigidbody>();
        if (!rect) rect = GetComponent<RectTransform>();

        if (startRot.Equals(Vector3.zero))
        {
            Quaternion worldRotation = transform.parent.rotation * transform.localRotation;
            startRot = worldRotation.eulerAngles;
        }

        if (startPos == null || startPos.Equals(Vector3.zero))
        {
            startPos = rect.position;
        }

        rb.position = startPos;
        rb.rotation = Quaternion.Euler(startRot);
    }

}
