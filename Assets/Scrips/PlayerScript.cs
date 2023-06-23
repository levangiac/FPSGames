using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("Player Movement")]
    public float playerSpeed = 1.9f;
    public float currentPlayerSpeed = 0f;
    public float playerSprint = 3f;
    public float currentPlayerSprint = 0f;

    [Header("Player Animato and Gravity")]
    [Header("Player Camera")]
    public Transform playerCamera;
    public CharacterController cC;
    [Header("Player Jumping and Velocity")]
    public float turnCalmTime = 0.1f;
    public float gravity = -9.81f;
    public Animator animator;
    public float surfaceDistance = -4.45f;
    public float jumpRange = 1f;
    Vector3 velocity;
    public Transform surfaceCheck;
    bool onSurface;
    float turnCalmVelocity;
    float turnCalmSurfaceDistance;
    public LayerMask surfaceMask;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

    }
    void Update()
    {
        onSurface = Physics.CheckSphere(surfaceCheck.position, surfaceDistance, surfaceMask);

        if (onSurface && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        velocity.y += gravity * Time.deltaTime;
        cC.Move(velocity * Time.deltaTime);

        playerMove();
        Jump();
        Sprint();
    }
    void playerMove()
    {
        float horizontal_axis = Input.GetAxisRaw("Horizontal");
        float vertical_axis = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal_axis, 0f, vertical_axis).normalized;


        if (direction.magnitude >= 0.1f)
        {

            animator.SetBool("Walk", true);
            animator.SetBool("Running", false);
            animator.SetBool("Idle", false);
            animator.SetTrigger("Jump");
            animator.SetBool("AnimWalk", false);
            animator.SetBool("IdleAnim", false);



            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnCalmVelocity, turnCalmTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            cC.Move(moveDirection.normalized * playerSpeed * Time.deltaTime);
            currentPlayerSpeed = playerSpeed;
        }
        else
        {
            animator.SetBool("Walk", false);
            animator.SetBool("Running", false);
            animator.SetBool("Idle", true);
            animator.SetTrigger("Jump");
            animator.SetBool("AnimWalk", false);
            animator.SetBool("IdleAnim", false);
            currentPlayerSpeed = 0f;
        }
    }
    void Jump()
    {

        if (Input.GetButtonDown("Jump") && onSurface)
        {
            animator.SetBool("Walk", false);
            animator.SetTrigger("Jump");
            velocity.y = Mathf.Sqrt(jumpRange * -2 * gravity);
        }
        else
        {
            animator.ResetTrigger("Jump");

        }
    }
    void Sprint()
    {
        if ((Input.GetButton("Sprint") && Input.GetKey(KeyCode.W)) && onSurface || onSurface && Input.GetKey(KeyCode.UpArrow))
        {

            float horizontal_axis = Input.GetAxisRaw("Horizontal");
            float vertical_axis = Input.GetAxisRaw("Vertical");
            Vector3 direction = new Vector3(horizontal_axis, 0f, vertical_axis).normalized;


            if (direction.magnitude >= 0.1f)
            {
                animator.SetBool("Walk", false);
                animator.SetBool("Running", true);
                animator.SetBool("Idle", false);
                animator.SetBool("IdleAnim", false);
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnCalmVelocity, turnCalmTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                cC.Move(moveDirection.normalized * playerSprint * Time.deltaTime);
                currentPlayerSprint = playerSprint;
            }
            else
            {
                animator.SetBool("Walk", false);
                animator.SetBool("Running", false);
                currentPlayerSprint = 0f;

            }
        }
    }
}

