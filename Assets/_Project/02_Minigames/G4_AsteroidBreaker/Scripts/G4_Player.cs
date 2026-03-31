using UnityEngine;

public class G4_Player : MonoBehaviour
{
    public float velocidad = 0.5f;
    private InputManager input;

    void Start()
    {
        // Buscamos el InputManager (esto ya funciona)
        input = Object.FindAnyObjectByType<InputManager>();

        // No tocamos la posición aquí.
        // Dejamos que los objetos usen la posición Z=1.2 que pusimos en el Inspector.
    }

    void Update()
    {
        if (input != null)
        {
            // Movimiento arriba/abajo (Y) e izquierda/derecha (X)
            Vector3 movimiento = new Vector3(input.moveDirection.x, input.moveDirection.y, 0);
            transform.position += movimiento * velocidad * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 1. Comprobamos el Tag
        if (other.CompareTag("Item"))
        {
            // 2. IMPORTANTE: Desactivamos el Collider del ítem inmediatamente
            // Esto evita que la bola azul lo toque dos veces mientras lo atraviesa
            Collider col = other.GetComponent<Collider>();
            if (col != null) col.enabled = false;

            // 3. Avisamos al Manager
            G4_GameManager managerAR = Object.FindAnyObjectByType<G4_GameManager>();
            if (managerAR != null)
            {
                managerAR.ItemRecogido();

                // 4. Lo destruimos
                Destroy(other.gameObject);
            }
        }
    }
}