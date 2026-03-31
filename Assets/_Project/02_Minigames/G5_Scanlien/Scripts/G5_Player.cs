using UnityEngine;

public class G5_Player : MonoBehaviour
{
    public float velocidad = 1.5f;
    private InputManager input;

    void Start()
    {
        input = Object.FindAnyObjectByType<InputManager>();
    }

    void Update()
    {
        if (input != null)
        {
            // Movimiento en X (izq/der) y Z (adelante/atrás)
            // Usamos la rotación del XR Origin para que "adelante" sea hacia donde miras
            Vector3 direccion = new Vector3(input.moveDirection.x, 0, input.moveDirection.y);
            transform.Translate(direccion * velocidad * Time.deltaTime);
        }
    }
}