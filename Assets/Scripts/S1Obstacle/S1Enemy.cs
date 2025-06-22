using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S1Enemy : Obstacle
{
    // Transform player;
    // void Start()
    // {
    //     player = GameObject.FindGameObjectWithTag("Player").transform;
    // }
    // void Update()
    // {
    //     LookAtPlayer();
    // }   
    // void LookAtPlayer()
    // {
    //     Vector3 direction = player.position - transform.position;
    //     direction.y = 0f; // y축 방향 제거 (수평만 고려)

    //     if (direction != Vector3.zero)
    //     {
    //         Quaternion targetRotation = Quaternion.LookRotation(direction);
    //         transform.rotation = targetRotation;
    //     }
    // }
    protected override void OnHitPlayer()
    {
        //nothing
    }
}
