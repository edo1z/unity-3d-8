using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float _gravity = -15.0f;

    // player
    [SerializeField] private float _walk_speed = 8f;
    [SerializeField] private float _grounded_offset = -0.14f;
    [SerializeField] private float _grounded_radius = 0.28f;
    [SerializeField] private LayerMask _ground_layers;

    private float _vertical_velocity;
    private float _terminal_velocity = 53.0f;
    public bool is_grounded = true;
    private Vector2 _move_direction;

    // Object
    private PlayerInput _input;
    private CharacterController _characon;

    private void Awake()
    {
        TryGetComponent(out _input);
        TryGetComponent(out _characon);
    }

    private void OnEnable()
    {
        _input.actions["Move"].performed += OnMove;
        _input.actions["Move"].canceled += OnMove;
        _input.actions["Fire"].started += OnFire;
    }

    private void OnDisable()
    {
        _input.actions["Move"].performed -= OnMove;
        _input.actions["Move"].canceled -= OnMove;
        _input.actions["Fire"].started -= OnFire;
    }

    private void OnMove(InputAction.CallbackContext obj)
    {
        _move_direction = obj.ReadValue<Vector2>();
    }

    private void Aim(Vector2 screen_pointer)
    {
        var ray = Camera.main.ScreenPointToRay(screen_pointer);
        Plane plane = new Plane();
        float distance = 0;
        plane.SetNormalAndPosition(Vector3.up, transform.localPosition);
        if (plane.Raycast(ray, out distance))
        {
            transform.LookAt(ray.GetPoint(distance));
        }
    }

    private void OnFire(InputAction.CallbackContext obj)
    {
        Debug.Log("on fire");
    }

    private void Move()
    {
        Vector3 direction = new Vector3(_move_direction.x, 0, _move_direction.y).normalized;
        Vector3 _move = direction * _walk_speed * Time.deltaTime;
        _move += new Vector3(0f, _vertical_velocity, 0f) * Time.deltaTime;
        _characon.Move(_move);
    }

    private void Update()
    {
        Aim(Mouse.current.position.ReadValue());
        Gravity();
        GroundedCheck();
        Move();
    }

    private void GroundedCheck()
    {
        Vector3 sphere_position = new Vector3(transform.position.x, transform.position.y - _grounded_offset,
            transform.position.z);
        is_grounded = Physics.CheckSphere(sphere_position, _grounded_radius, _ground_layers,
            QueryTriggerInteraction.Ignore);
    }

    private void Gravity()
    {
        if (is_grounded)
        {
            if (_vertical_velocity < 0.0f)
            {
                _vertical_velocity = -2f;
            }
        }
        if (_vertical_velocity < _terminal_velocity)
        {
            _vertical_velocity += _gravity * Time.deltaTime;
        }
    }

}
