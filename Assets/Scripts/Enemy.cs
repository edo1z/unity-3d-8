using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    private NavMeshAgent enemy;
    [SerializeField] private GameObject target;

    void Awake()
    {
      enemy = gameObject.GetComponent<NavMeshAgent>();
    }

    void Update()
    {
      if (target != null) {
        enemy.destination = target.transform.position;
      }
    }

    void OnCollisionEnter(Collision collision)
    {
      string tag = collision.collider.tag;
      Debug.Log("Collision! " + tag);
    }
}
