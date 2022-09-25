using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private static GameObject _player_bullet;
    private static GameObject _enemy_bullet;
    private static GameObject _player_bullet_particles;
    private static GameObject _enemy_bullet_particles;
    private static float _bullet_speed = 30f;
    private static float _bullet_lifetime_limit = 3f;
    private float _bullet_lifetime = 0f;
    private string _bullet_type = "Player"; // Player or Enemy

    private Rigidbody _rig;

    private static GameObject GetPlayerBullet()
    {
        return _player_bullet ?? (_player_bullet = (GameObject)Resources.Load("Prefabs/Player/PlayerBullet"));
    }

    private static GameObject GetEnemyBullet()
    {
        return _enemy_bullet ?? (_enemy_bullet = (GameObject)Resources.Load("Prefabs/Enemy/EnemyBullet"));
    }

    public static GameObject GetPlayerBulletParticles()
    {
        return _player_bullet_particles ?? (_player_bullet_particles = (GameObject)Resources.Load("Prefabs/Player/PlayerDestroyedBullet"));
    }

    public static GameObject GetEnemyBulletParticles()
    {
        return _enemy_bullet_particles ?? (_enemy_bullet_particles = (GameObject)Resources.Load("Prefabs/Enemy/EnemyDestroyedBullet"));
    }

    public static Bullet create(string target, Vector3 posi, Vector3 direction)
    {
        Vector3 velocity = direction * _bullet_speed;
        posi += velocity * 0.02f;
        GameObject bullet = (target == "Player") ? GetPlayerBullet() : GetEnemyBullet();
        GameObject g = (GameObject)Instantiate(bullet, posi, Quaternion.identity);
        Bullet b = g.GetComponent<Bullet>();
        b.SetVelocity(velocity);
        b.SetBulletType(target);
        return b;
    }

    public void SetVelocity(Vector3 v)
    {
        _rig.velocity = v;
    }

    public void SetBulletType(string type)
    {
        _bullet_type = type;
    }

    private void Awake()
    {
        TryGetComponent(out _rig);
    }

    private void Update()
    {
        _bullet_lifetime += Time.deltaTime;
        if (_bullet_lifetime > _bullet_lifetime_limit)
        {
            GameObject particles = (_bullet_type == "Player") ? GetPlayerBulletParticles() : GetEnemyBulletParticles();
            GameObject g = Instantiate(particles, transform.position, Quaternion.identity);
            ParticleSystem p = g.GetComponent<ParticleSystem>();
            p.Play();
            Destroy(p.gameObject, 3f);
            Destroy(gameObject);
        }
    }

}
