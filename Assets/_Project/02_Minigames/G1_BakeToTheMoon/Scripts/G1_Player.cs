using UnityEngine;

public class G1_Player : MonoBehaviour
{
    public float velocidad = 5f;
    private InputManager input;
    private G1_GameManager gameManager;

    void Start()
    {
        input = Object.FindAnyObjectByType<InputManager>();
        gameManager = Object.FindAnyObjectByType<G1_GameManager>();
    }

    void Update()
    {
        Vector3 movimiento = new Vector3(input.moveDirection.x, input.moveDirection.y, 0);
        transform.position += movimiento * velocidad * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D otro)
    {
        // Si tocamos un Item, avisamos al GameManager del nivel
        if (otro.CompareTag("Item"))
        {
            Destroy(otro.gameObject);
            gameManager.ItemRecogido();
        }
    }
}