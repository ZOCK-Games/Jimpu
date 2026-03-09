using UnityEngine;
using TMPro;
using System.Collections;

public class NPCManager : MonoBehaviour
{
    public string npcName;
    [SerializeField] private float Health;
    public GameObject DeathParticle;
    public Animator DeathAnimator;
    public TextMeshPro HealthText;
    protected virtual void Start()
    {
        DeathAnimator = GetComponent<Animator>();
        if (DeathAnimator == null)
        {
            DeathAnimator = GetComponentInChildren<Animator>();
        }
        HealthText = (TextMeshPro)GetComponentInChildren<TMPro.TMP_Text>(true);

        StartCoroutine(RefreshHealthText());
    }

    public virtual void SetHealth(float Amount)
    {
        Health = Amount;
        StartCoroutine(RefreshHealthText());
    }

    public virtual float GetHealth()
    {
        return Health;
    }
    public virtual IEnumerator RefreshHealthText()
    {
        yield return null;
        if (HealthText != null)
        {
            HealthText.text = Health.ToString(); // Refreshes the HealthText
        }
        else
        {
            Debug.LogError($"There is no health text at {gameObject.name}");
        }
    }
    public virtual void TakeDamage(float damage)
    {
        Health -= damage;
        RefreshHealthText();
        if (Health <= 0)
        {
            Die();
        }
    }
    /// <summary>
    /// The Standard Die Function
    /// with animation and particle
    /// </summary>
    protected virtual void Die()
    {
        ParticelManager.instance.SpawnParticle(transform.position, "Particle System Damage", 0.6f);

        if (DeathAnimator != null)
        {
            DeathAnimator.SetBool("Death", true);
        }

        else if (DeathAnimator == null)
        {
            DeathAnimator = GetComponentInChildren<Animator>();
            DeathAnimator.SetBool("Death", true);
        }
        
        Destroy(gameObject, 0.6f);
    }
}

