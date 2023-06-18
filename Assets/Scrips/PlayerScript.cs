using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("Player Movement")]
    public float playerSpeed = 1.9f;
    [Header("Player Animato and Gravity")]
    public CharacterController cC;
    [Header("Player Jumping and Velocity")]
    public float turnCalmTime = 0.1f;

    void Update()
    {
        playerMove();
    }
    void playerMove()
    {
        float horizontal_axis = Input.GetAxisRaw("Horizontal");
        float vertical_axis = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal_axis, 0f, vertical_axis).normalized;


        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngle.y
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
            cC.Move(direction.normalized * playerSpeed * Time.deltaTime);
        }
    }
}

