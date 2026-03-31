using UnityEngine;
using TMPro;

public class G2_GameManager : MonoBehaviour
{
    public int itemsParaGanar = 2;
    public TextMeshProUGUI textoUI;
    public GameObject panelVictoria;

    private int itemsActuales = 0;
    private int puntosTotales = 0;

    public void ItemRecogido()
    {
        itemsActuales++;
        puntosTotales += 5;

        if (textoUI != null)
        {
            textoUI.text = "Puntos: " + puntosTotales;
        }

        // --- MEJORA: Usamos la instancia directa ---
        if (MainManager.Instance != null)
        {
            MainManager.Instance.SumarPuntoTemporal(5);
        }

        if (itemsActuales >= itemsParaGanar)
        {
            GanarMinijuego();
        }
    }

    private void GanarMinijuego()
    {
        if (panelVictoria != null)
        {
            panelVictoria.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void BotonVolverAlSelector()
    {
        // --- MEJORA: Usamos la instancia directa ---
        if (MainManager.Instance != null)
        {
            // Esto decidirá si va a la siguiente Intro o al Menú
            MainManager.Instance.FinalizarEscenaActual();
        }
    }
}