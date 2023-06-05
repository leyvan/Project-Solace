using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    private CharacterController characterController;
    private PlayerAnimations playerAnimManager;
    private float walkSpeed = 2f;
    private float runSpeed = 8f;

    private float speed;

    private Vector3 characterLookDirection;
    private float horizontalInput;
    private float verticalInput;

    private bool isRunning;

    private float characterTurnTime = 0.1f;
    private float characterTurnVelocity;

    private Camera mainCamera;

    private WallDetector wallDetector;

    private Rigidbody characterRigidBody;

    private bool isGrounded;

    private enum CharacterState
    {
        InCombat,
        InOverWorld,
        InUI
    };

    private CharacterState currentCharacterState;

    private void Awake()
    {
        playerAnimManager = GetComponent<PlayerAnimations>();
        characterController = GetComponent<CharacterController>();
        characterRigidBody = GetComponent<Rigidbody>();
        wallDetector = GetComponentInChildren<WallDetector>();
        mainCamera = Camera.main;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isGrounded = true;
    }

    //For Animations or anything else
    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        characterLookDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;

        if (Input.GetKey(KeyCode.LeftShift) && characterController.isGrounded)
        {
            isRunning = true;
        }else { isRunning = false; }
        
        if(Input.GetKey(KeyCode.Space) && isGrounded == true)
        {
            characterRigidBody.AddForce(0, 0.5f, 0, ForceMode.Impulse);
        }

        if (characterLookDirection.magnitude >= 0.1f)
        {
            HandleCharacterMovementAndRotation();
        }
    }

    
    private void HandleCharacterMovementAndRotation()
    {
        speed = isRunning ? runSpeed : walkSpeed;

        float targetAngle = Mathf.Atan2(characterLookDirection.x, characterLookDirection.y) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref characterTurnVelocity, characterTurnTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0);

        Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        characterController.Move(moveDirection.normalized * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground")
        {
            isGrounded = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ground")
        {
            isGrounded = false;
        }
    }
}
