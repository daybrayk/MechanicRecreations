using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof (OriCharacter))]
    public class OriControlInput : MonoBehaviour
    {
        private OriCharacter m_Character;
        private WallMovement m_WallMovement;
        private bool m_Jump;


        private void Awake()
        {
            m_Character = GetComponent<OriCharacter>();
            m_WallMovement = GetComponent<WallMovement>();
        }


        private void Update()
        {
            if (!m_Jump)
            {
                // Read the jump input in Update so button presses aren't missed.
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }
        }


        private void FixedUpdate()
        {
            // Read the inputs.
            bool crouch = Input.GetKey(KeyCode.LeftControl);
            bool grabWall = Input.GetKey(KeyCode.LeftShift);
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");
            if (h > 0)
                m_WallMovement.SetFacingRight(1);
            else if (h < 0)
                m_WallMovement.SetFacingRight(-1);
            // Pass all parameters to the character control script.
            if (!m_WallMovement.GetWallMovementActive() || (m_Character.GetGrounded() && !grabWall))
                m_Character.Move(h, crouch, m_Jump);
            else   //Movement functionality while attached to a wall
            {
                m_WallMovement.WallMove(h, v, m_Jump, grabWall);
            }

            m_Jump = false;
        }
    }
}
