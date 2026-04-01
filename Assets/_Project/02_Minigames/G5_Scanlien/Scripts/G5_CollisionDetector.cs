using UnityEngine;

public class G5_CollisionDetector : MonoBehaviour
{
    private G5_GameManager manager;

    void Start()
    {
        manager = Object.FindAnyObjectByType<G5_GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            other.enabled = false; // Bloqueamos el collider del item

            if (manager != null)
            {
                manager.ItemRecogido();
                Destroy(other.gameObject);
            }
        }
    }
}