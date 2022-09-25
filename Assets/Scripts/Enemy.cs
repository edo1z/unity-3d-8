using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    private static GameObject _player;
    private NavMeshAgent _enemy;
    private float _fired_time = 0f;
    private float _fired_interval = 5f;

    private static GameObject GetPlayer()
    {
        return _player ?? (_player = GameObject.FindWithTag("Player"));
    }

    void Awake()
    {
        _enemy = gameObject.GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        Vector3 player_position = GetPlayer().transform.position;
        _enemy.destination = player_position;
        _fired_time += Time.deltaTime;
        if (_fired_time > _fired_interval)
        {
          _fired_time = 0f;
          Vector3 direction = (player_position - transform.position).normalized;
          Bullet.create("Enemy", transform.position, direction);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        string tag = collision.collider.tag;
        if (tag == "PlayerBullet")
        {
            GameObject particles = Bullet.GetEnemyBulletParticles();
            GameObject g = Instantiate(particles, transform.position, Quaternion.identity);
            ParticleSystem p = g.GetComponent<ParticleSystem>();
            p.Play();
            Destroy(p.gameObject, 3f);
            Destroy(gameObject);
        }
    }
}
