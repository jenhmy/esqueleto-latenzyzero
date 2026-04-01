using UnityEngine;
using UnityEngine.InputSystem;

public class G5_Player : MonoBehaviour
{
    [Header("Ajustes")]
    public float velocidad = 5f;

    private Vector2 direccionInput;

    // Recibe el mensaje "Move"
    private void OnMove(InputValue value)
    {
        direccionInput = value.Get<Vector2>();
    }

    void Update()
    {
        // Movimiento relativo al mundo:
        // X = Izquierda/Derecha
        // Z = Adelante/Atrás (input.y)
        Vector3 movimiento = new Vector3(direccionInput.x, 0, direccionInput.y);

        // Usamos position para mover la bola "físicamente" por el mundo
        transform.position += movimiento * velocidad * Time.deltaTime;
    }
}