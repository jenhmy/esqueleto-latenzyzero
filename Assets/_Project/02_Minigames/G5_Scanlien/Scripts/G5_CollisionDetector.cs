using UnityEngine;

public class G5_CollisionDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            // Desactivamos el collider para evitar el doble choque que nos pasaba antes
            other.enabled = false;

            G5_GameManager manager = Object.FindAnyObjectByType<G5_GameManager>();
            if (manager != null)
            {
                manager.ItemRecogido();
                Destroy(other.gameObject);
            }
        }
    }
}