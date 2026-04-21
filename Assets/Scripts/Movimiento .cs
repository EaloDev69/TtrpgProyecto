using UnityEngine;

public class Movimiento : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 5f;
    private float horizontal;
    private bool mirandoDerecha = true;

    [Header("Salto")]
    public float speedSalto = 8f;
    public Transform checkPiso;
    public LayerMask layerPiso;

    private Rigidbody2D rb;

    // [Header("Animacion")]
    // public Animator anim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        // anim.SetFloat("Walk", Mathf.Abs(horizontal));

        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, speedSalto);
        }

        // if (isGrounded())
        // {
        //     anim.SetBool("Jump", false);
        //     anim.SetBool("Fall", false);
        // }
        // else
        // {
        //     if (rb.velocity.y > 0)
        //     {
        //         anim.SetBool("Jump", true);
        //         anim.SetBool("Fall", false);
        //     }
        //     else if (rb.velocity.y < 0)
        //     {
        //         anim.SetBool("Jump", false);
        //         anim.SetBool("Fall", true);
        //     }
        // }

        voltear();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * velocidad, rb.velocity.y);
    }

    public bool isGrounded()
    {
        return Physics2D.OverlapCircle(checkPiso.position, 0.2f, layerPiso);
    }

    private void voltear()
    {
        if ((mirandoDerecha && horizontal < 0f) || (!mirandoDerecha && horizontal > 0f))
        {
            mirandoDerecha = !mirandoDerecha;
            transform.Rotate(0f, 180f, 0f);
        }
    }
}