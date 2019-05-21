using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Orbit : MonoBehaviour
{
    public float speed = 8f;
    public float distance = 3f;
    public Transform target;
    private Vector2 input;
    private float thing;
    public Rigidbody rb;

    //void Start() { rb = this.GetComponents<Rigidbody>; }

    void Update()
    {

        thing = Input.GetAxis("Mouse Y");
        if (Input.GetButton("Fire2"))
        {
            input += new Vector2(Input.GetAxis("Mouse X") * speed, thing * speed);
        }
        Quaternion rotation = Quaternion.Euler(input.y, input.x, 0);
        Vector3 position = target.position - (rotation * Vector3.forward * distance);
        transform.localRotation = rotation;
        transform.localPosition = position;
    }
}