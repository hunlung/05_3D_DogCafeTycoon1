using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{


    Vector2 MoveDirection;


    [SerializeField] float Speed = 7f;


    PlayerInput playerinput;

    private void Awake()
    {
        playerinput = new PlayerInput();
    }
    private void Start()
    {
    }


    private void OnEnable()
    {
        playerinput.Player.Enable();
        playerinput.Player.WASD.performed += OnMoveDirection;
        playerinput.Player.WASD.canceled += OnStop;
        playerinput.Player.Q.performed += OnLeftAngle;
        playerinput.Player.E.performed += OnRightAngle;
        playerinput.Player.LeftClick.performed += OnClick;
        playerinput.Player.RightClick.performed += OnRightClick;
        playerinput.Player.MouseWheel.performed += OnCameraZoom;
        playerinput.Player._1.performed += StoreInfo;
        playerinput.Player._2.performed += RemainStockInfo;

    }
    private void OnDisable()
    {
        playerinput.Player.WASD.performed -= OnMoveDirection;
        playerinput.Player.WASD.canceled -= OnStop;
        playerinput.Player.Q.performed -= OnLeftAngle;
        playerinput.Player.E.performed -= OnRightAngle;
        playerinput.Player.LeftClick.performed -= OnClick;
        playerinput.Player.RightClick.performed -= OnRightClick;
        playerinput.Player.MouseWheel.performed -= OnCameraZoom;
        playerinput.Player._1.performed -= StoreInfo;
        playerinput.Player._2.performed -= RemainStockInfo;
        playerinput.Disable();
    }


    private void OnCameraZoom(InputAction.CallbackContext context)
    {
        float Value = context.ReadValue<float>();
        Transform camearaTransform = gameObject.transform;
        gameObject.transform.position = new Vector3(camearaTransform.position.x,
                                                    Mathf.Clamp(camearaTransform.position.y + Value, 4, 8)
                                                    , camearaTransform.transform.position.z);
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
    }

    private void OnLeftAngle(InputAction.CallbackContext context)
    {
        
    }

    private void OnMoveDirection(InputAction.CallbackContext context)
    {
        MoveDirection = context.ReadValue<Vector2>();
    }

    private void OnStop(InputAction.CallbackContext context)
    {
        MoveDirection = Vector2.zero;
    }


    private void Update()
    {
        gameObject.transform.Translate(new Vector3(MoveDirection.x * Time.deltaTime * Speed, 0
                                          , MoveDirection.y * Time.deltaTime * Speed),Space.World);
    }



}
