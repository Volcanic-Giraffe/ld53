using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAlign : MonoBehaviour
{
    private Rigidbody rb;
    public Rigidbody RB { get => rb; }
    [SerializeField] bool AlignToVelocity = true;
    [SerializeField] float AlignSpeed = 100f;
    [SerializeField] bool ReorientToClosePlanet = true;
    [SerializeField, Tooltip("If closer than this to planet, and not going too fast - reorient to planet")] float MinReorientDistance = 0.5f;
    [SerializeField] float MinReorientSpeed = 0.5f;

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
        if (LevelScenario.IsCompleted || LevelScenario.IsPaused) return;
        var alignTarget = Vector3.zero;
        if (AlignToVelocity) alignTarget = RB.velocity;
        if (ReorientToClosePlanet)
        {
            if (RB.velocity.magnitude < MinReorientSpeed
            && Vector3.Distance(_ship.ClosestPlanet.transform.position, transform.position) < MinReorientDistance)
            {
                alignTarget = -(_ship.ClosestPlanet.transform.position - transform.position);
            }
        }

        var tForward = Input.GetAxis("Thrust Forward");
        var tBackwards = Input.GetAxis("Thrust Backwards");
        var joyInput = tForward != 0 || tBackwards != 0;
        
        if (alignTarget != Vector3.zero && !Input.GetMouseButton(1) && !Input.GetMouseButton(0) && !Input.GetKey(KeyCode.LeftShift) && !joyInput)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(alignTarget, _cam.transform.up), Time.fixedDeltaTime * AlignSpeed);
        }
    }
}
