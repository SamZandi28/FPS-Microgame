using System.Collections;
using Unity.FPS.AI;
using UnityEngine;

public class ShockwaveAbility : MonoBehaviour
{
    public float shockwaveRadius = 5f;
    public float pushForce = 10f;
    public float stunDuration = 2f;
    public float cooldownTime = 5f;
    public ParticleSystem shockwaveEffect;

    private bool isCooldown = false;

    public IEnumerator ActivateShockwave()
    {
        if (isCooldown) yield break;

        isCooldown = true;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, shockwaveRadius);

        foreach (Collider hitCollider in hitColliders)
        {
            EnemyController enemy = hitCollider.GetComponent<EnemyController>();
            if (enemy != null)
            {
                Vector3 pushDirection = (hitCollider.transform.position - transform.position).normalized;
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                float force = Mathf.Lerp(pushForce, 0, distance / shockwaveRadius);
                hitCollider.transform.position += pushDirection * force;
                enemy.Stun(stunDuration);
            }
        }

        if (shockwaveEffect != null)
        {
            ParticleSystem effect = Instantiate(shockwaveEffect, transform.position, Quaternion.identity);
            effect.Play();
            Destroy(effect.gameObject, effect.main.duration);
        }

        yield return new WaitForSeconds(cooldownTime);
        isCooldown = false;
    }
}
