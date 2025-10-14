using UnityEngine;

public class EnemyInfo : MonoBehaviour
{
    public int EnemyHealt;
    public Vector3 EnemyPosition;
    public bool IsMoving;
    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    void Start()
    {
        anim = this.gameObject.GetComponent<Animator>();
        sr = this.gameObject.GetComponent<SpriteRenderer>();
        rb = this.gameObject.GetComponent<Rigidbody2D>();

    }
    void Update()
    {
        Vector3 PlayerPosBevore = this.gameObject.transform.position;
        if (this.gameObject.transform.position == PlayerPosBevore)
        {
            anim.SetBool("Walk", true);
        }
            anim.SetBool("Walk", false);

        if (rb.linearVelocity.x > 0)
        {
            sr.flipX = true;
        }
        else if (rb.linearVelocity.x < 0)
        {
            sr.flipX = false;
        }
    }

}
