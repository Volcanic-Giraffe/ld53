using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignToCamera : MonoBehaviour
{
    private Rigidbody rb;
    public Rigidbody RB { get => rb; }
    [SerializeField] bool Auto = true;
    [SerializeField] float AlignSpeed = 100f;
    [SerializeField] int MouseButton = 1;

    private CameraController _cam;
    private Ship _ship;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        _cam = FindObjectOfType<CameraController>();
        _ship = GetComponent<Ship>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_ship.Standby) return;
        var alignTarget = rb.position - _cam.transform.position;
        
        if ((Auto || Input.GetMouseButton(MouseButton)) && alignTarget != Vector3.zero)
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(alignTarget, Vector3.up), Time.fixedDeltaTime * AlignSpeed);
    }
}
