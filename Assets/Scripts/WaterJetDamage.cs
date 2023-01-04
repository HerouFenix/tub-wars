using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterJetDamage : MonoBehaviour
{
    public float damage = 2.5f;

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<ChasingEnemy>().takeDamage(damage);
        }

    }
}
