using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] int damage = 10;

    void OnTriggerEnter(Collider other)
    {
        var dmgReceiver = other.GetComponent<IDamageable>();
        if (dmgReceiver != null)
        {
            dmgReceiver.ChangeHealth(damage);
            Destroy(gameObject);
        }
    }
}
