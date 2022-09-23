using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float _walk_speed = 8f;

    private PlayerInput _input;
    private CharacterController _characon;
    private Vector2 _move_direction;
    private Vector2 _look_direction;

    private void Awake()
    {
        Debug.Log("Awake");
        TryGetComponent(out _input);
        TryGetComponent(out _characon);
    }

    private void OnEnable()
    {
        Debug.Log("OnEnable");
        _input.actions["Move"].performed += OnMove;
        _input.actions["Move"].canceled += OnMove;
        _input.actions["Look"].performed += OnLook;
        _input.actions["Fire"].started += OnFire;
    }

    private void OnDisable()
    {
        Debug.Log("OnDisable");
        _input.actions["Move"].performed -= OnMove;
        _input.actions["Move"].canceled -= OnMove;
        _input.actions["Look"].started -= OnLook;
        _input.actions["Fire"].started -= OnFire;
    }

    private void OnMove(InputAction.CallbackContext obj)
    {
        _move_direction = obj.ReadValue<Vector2>();
    }

    private void OnLook(InputAction.CallbackContext obj)
    {
        _look_direction = obj.ReadValue<Vector2>();
    }

    private void OnFire(InputAction.CallbackContext obj)
    {
        Debug.Log("on fire");
    }

    private void Move()
    {
        Vector3 direction = new Vector3(_move_direction.x, 0, _move_direction.y).normalized;
        _characon.Move(direction * _walk_speed * Time.deltaTime);
    }

    private void Update()
    {
        Move();
    }

}
