//Movimiento basico en unity version 1.2_OmarAguilar
//----------------------------------------------------------
//Cambios 21/01/25
//currentVelocity no era necesario → Se reemplaza con rb.velocity.x.
//El cálculo de velocidad no tomaba en cuenta la velocidad real del Rigidbody2D.
//Vector2.Lerp() no era la mejor opción → Se usa Mathf.Lerp() para interpolar un solo valor.
//----------------------------------------------------------

using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float maxSpeed = 5f;        // Velocidad máxima del personaje
    public float acceleration = 8f;    // Aceleración al moverse
    public float deceleration = 10f;   // Desaceleración al soltar la tecla de movimiento

    [Header("Salto")]
    public float jumpForce = 8f;       // Fuerza del salto
    public Transform groundCheck;      // Objeto vacío como detector de suelo
    public LayerMask groundLayer;      // Capa del suelo

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Capturar entrada del usuario (A/D o flechas izquierda/derecha)
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), 0).normalized;

        // Verificar si el jugador está tocando el suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        // Salto
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    void FixedUpdate()
    {
        // Definir velocidad objetivo
        float targetSpeed = moveInput.x * maxSpeed;
        float speedDiff = targetSpeed - rb.velocity.x;

        // Determinar si usar aceleración o desaceleración
        float accelRate = (Mathf.Abs(targetSpeed) > 0.1f) ? acceleration : deceleration;

        // Aplicar interpolación lineal para suavizar el movimiento
        float movement = Mathf.Lerp(rb.velocity.x, targetSpeed, accelRate * Time.fixedDeltaTime);

        // Aplicamos la velocidad calculada
        rb.velocity = new Vector2(movement, rb.velocity.y);
    }
}
