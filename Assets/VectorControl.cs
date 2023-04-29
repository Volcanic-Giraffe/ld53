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
    private Vector2 _strafe;

    private CameraController _cam;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        _cam = FindObjectOfType<CameraController>();
    }

    private void Update()
    {
        _strafe = new Vector2(-Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RB.AddForce(_strafe * StrafePower * Time.fixedDeltaTime);
        if (RB.velocity.magnitude > 1f)
        {
            var sign = AlignMod ? 1f : -1f;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(sign * RB.velocity, Vector3.up), Time.fixedDeltaTime * AlignSpeed);
        }
    }
}
