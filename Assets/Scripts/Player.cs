using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private static GameObject _player;

    // player
    [SerializeField] private float _walk_speed = 8f;
    [SerializeField] private float _grounded_offset = -0.14f;
    [SerializeField] private float _grounded_radius = 0.28f;
    [SerializeField] private LayerMask _ground_layers;
    private float _respawn_interval = 2f;

    public float _gravity = -15.0f;
    private float _vertical_velocity;
    private float _terminal_velocity = 53.0f;
    public bool is_grounded = true;
    private Vector2 _move_direction;
    private bool _destroyed = false;

    // Object
    private PlayerInput _input;
    private CharacterController _characon;

    private static GameObject GetPlayer()
    {
        return _player ?? (_player = (GameObject)Resources.Load("Prefabs/Player/Player"));
    }

    public static void Spawn(Vector3 posi)
    {
        Instantiate(GetPlayer(), posi, Quaternion.identity);
    }

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

    private void AimAndFire(bool fire)
    {
        Vector2 screen_pointer = Mouse.current.position.ReadValue();
        var ray = Camera.main.ScreenPointToRay(screen_pointer);
        Plane plane = new Plane();
        float distance = 0;
        plane.SetNormalAndPosition(Vector3.up, transform.localPosition);
        if (plane.Raycast(ray, out distance))
        {
            Vector3 point = ray.GetPoint(distance);
            transform.LookAt(point);
            if (fire)
            {
                Vector3 direction = (point - transform.position).normalized;
                Bullet.create("Player", transform.position, direction);
            }
        }
    }

    private void OnFire(InputAction.CallbackContext obj)
    {
        if (!_destroyed)
        {
            AimAndFire(true);
        }
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
        if (!_destroyed)
        {
            AimAndFire(false);
            Gravity();
            GroundedCheck();
            Move();
        }
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

    private void DestroyPlayer()
    {
        _destroyed = true;
        Game.DestroyedPlayer();
        transform.localScale = new Vector3(0, 0, 0);
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(_respawn_interval);
        transform.localScale = new Vector3(1f, 1f, 1f);
        transform.position = Game.GetPlayerPosi();
        _destroyed = false;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        string tag = hit.collider.tag;
        if (tag == "EnemyBullet" && !_destroyed)
        {
            DestroyPlayer();
            GameObject particles = Bullet.GetPlayerBulletParticles();
            GameObject g = Instantiate(particles, transform.position, Quaternion.identity);
            ParticleSystem p = g.GetComponent<ParticleSystem>();
            p.Play();
            Destroy(p.gameObject, 3f);
        }
    }

}
