using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorControl : MonoBehaviour
{
    private Rigidbody rb;
    public Rigidbody RB { get => rb; }
    [SerializeField] float StrafePower = 300f;
    [SerializeField, Tooltip("True - control in camera space, false - local space")] bool camSpace = true;
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
        if(camSpace)
        {
            RB.AddForce(_cam.transform.right * _strafe.x * StrafePower * Time.fixedDeltaTime);
            RB.AddForce(_cam.transform.up * _strafe.y * StrafePower * Time.fixedDeltaTime);
        } else
        {
            RB.AddRelativeForce(_strafe * StrafePower * Time.fixedDeltaTime);
        }
    }
}
