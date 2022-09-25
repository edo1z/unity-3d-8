using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    private static GameObject _player;
    private static GameObject _enemy_obj;

    private NavMeshAgent _enemy;
    private float _fired_time = 0f;
    private float _fired_interval = 2f;
    private float _respawn_interval = 3f;
    private int _enemy_index;
    private bool _destroyed = false;

    private static GameObject GetEnemy()
    {
        return _enemy_obj ?? (_enemy_obj = (GameObject)Resources.Load("Prefabs/Enemy/Enemy"));
    }

    private static GameObject GetPlayer()
    {
        return _player ?? (_player = GameObject.FindWithTag("Player"));
    }

    public static void Spawn(Vector3 posi, int enemy_index)
    {
        GameObject g = Instantiate(GetEnemy(), posi, Quaternion.identity);
        Enemy e = g.GetComponent<Enemy>();
        e.SetEnemyIndex(enemy_index);
    }

    public void SetEnemyIndex(int index)
    {
        _enemy_index = index;
    }

    void Awake()
    {
        _enemy = gameObject.GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (!_destroyed)
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
    }

    private void DestroyEnemy()
    {
        _destroyed = true;
        Game.DestroyedEnemy();
        transform.localScale = new Vector3(0, 0, 0);
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(_respawn_interval);
        transform.localScale = new Vector3(1f, 1f, 1f);
        _enemy.Warp(Game.GetEnemyPosi(_enemy_index));
        _fired_time = 0;
        _destroyed = false;
    }


    void OnCollisionEnter(Collision collision)
    {
        string tag = collision.collider.tag;
        if (tag == "PlayerBullet" && !_destroyed)
        {
            DestroyEnemy();
            GameObject particles = Bullet.GetEnemyBulletParticles();
            GameObject g = Instantiate(particles, transform.position, Quaternion.identity);
            ParticleSystem p = g.GetComponent<ParticleSystem>();
            p.Play();
            Destroy(p.gameObject, 3f);
        }
    }
}
