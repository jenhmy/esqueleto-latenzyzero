using UnityEngine;
using TMPro;

public class G1_GameManager : MonoBehaviour
{
    public int itemsParaGanar = 2;
    public TextMeshProUGUI textoUI;
    public GameObject panelVictoria;

    [Header("Botones de Victoria")]
    public GameObject botonContinuar; // El de "Siguiente Nivel"
    public GameObject botonSalir;    // El de "Volver al Menú"

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

            // LÓGICA DE BOTONES SEGÚN EL MODO
            if (MainManager.Instance != null)
            {
                if (MainManager.Instance.modoHistoriaActivo)
                {
                    // En Historia: puede continuar o salir
                    botonContinuar.SetActive(true);
                    botonSalir.SetActive(true);
                }
                else
                {
                    // En Nivel Suelto: solo puede salir
                    botonContinuar.SetActive(false);
                    botonSalir.SetActive(true);
                }
            }
        }
    }
}