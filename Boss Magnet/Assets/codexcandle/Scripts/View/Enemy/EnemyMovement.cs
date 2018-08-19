using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Codebycandle.NCSoftDemo;

public class EnemyMovement:MonoBehaviour
{
    private Transform player;
    private NavMeshAgent nav;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag(GameTag.TAG_PLAYER).transform;

        nav = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        nav.SetDestination(player.position);
    }
}