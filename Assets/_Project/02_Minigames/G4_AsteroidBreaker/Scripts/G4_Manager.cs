using UnityEngine;
using TMPro;

public class G4_GameManager : MonoBehaviour
{
    public int itemsParaGanar = 2;
    public TextMeshProUGUI textoPuntos;
    public GameObject panelVictoria;

    private int itemsActuales = 0;
    private int puntosTotales = 0;

    // Esta función la llamará el Player cuando toque una bola
    public void ItemRecogido()
    {
        itemsActuales++;
        puntosTotales += 20;

        if (textoPuntos != null)
            textoPuntos.text = "Puntos: " + puntosTotales;

        // Avisamos al MainManager (el que guarda los puntos entre escenas)
        MainManager main = Object.FindAnyObjectByType<MainManager>();
        if (main != null) main.SumarPuntoTemporal(20);

        // Si ya tenemos los 2, ganamos
        if (itemsActuales >= itemsParaGanar)
        {
            if (panelVictoria != null) panelVictoria.SetActive(true);
        }
    }

    public void BotonVolver()
    {
        MainManager main = Object.FindAnyObjectByType<MainManager>();
        if (main != null) main.FinalizarEscenaActual();
    }
}