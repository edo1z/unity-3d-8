using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
 
    private static GameObject _player;
    private NavMeshAgent _enemy;

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
        _enemy.destination = GetPlayer().transform.position;
    }

    void OnCollisionEnter(Collision collision)
    {
      string tag = collision.collider.tag;
      Debug.Log("Collision! " + tag);
    }
}
