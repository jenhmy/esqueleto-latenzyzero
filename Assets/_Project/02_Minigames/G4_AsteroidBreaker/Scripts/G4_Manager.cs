using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.XR.Management;

public class G4_GameManager : MonoBehaviour
{
    [Header("Configuración AR")]
    public GameObject objetoARSession; // AR Session y AR Session Origin

    [Header("Configuración de Juego")]
    public int itemsParaGanar = 2;
    public TextMeshProUGUI textoPuntos;
    public GameObject panelVictoria;

    [Header("Botones de Victoria")]
    public GameObject botonContinuar;
    public GameObject botonSalir;

    private int itemsActuales = 0;
    private int puntosTotales = 0;

    void Start()
    {
        if (objetoARSession != null) objetoARSession.SetActive(false);
        StartCoroutine(ReactivarAR());
    }

    IEnumerator ReactivarAR()
    {
        Debug.Log("G4: Comprobando motor...");
        // Intentamos arrancar los subsistemas
        if (XRGeneralSettings.Instance.Manager.activeLoader != null)
        {
            XRGeneralSettings.Instance.Manager.StartSubsystems();
        }
        else
        {
            yield return XRGeneralSettings.Instance.Manager.InitializeLoader();
            if (XRGeneralSettings.Instance.Manager.activeLoader != null)
            {
                XRGeneralSettings.Instance.Manager.StartSubsystems();
            }
        }

        yield return new WaitForSeconds(0.5f);
        if (objetoARSession != null) objetoARSession.SetActive(true);
        Debug.Log("G4: AR Re-activado con seguridad.");
    }

    public void ItemRecogido()
    {
        itemsActuales++;
        puntosTotales += 20;

        if (textoPuntos != null)
            textoPuntos.text = "Puntos: " + puntosTotales;

        if (MainManager.Instance != null)
            MainManager.Instance.SumarPuntoTemporal(20);

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

            if (MainManager.Instance != null)
            {
                bool modoHistoria = MainManager.Instance.modoHistoriaActivo;
                if (botonContinuar != null) botonContinuar.SetActive(modoHistoria);
                if (botonSalir != null) botonSalir.SetActive(true);
            }
        }
    }

    public void FinalizarNivel()
    {
        if (MainManager.Instance != null)
            MainManager.Instance.FinalizarEscenaActual();
    }
}