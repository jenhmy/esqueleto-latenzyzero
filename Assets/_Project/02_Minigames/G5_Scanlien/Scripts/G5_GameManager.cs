using UnityEngine;
using TMPro;

public class G5_GameManager : MonoBehaviour
{
    public int itemsParaGanar = 2;
    public TextMeshProUGUI textoPuntos;
    public GameObject panelVictoria;

    private int itemsActuales = 0;
    private int puntosTotales = 0;

    public void ItemRecogido()
    {
        itemsActuales++;
        puntosTotales += 20;

        if (textoPuntos != null)
            textoPuntos.text = "Puntos: " + puntosTotales;

        // Sumamos al MainManager global
        MainManager main = Object.FindAnyObjectByType<MainManager>();
        if (main != null) main.SumarPuntoTemporal(20);

        if (itemsActuales >= itemsParaGanar)
        {
            if (panelVictoria != null)
            {
                panelVictoria.SetActive(true);
                // En VR es vital liberar el ratón para poder pulsar el botón en pantalla
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }

    public void BotonVolver()
    {
        MainManager main = Object.FindAnyObjectByType<MainManager>();
        if (main != null) main.FinalizarEscenaActual();
    }
}