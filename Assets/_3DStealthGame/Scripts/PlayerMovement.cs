# PlayerMovement

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Animator m_Animator;
    public InputAction MoveAction;
    public float walkSpeed = 1.0f;
    public float turnSpeed = 20f;
    private float currentSpeed;
    Rigidbody m_Rigidbody;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;

    private Coroutine activeBuffCoroutine; // track active buff

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        MoveAction.Enable();
        m_Animator = GetComponent<Animator>();
        currentSpeed = walkSpeed;
    }

    void FixedUpdate()
    {
        var pos = MoveAction.ReadValue<Vector2>();
        float horizontal = pos.x;
        float vertical = pos.y;

        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize();

        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;

        m_Animator.SetBool("IsWalking", isWalking);

        // Only move and rotate if there's input
        if (isWalking)
        {
            Vector3 desiredForward = Vector3.RotateTowards(
                transform.forward,
                m_Movement,
                turnSpeed * Time.fixedDeltaTime,
                0f
            );
            m_Rotation = Quaternion.LookRotation(desiredForward);

            m_Rigidbody.MoveRotation(m_Rotation);
            m_Rigidbody.MovePosition(
                m_Rigidbody.position + m_Movement * currentSpeed * Time.fixedDeltaTime
            );
        }
    }

    void OnDisable()
    {
        MoveAction.Disable();
    }

    // ---------------------------
    // SPEED BUFF FUNCTION
    // ---------------------------
    public void ApplySpeedBuff(float buffAmount, float duration)
    {
        // Stop existing buff to prevent stacking
        if (activeBuffCoroutine != null)
        {
            StopCoroutine(activeBuffCoroutine);
        }

        activeBuffCoroutine = StartCoroutine(SpeedBuffRoutine(buffAmount, duration));
    }

    private IEnumerator SpeedBuffRoutine(float buffAmount, float duration)
    {
        currentSpeed = walkSpeed + buffAmount;
        yield return new WaitForSeconds(duration);
        currentSpeed = walkSpeed;
        activeBuffCoroutine = null; // clear reference
    }
}
