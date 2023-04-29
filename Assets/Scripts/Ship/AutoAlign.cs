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
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(alignTarget, Vector3.up), Time.fixedDeltaTime * AlignSpeed);
    }
}
