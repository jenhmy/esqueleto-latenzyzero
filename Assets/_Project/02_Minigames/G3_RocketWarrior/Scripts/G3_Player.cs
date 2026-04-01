using UnityEngine;
using UnityEngine.InputSystem;

public class G3_Player : MonoBehaviour
{
    public float velocidad = 5f;
    private PlayerInput playerInput;
    private G3_GameManager gameManager;

    void Start()
    {
        // 1. Buscamos el componente EN el propio jugador
        playerInput = GetComponent<PlayerInput>();
        // 2. Buscamos al manager del G3
        gameManager = Object.FindAnyObjectByType<G3_GameManager>();
    }

    void Update()
    {
        if (playerInput == null) return;

        Vector2 inputMovimiento = playerInput.actions["Move"].ReadValue<Vector2>();
        Vector3 movimiento = new Vector3(inputMovimiento.x, inputMovimiento.y, 0);
        transform.position += movimiento * velocidad * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D otro)
    {
        if (otro.CompareTag("Item"))
        {
            Destroy(otro.gameObject);
            if (gameManager != null) gameManager.ItemRecogido();
        }
    }
}