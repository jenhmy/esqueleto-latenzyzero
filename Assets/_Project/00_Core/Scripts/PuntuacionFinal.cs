using UnityEngine;
using TMPro;

public class PuntuacionFinal : MonoBehaviour
{
    public TextMeshProUGUI textoPuntos;

    void Start()
    {
        // Cogemos los puntos directamente del MainManager antes de que se borren
        if (MainManager.Instance != null)
        {
            textoPuntos.text = "PUNTUACIÓN TOTAL: " + MainManager.Instance.puntosTotalesVisualizar;
        }
    }

    public void IrAlMenuPrincipal()
    {
        if (MainManager.Instance != null)
        {
            MainManager.Instance.VolverAlMainMenu();
        }
    }
}
