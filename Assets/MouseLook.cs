using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 10;
    [SerializeField]  Transform debugHitPointTransform;

    [SerializeField] Transform hookshotTransform;

    private CharacterController characterController;
    private float cameraVerticalAngle;
    private float characterVelocityY;
    private Camera playerCamera;
    private Camera playerCamera3;
    private CinemachineVirtualCamera cameras;
    private State state;
    private Vector3 hookshotPosition;
    private float hookshotSize;
    public GameObject cam1;
    public GameObject cam2;
    public GameObject cam3;
    public GameObject crosshair;
    

    private enum State
    {
        Normal,
        HookshotThrown,
        HookshotFlyingPlayer,
        
    }

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
       playerCamera = transform.Find("Main Camera").GetComponent<Camera>();
       playerCamera3 = transform.Find("3rd Person").GetComponent<Camera>();

        Cursor.lockState = CursorLockMode.Locked;
        state = State.Normal;
        hookshotTransform.gameObject.SetActive(false);
    }


    // Update is called once per frame
    private void Update()
    {
        switch (state) {
            default:
            case State.Normal:
            HandleCharacterLook();
            HandleCharacterMovement();
            HandleHookshotStart();
                break;
            case State.HookshotThrown:
               
                HandleHookshotThrow();
                HandleCharacterMovement();
                break;
            case State.HookshotFlyingPlayer:
                HandleHookshotMovement();
                //HandleCharacterLook();
                break;
        }
    }

    private void HandleCharacterLook()
    {
        float lookX = Input.GetAxis("Mouse X");
        float lookY = Input.GetAxis("Mouse Y");

        transform.Rotate(new Vector3(0f, lookX * mouseSensitivity, 0f), Space.Self);

        cameraVerticalAngle -= lookY * mouseSensitivity;

        cameraVerticalAngle = Mathf.Clamp(cameraVerticalAngle, -89f, 89f);

        playerCamera.transform.localEulerAngles = new Vector3(cameraVerticalAngle, 0, 0);
        playerCamera3.transform.localEulerAngles = new Vector3(cameraVerticalAngle, 0, 0);
    }

    private void HandleCharacterMovement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        float moveSpeed = 20f;

        Vector3 characterVelocity = transform.right * moveX * moveSpeed + transform.forward * moveZ * moveSpeed;

        if (characterController.isGrounded)
        {
            characterVelocityY = 0f;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                float jumpSpeed = 30f;
                characterVelocityY = jumpSpeed;
            }
        }

        float gravityDownForce = -60f;
        characterVelocityY += gravityDownForce * Time.deltaTime;

        characterVelocity.y = characterVelocityY;

        characterController.Move(characterVelocity * Time.deltaTime);

    }

   
    private void HandleHookshotStart()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            cam2.SetActive(false);
            cam1.SetActive(true);
            cam3.SetActive(false);

            crosshair.SetActive(true);

            if (Physics.Raycast(playerCamera.transform.position,playerCamera.transform.forward, out RaycastHit raycastHit)){
                debugHitPointTransform.position = raycastHit.point;
                hookshotPosition = raycastHit.point;
                hookshotSize = 10f;
                hookshotTransform.gameObject.SetActive(true);
                
               
                state = State.HookshotThrown;
               
            }
        }
    }

    private void HandleHookshotThrow()
    {

        cam3.SetActive(false);
        cam1.SetActive(false);
        cam2.SetActive(false);

        hookshotTransform.LookAt(hookshotPosition);

        float hookshotThrowSpeed = 40f;
        hookshotSize += hookshotThrowSpeed * Time.deltaTime;
        hookshotTransform.localScale = new Vector3(1, 1, hookshotSize);

        if (hookshotSize >= Vector3.Distance(transform.position, hookshotPosition))
        {
            state = State.HookshotFlyingPlayer;
        }
    }


    private void HandleHookshotMovement()
    {
        Vector3 hookshotDir = (hookshotPosition - transform.position).normalized;

        float hookshotSpeedMin = 10f;
        float hookshotSpeedMax = 40f;
        float hookshotSpeed = Mathf.Clamp(Vector3.Distance(transform.position, hookshotPosition), hookshotSpeedMin, hookshotSpeedMax);
        float hookshotSpeedMultiplier = 2f;




        characterController.Move(hookshotDir * hookshotSpeed * hookshotSpeedMultiplier * Time.deltaTime);

        float reachedHookshotPositionDistance = 3f;
        if (Vector3.Distance(transform.position, hookshotPosition) < reachedHookshotPositionDistance)
        {
            state = State.Normal;
            hookshotTransform.gameObject.SetActive(false);

            cam3.SetActive(true);
            cam1.SetActive(false);
            cam2.SetActive(true);
            crosshair.SetActive(false);
        }
    }


}
