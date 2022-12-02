using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private CharacterController characterController;

    [SerializeField]private float speed = 10;
    [SerializeField]private float jumpHeight = 5;

    [SerializeField]private Transform cam;

    [SerializeField]private Transform groundSensor;
    [SerializeField]private bool isGrounded;
    [SerializeField]private LayerMask ground;
    [SerializeField]private float sensorRadius = 0.1f;
    private SphereCollider[] sphereCollider;
    private Vector3 playerVelocity;
    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    private float gravity = -9.81f;


    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {

        MovementFPS();
        Jump();
    }


    void MovementFPS()
    {
        float z = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector3 move = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        if(move != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0, angle, 0);
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0) * Vector3.forward;
            characterController.Move(moveDirection.normalized * speed * Time.deltaTime);
        }

    }

    void Jump()
    {
        if(Physics.Raycast(groundSensor.position, Vector3.down, sensorRadius, ground))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        if(isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0;
        }
        if(isGrounded && Input.GetButtonDown("Jump"))
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }
        playerVelocity.y += gravity * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);

    }
}
