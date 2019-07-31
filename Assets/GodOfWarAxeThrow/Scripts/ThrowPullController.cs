using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowPullController : MonoBehaviour
{
    [Tooltip("Axe rotation speed in rotations/second")][SerializeField] private float m_rotationalSpeed;
    [SerializeField] private Transform m_kratosHand;
    [SerializeField] private Transform m_midPoint;
    private Camera m_main;
    private Vector3 m_originalLocalPosition;
    private Vector3 m_originalLocalRotation;
    private Rigidbody m_rigidbody;
    private float m_pullTimer;
    private Vector3 m_p0;
    private Vector3 m_p1;
    private Vector3 m_p2;
    private bool m_impaled;
    [HideInInspector] public bool pullAxe;
    [HideInInspector] public bool isThrown;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_rotationalSpeed *= Time.deltaTime;
        m_main = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (pullAxe)
        {
            m_rigidbody.isKinematic = true;
            m_p1 = m_midPoint.position;
            m_p2 = m_kratosHand.position;
            transform.position = HelperFunctions.CalculateQuadraticBezierPoint(m_pullTimer, m_p0, m_p1, m_p2);
            transform.Rotate(0, 0, 360 * m_rotationalSpeed);
            m_pullTimer += Time.fixedDeltaTime;
            if (m_pullTimer >= 1)
            {
                Attach();
                m_pullTimer = 1;
            }
        }
        else if (isThrown && !m_impaled)
        {
            transform.Rotate(0, 0, 360 * -m_rotationalSpeed);
        }
        else
            m_pullTimer = 0;
    }

    private void Attach()
    {
        isThrown = false;
        pullAxe = false;
        transform.parent = m_kratosHand;
        transform.localEulerAngles = m_originalLocalRotation;
        transform.localPosition = m_originalLocalPosition;
        m_pullTimer = 0;
    }

    public void ThrowAxe()
    {
        isThrown = true;
        m_originalLocalPosition = transform.localPosition;
        m_originalLocalRotation = transform.localEulerAngles;
        transform.parent = null;
        m_rigidbody.isKinematic = false;
        m_rigidbody.AddForce(m_main.transform.forward* 50, ForceMode.Impulse);
        m_rigidbody.angularVelocity = Vector3.zero;
    }

    public void PullAxe()
    {
        pullAxe = true;
        m_impaled = false;
        m_p0 = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        m_impaled = true;
        m_rigidbody.isKinematic = true;
    }
}
