using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private static GameObject _bullet;
    private static float _bullet_speed = 11f;

    private Rigidbody _rig;

    private static GameObject GetPrefab()
    {
        return _bullet ?? (_bullet = (GameObject)Resources.Load("Prefabs/Bullet"));
    }

    public static Bullet create(Vector3 posi, Vector3 direction)
    {
        Vector3 velocity = direction * _bullet_speed;
        posi += velocity * 0.1f;
        GameObject g = (GameObject)Instantiate(GetPrefab(), posi, Quaternion.identity);
        Bullet b = g.GetComponent<Bullet>();
        b.SetVelocity(velocity);
        return b;
    }

    private void Awake()
    {
        TryGetComponent(out _rig);
    }

    public void SetVelocity(Vector3 v)
    {
        _rig.velocity = v;
    }

}
