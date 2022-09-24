using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private static GameObject _bullet;
    private static float _bullet_speed = 30f;
    private static float _bullet_lifetime_limit = 3f;
    private float _bullet_lifetime = 0f;

    private Rigidbody _rig;

    private static GameObject GetPrefab()
    {
        return _bullet ?? (_bullet = (GameObject)Resources.Load("Prefabs/Bullet"));
    }

    public static Bullet create(Vector3 posi, Vector3 direction)
    {
        Vector3 velocity = direction * _bullet_speed;
        posi += velocity * 0.02f;
        GameObject g = (GameObject)Instantiate(GetPrefab(), posi, Quaternion.identity);
        Bullet b = g.GetComponent<Bullet>();
        b.SetVelocity(velocity);
        return b;
    }

    public void SetVelocity(Vector3 v)
    {
        _rig.velocity = v;
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
            Destroy(gameObject);
        }
    }

}
