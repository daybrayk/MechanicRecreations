using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInput : MonoBehaviour
{
    #region Object References
    [SerializeField] private ThrowPullController m_throwPullController;
    private Camera m_Main;
    private Animator m_animator;
    private CharacterController m_Controller;
    #endregion

    #region Movement and Rotation
    private Vector3 m_Forward;
    private Vector3 m_Right;
    private float m_MinRotationSpeed = 0.1f;
    private float m_RotationSpeed = 0.1f;
    #endregion

    #region Grounded Variables
    private Vector3 m_GroundNormal;
    private bool m_IsGrounded;
    private float m_GroundCheckDistance = 0.1f;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        m_Main = Camera.main;
        m_animator = GetComponent<Animator>();
        m_Controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        m_animator.SetBool("isAiming", Input.GetMouseButton(1));
        if (Input.GetMouseButtonDown(0) && !m_throwPullController.isThrown)
        {
            m_animator.SetTrigger("throwAxe");
        }
        else if (Input.GetMouseButtonDown(0))
            m_throwPullController.PullAxe();
        if (Input.GetMouseButtonUp(0))
            m_animator.ResetTrigger("throwAxe");

        GetMovementInputs();
        
    }

    private void ThrowAxe()
    {

        m_throwPullController.ThrowAxe();
    }

    private void GetMovementInputs()
    {
        //return if player is aiming
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        m_Forward = m_Main.transform.forward;
        m_Right = m_Main.transform.right;

        m_Forward.y = 0;
        m_Right.y = 0;

        m_Forward.Normalize();
        m_Right.Normalize();

        Vector3 desiredDirection = m_Forward * z + m_Right * x;
        m_animator.SetFloat("Forward", desiredDirection.magnitude);

        if (desiredDirection.sqrMagnitude > m_MinRotationSpeed)
            PlayerMoveAndRotation(desiredDirection);
    }

    private void PlayerMoveAndRotation(Vector3 desiredDirection)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredDirection), m_RotationSpeed);
        m_Controller.Move(desiredDirection * Time.deltaTime * 3);
    }

    void CheckGroundStatus()
    {
        RaycastHit hitInfo;
#if UNITY_EDITOR
        // helper to visualise the ground check ray in the scene view
        Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance));
#endif
        // 0.1f is a small offset to start the ray from inside the character
        // it is also good to note that the transform position in the sample assets is at the base of the character
        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
        {
            m_GroundNormal = hitInfo.normal;
            m_IsGrounded = true;
        }
        else
        {
            m_IsGrounded = false;
            m_GroundNormal = Vector3.up;
        }
    }
}
