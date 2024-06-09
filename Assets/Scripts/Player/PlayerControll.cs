using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControll : MonoBehaviour
{
    [SerializeField] float speed = 7f;
    private float currentRotationAngle = 0f;
    private float targetRotationAngle = 0f;
    private readonly float rotationSpeed = 90f; 
    private Coroutine rotateCoroutine;


    private Vector2 moveDirection;
    private PlayerInput playerInput;
    private Camera mainCamera;
    TimeManager timemanager;

    private void Awake()
    {
        playerInput = new PlayerInput();
        mainCamera = Camera.main;
    }
    private void Start()
    {
        timemanager = GameManager.Instance.TimeManager;
    }

    private void OnEnable()
    {
        playerInput.Player.Enable();
        playerInput.Player.WASD.performed += OnMoveDirection;
        playerInput.Player.WASD.canceled += OnStop;
        playerInput.Player.Q.performed += OnLeftAngle;
        playerInput.Player.E.performed += OnRightAngle;
        playerInput.Player.LeftClick.performed += OnClick;
        playerInput.Player.RightClick.performed += OnRightClick;
        playerInput.Player.MouseWheel.performed += OnCameraZoom;
        playerInput.Player._1.performed += StoreInfo;
        playerInput.Player._2.performed += RemainStockInfo;
        playerInput.Player.V.performed += ChangeTimeSpeed;
        playerInput.Player.Space.performed += StopTimeSpeed;
    }

    private void StopTimeSpeed(InputAction.CallbackContext context)
    {
        timemanager.StopTimeToggle();
    }

    private void ChangeTimeSpeed(InputAction.CallbackContext context)
    {
        timemanager.TimeSpeedChange();
    }

    private void OnDisable()
    {
        playerInput.Player.WASD.performed -= OnMoveDirection;
        playerInput.Player.WASD.canceled -= OnStop;
        playerInput.Player.Q.performed -= OnLeftAngle;
        playerInput.Player.E.performed -= OnRightAngle;
        playerInput.Player.LeftClick.performed -= OnClick;
        playerInput.Player.RightClick.performed -= OnRightClick;
        playerInput.Player.MouseWheel.performed -= OnCameraZoom;
        playerInput.Player._1.performed -= StoreInfo;
        playerInput.Player._2.performed -= RemainStockInfo;
        playerInput.Player.V.performed -= ChangeTimeSpeed;
        playerInput.Player.Space.performed -= StopTimeSpeed;
        playerInput.Disable();
    }

    private void OnCameraZoom(InputAction.CallbackContext context)
    {
        float scrollValue = context.ReadValue<float>();
        float scrollNomalize;
        if(scrollValue > 0)
        {
            scrollNomalize = -1;
        }
        else
        {
            scrollNomalize = 1;
        }

        float PositionY = transform.position.y;
        PositionY += scrollNomalize;
        PositionY = Mathf.Clamp(PositionY, 4, 10);
        
        transform.position = new Vector3(transform.position.x,PositionY,transform.position.z);


    }

    private void RemainStockInfo(InputAction.CallbackContext context)
    {
    }

    private void StoreInfo(InputAction.CallbackContext context)
    {
    }

    private void OnRightClick(InputAction.CallbackContext context)
    {
    }

    private void OnClick(InputAction.CallbackContext context)
    {
    }
    private void OnRightAngle(InputAction.CallbackContext context)
    {
        targetRotationAngle += 25f;
        rotateCoroutine ??= StartCoroutine(RotateCamera());
    }

    private void OnLeftAngle(InputAction.CallbackContext context)
    {
        targetRotationAngle -= 25f;
        rotateCoroutine ??= StartCoroutine(RotateCamera());
    }


    private void OnMoveDirection(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
    }

    private void OnStop(InputAction.CallbackContext context)
    {
        moveDirection = Vector2.zero;
    }

    private void Update()
    {
        Vector3 movement = new(moveDirection.x, 0f, moveDirection.y);
        movement = Quaternion.Euler(0f, mainCamera.transform.eulerAngles.y, 0f) * movement;
        movement.Normalize();

        movement.x *= speed;
        movement.z *= speed;

        transform.position += movement * Time.unscaledDeltaTime;
    }

    private IEnumerator RotateCamera()
    {
        while (Mathf.Abs(currentRotationAngle - targetRotationAngle) > 0.1f)
        {
            currentRotationAngle = Mathf.MoveTowards(currentRotationAngle, targetRotationAngle, rotationSpeed * Time.unscaledDeltaTime);
            Vector3 currentRotation = mainCamera.transform.eulerAngles;
            currentRotation.y = currentRotationAngle;
            mainCamera.transform.eulerAngles = currentRotation;
            yield return null;
        }

        currentRotationAngle = targetRotationAngle;
        Vector3 targetRotation = mainCamera.transform.eulerAngles;
        targetRotation.y = currentRotationAngle;
        mainCamera.transform.eulerAngles = targetRotation;
        rotateCoroutine = null;
    }


}