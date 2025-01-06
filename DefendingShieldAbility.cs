using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.UI;

public class DefendingShieldAbility : MonoBehaviour
{
    public float shieldDuration = 5f;       // Duration of the shield
    public GameObject shieldSphere;         // Shield visual object
    public Slider shieldDurationSlider;     // Duration slider in HUD

    private bool isActive = false;
    private float remainingShieldTime;
    private Health playerHealth;

    void Start()
    {
        playerHealth = GetComponent<Health>();
        shieldSphere.SetActive(false);
        shieldDurationSlider.gameObject.SetActive(false);
    }

    public void ActivateShield()
    {
        if (isActive) return;

        isActive = true;
        playerHealth.Invincible = true;
        shieldSphere.SetActive(true);
        remainingShieldTime = shieldDuration;

        shieldDurationSlider.gameObject.SetActive(true);
        shieldDurationSlider.value = 1f;
    }

    void Update()
    {
        if (isActive)
        {
            remainingShieldTime -= Time.deltaTime;
            shieldDurationSlider.value = remainingShieldTime / shieldDuration;

            if (remainingShieldTime <= 0f)
            {
                DeactivateShield();
            }
        }
    }

    private void DeactivateShield()
    {
        isActive = false;
        playerHealth.Invincible = false;
        shieldSphere.SetActive(false);
        shieldDurationSlider.gameObject.SetActive(false);
    }
}
