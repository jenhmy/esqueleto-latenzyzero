using UnityEngine;
using UnityEngine.InputSystem; // ¡IMPORTANTE AÑADIR ESTO!

public class G1_Player : MonoBehaviour
{
    public float velocidad = 5f;
    private G1_GameManager gameManager;

    // Referencia al componente que pusiste en GLOBAL_SYSTEMS
    private PlayerInput playerInput;

    void Start()
    {
        // CAMBIO: Ahora lo busca en el propio jugador, no en toda la escena
        playerInput = GetComponent<PlayerInput>();
        gameManager = Object.FindAnyObjectByType<G1_GameManager>();
    }

    void Update()
    {
        // SEGURIDAD: Si el playerInput no está listo, no hace nada y así no da error
        if (playerInput == null) return;

        // Leemos la acción "Move" de tu archivo azul
        Vector2 inputMovimiento = playerInput.actions["Move"].ReadValue<Vector2>();

        Vector3 movimiento = new Vector3(inputMovimiento.x, inputMovimiento.y, 0);
        transform.position += movimiento * velocidad * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D otro)
    {
        if (otro.CompareTag("Item"))
        {
            Destroy(otro.gameObject);
            gameManager.ItemRecogido();
        }
    }
}