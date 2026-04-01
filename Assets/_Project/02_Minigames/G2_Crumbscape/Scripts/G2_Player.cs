using UnityEngine;
using UnityEngine.InputSystem;

public class G2_Player : MonoBehaviour
{
    public float velocidad = 5f;
    private G2_GameManager gameManager;
    private PlayerInput playerInput;

    void Start()
    {
        // Usamos GetComponent porque ya vimos que lo tienes en el objeto
        playerInput = GetComponent<PlayerInput>();
        // Buscamos al Manager del G2
        gameManager = Object.FindAnyObjectByType<G2_GameManager>();
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
            if (gameManager != null)
            {
                gameManager.ItemRecogido();
            }
            else
            {
                Debug.LogError("¡El Player no encuentra el G2_GameManager!");
            }
        }
    }
}