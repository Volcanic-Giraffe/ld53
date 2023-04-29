using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorControl : MonoBehaviour
{
    private Rigidbody rb;
    public Rigidbody RB { get => rb; }
    [SerializeField] float StrafePower = 300f;
    [SerializeField] float AlignSpeed = 100f;
    [SerializeField, Tooltip("True - forward, false - backward")] bool AlignMod = false;
    private Vector3 _strafe;

    private CameraController _cam;
    private Ship _ship;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        _cam = FindObjectOfType<CameraController>();
        _ship = GetComponent<Ship>();
    }

    private void Update()
    {
        _strafe = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_ship.Standby) return;
        RB.AddRelativeForce(_strafe * StrafePower * Time.fixedDeltaTime);
        
        var alignTarget = RB.velocity.magnitude > 1f ? RB.velocity : (_ship.ClosestPlanet.transform.position - transform.position);
        var sign = AlignMod ? 1f : -1f;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(sign * alignTarget, Vector3.up), Time.fixedDeltaTime * AlignSpeed);
    }
}
