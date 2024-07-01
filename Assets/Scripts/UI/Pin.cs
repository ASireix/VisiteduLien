using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pin : MonoBehaviour
{
    [SerializeField] float rotationSpeed;
    [SerializeField] [ColorUsage(true,true)]Color color;
    [SerializeField] bool useColorAtStart;
    // Start is called before the first frame update
    void Start()
    {
        if (useColorAtStart)
        {
            GetComponent<Renderer>().materials[0].SetColor("_Color", color);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * rotationSpeed);
    }
}
