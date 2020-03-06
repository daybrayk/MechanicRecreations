using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;
public class WallMovement : MonoBehaviour
{
    [SerializeField] private Transform m_WallCheck;
    [SerializeField] private Transform m_TopLimit;
    [SerializeField] private Transform m_BottomLimit;
    [SerializeField] private LayerMask m_WhatIsAWall;
    private const float MAX_CLIMB_SPEED = 10.0f;
    private Rigidbody2D m_Rigidbody;
    private OriCharacter m_controller;
    private bool m_WallMovementActive;
    private int m_FacingRight;  //true if 1, false if -1
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_controller = GetComponent<OriCharacter>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Physics2D.Raycast(m_WallCheck.position, transform.right * m_FacingRight, 0.29f, m_WhatIsAWall))
        {
            if (!m_WallMovementActive)
                m_Rigidbody.velocity = Vector2.zero;
            m_WallMovementActive = true;
        }
        else
        {
            m_Rigidbody.gravityScale = 3;
            m_WallMovementActive = false;
        }
    }

    public void WallMove(float h, float v, bool jump, bool grab)
    {
        m_controller.Move(0, false, false);

        if (!grab)
        {
            m_controller.Move(h, false, false);
            m_Rigidbody.gravityScale = 0.5f;
        }
        else
        {
            if (v > 0 && Physics2D.Raycast(m_TopLimit.position, transform.right * m_FacingRight, 0.29f, m_WhatIsAWall))
                m_Rigidbody.velocity = new Vector2(0, v * MAX_CLIMB_SPEED);
            else if (v < 0 && Physics2D.Raycast(m_BottomLimit.position, transform.right * m_FacingRight, 0.29f, m_WhatIsAWall))
                m_Rigidbody.velocity = new Vector2(0, v * MAX_CLIMB_SPEED);
            else
                m_Rigidbody.velocity = Vector2.zero;

            m_Rigidbody.gravityScale = 0;
        }
    }

    public bool GetWallMovementActive()
    {
        return m_WallMovementActive;
    }

    public void SetFacingRight(int dir)
    {
        m_FacingRight = dir;
    }
}
