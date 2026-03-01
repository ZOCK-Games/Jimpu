using UnityEngine;
using TMPro;

public class NPCManager : MonoBehaviour
{
    public string npcName;
    [SerializeField ]private float Health;
    public GameObject DeathParticle;
    public Animator DeathAnimator;
    public TextMeshPro HealthText;
    protected Transform player;

    protected virtual void Start()
    {
        DeathAnimator = GetComponent<Animator>();
        if (DeathAnimator == null)
        {
            DeathAnimator = GetComponentInChildren<Animator>();
        }
        HealthText = (TextMeshPro)GetComponentInChildren<TMPro.TMP_Text>(true);
        player = GameObject.FindGameObjectWithTag("Player").transform;
        RefreshHealthText();
    }

    public virtual void SetHealth(float Amount)
    {
        Health = Amount;
        RefreshHealthText();
    }

    public virtual float GetHealth()
    {
        return Health;
    }
    public virtual void RefreshHealthText()
    {
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
        GameObject ParticleGo = Instantiate(DeathParticle);
        ParticleGo.transform.position = transform.position;
        ParticleSystem ParticleSy = ParticleGo.GetComponent<ParticleSystem>();
        ParticleSy.Play();
        DeathAnimator.SetTrigger("Death");
        Destroy(ParticleGo, 0.6f);
        Destroy(gameObject, 0.6f);
    }
}

