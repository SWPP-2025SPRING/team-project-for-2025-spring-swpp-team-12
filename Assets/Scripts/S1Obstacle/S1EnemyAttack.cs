using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S1EnemyAttack : Obstacle
{
    Animator anim;
    BoxCollider box;
    AnimatorStateInfo info;

    void Start()
    {
        // Debug.Log(transform.parent.name);
        anim = transform.parent?.GetComponent<Animator>();
        box = GetComponent<BoxCollider>();
    }

    void Update()
    {
        info = anim.GetCurrentAnimatorStateInfo(0);
        if (info.shortNameHash == Animator.StringToHash("Attack") && info.normalizedTime > 0.4f && info.normalizedTime < 0.6f)
        {
            box.enabled = true;
        }
        else
        {
            box.enabled = false;
        }
    }

    protected override void OnHitPlayer()
    {
        //dotdeal
    }
}
