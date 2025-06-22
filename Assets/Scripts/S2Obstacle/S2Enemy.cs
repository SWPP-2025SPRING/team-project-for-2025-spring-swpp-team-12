using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2Enemy : Obstacle
{
    Animator anim;
    AnimatorStateInfo info;
    Transform player;

    GameObject laser;
    BoxCollider box;

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        laser = transform.GetChild(2).gameObject;
        box = laser.GetComponent<BoxCollider>();
        laser.SetActive(false);
        box.enabled = false;
    }

    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        info = anim.GetCurrentAnimatorStateInfo(0);
        if (info.shortNameHash == Animator.StringToHash("Attack") && info.normalizedTime > 0.3f && info.normalizedTime < 0.9f)
        {
            laser.SetActive(true);
            if (info.normalizedTime > 0.5f && info.normalizedTime < 0.9f)
            {
                box.enabled = true; // 공격 활성화
            }
            else
            {
                box.enabled = false; // 공격 비활성화
            }
        }
        else
        {
            laser.SetActive(false);
            box.enabled = false;
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
