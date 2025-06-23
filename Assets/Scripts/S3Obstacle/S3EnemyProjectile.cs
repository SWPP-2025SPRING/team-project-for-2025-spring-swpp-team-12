using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S3EnemyProjectile : Obstacle
{
    public float moveSpeed = 5f;
    public float rotateSpeed = 360f;
    Vector3 moveDirection;
    void Start()
    {
        Destroy(gameObject, 3f);
        moveDirection = transform.forward; 
        moveDirection.y = 0f;
    }
    void Update()
    {
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
        transform.Rotate(Vector3.right, rotateSpeed * Time.deltaTime);
    }
    protected override void OnHitPlayer()
    {
        //nothing
    }
}