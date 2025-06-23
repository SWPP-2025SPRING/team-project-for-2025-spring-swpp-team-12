using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S3Enemy : Obstacle
{
    Animator anim;
    AnimatorStateInfo info;
    public GameObject proj;
    Transform player;
    public Transform firePoint;
    bool canAttack = false;
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update()
    {
        info = anim.GetCurrentAnimatorStateInfo(0);
        if (info.shortNameHash == Animator.StringToHash("Attack") && info.normalizedTime > 0.3f && canAttack)
        {
            canAttack = false;
            GameObject projectile = Instantiate(proj, firePoint.position, Quaternion.identity);
            projectile.transform.LookAt(player);
        }
        else
        {
            if (info.shortNameHash == Animator.StringToHash("Idle"))
            {
                canAttack = true;
            }
        }
        LookAtPlayer();
    }
    void LookAtPlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0f; // y축 방향 제거 (수평만 고려)

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;
        }
    }
    protected override void OnHitPlayer()
    {
        //nothing
    }
}
