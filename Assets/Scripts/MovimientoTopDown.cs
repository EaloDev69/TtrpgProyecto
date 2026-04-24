using UnityEngine;

public class MovimientoTopDown : MonoBehaviour
{
    public float velocidad = 5f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        Vector2 movimiento = Vector2.zero;

        // Prioriza el eje horizontal, si no hay input horizontal revisa vertical
        if (inputX != 0)
        {
            movimiento = new Vector2(inputX, 0);
        }
        else if (inputY != 0)
        {
            movimiento = new Vector2(0, inputY);
        }

        rb.velocity = movimiento * velocidad;
    }
}