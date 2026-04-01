using UnityEngine;
using UnityEngine.InputSystem;

public class G4_Player : MonoBehaviour
{
    public float velocidad = 0.5f;
    private PlayerInput playerInput;
    private G4_GameManager gameManager;

    void Start()
    {
        // Buscamos en el propio objeto (más seguro)
        playerInput = GetComponent<PlayerInput>();
        gameManager = Object.FindAnyObjectByType<G4_GameManager>();
    }

    void Update()
    {
        if (playerInput == null) return;

        Vector2 inputMovimiento = playerInput.actions["Move"].ReadValue<Vector2>();

        // En AR, a veces Move es X (lado a lado) e Y (arriba/abajo) 
        // o X y Z (adelante/atrás). Ajusta según tu necesidad:
        Vector3 movimiento = new Vector3(inputMovimiento.x, inputMovimiento.y, 0);
        transform.position += movimiento * velocidad * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other) // Cambiado a 3D
    {
        if (other.CompareTag("Item"))
        {
            // Desactivamos collider para evitar errores
            other.enabled = false;

            if (gameManager != null)
            {
                gameManager.ItemRecogido();
                Destroy(other.gameObject);
            }
        }
    }
}