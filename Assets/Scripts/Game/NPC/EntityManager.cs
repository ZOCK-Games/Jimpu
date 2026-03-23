using UnityEngine;
using TMPro;
using System.Collections;

public class EntityManager : MonoBehaviour
{
    public string npcName;
    [SerializeField] private float Health;
    public Animator DeathAnimator;
    public TextMeshPro HealthText;
    public bool CanMove;
    public bool canTakeDamage;
    private Animator animator;
    private Rigidbody2D rb;
    public float DestroyTime = 0.6f;

    protected virtual void Start()
    {
        canTakeDamage = true;
        DeathAnimator = this.GetComponent<Animator>();
        if (DeathAnimator == null)
        {
            DeathAnimator = GetComponentInChildren<Animator>();
        }
        HealthText = (TextMeshPro)GetComponentInChildren<TMPro.TMP_Text>(true);
        if (HealthText == null)
        {
            TextMeshPro Text = new GameObject().AddComponent<TextMeshPro>();
            HealthText = Text;
        }
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(RefreshHealthText());
    }

    public virtual void SetMovement(bool canMove)
    {
        animator?.SetBool("CanMove", canMove);

        CanMove = canMove;
        if (rb != null)
        {
            rb.constraints = canMove
                ? RigidbodyConstraints2D.FreezeRotation
                : RigidbodyConstraints2D.FreezeAll;
        }
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
        if (canTakeDamage)
        {
            Health -= damage;
            StartCoroutine(RefreshHealthText());
            if (Health <= 0)
            {
                Die();
            }

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
            if (DeathAnimator != null)
            {
                DeathAnimator.SetBool("Death", true);
            }
        }

        canTakeDamage = false;
        Destroy(gameObject, DestroyTime);
    }
}

