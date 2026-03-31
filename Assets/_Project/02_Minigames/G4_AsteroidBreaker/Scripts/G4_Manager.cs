using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.XR.Management;

public class G4_GameManager : MonoBehaviour
{
    [Header("Configuración AR")]
    public GameObject objetoARSession;

    [Header("Configuración de Juego")]
    public int itemsParaGanar = 2;
    public TextMeshProUGUI textoPuntos;
    public GameObject panelVictoria;

    private int itemsActuales = 0;
    private int puntosTotales = 0;

    private void Awake()
    {
        // --- NUEVA LIMPIEZA DE CÁMARAS FANTASMA ---
        // Buscamos todas las cámaras para destruir las que no pertenecen a este nivel
        Camera[] todasLasCamaras = Object.FindObjectsByType<Camera>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (Camera cam in todasLasCamaras)
        {
            // Si es la cámara de simulación o la cámara que viene de Transition, la borramos
            if (cam.gameObject.name.Contains("Simulation") || (cam.gameObject.name == "Main Camera" && cam.transform.parent == null))
            {
                Debug.Log("G4: Destruyendo cámara intrusa: " + cam.gameObject.name);
                Destroy(cam.gameObject);
            }
        }

        // También borramos el entorno de simulación si existe para que no tape el mundo real
        GameObject simEnv = GameObject.Find("Environment");
        if (simEnv != null) Destroy(simEnv);
    }

    void Start()
    {
        if (objetoARSession != null)
        {
            objetoARSession.SetActive(false);
            StartCoroutine(EncenderMotorAR());
        }
    }

    IEnumerator EncenderMotorAR()
    {
        yield return new WaitForSecondsRealtime(1.0f);

        if (XRGeneralSettings.Instance.Manager.activeLoader != null)
        {
            XRGeneralSettings.Instance.Manager.StopSubsystems();
            XRGeneralSettings.Instance.Manager.DeinitializeLoader();
            yield return new WaitForSecondsRealtime(0.2f);
        }

        yield return XRGeneralSettings.Instance.Manager.InitializeLoader();

        if (XRGeneralSettings.Instance.Manager.activeLoader != null)
        {
            XRGeneralSettings.Instance.Manager.StartSubsystems();
            yield return new WaitForSecondsRealtime(0.2f);

            if (objetoARSession != null)
            {
                objetoARSession.SetActive(true);
                Debug.Log("G4: AR Session activado correctamente.");
            }
        }
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
            if (panelVictoria != null) panelVictoria.SetActive(true);
        }
    }

    public void BotonVolver()
    {
        if (MainManager.Instance != null)
            MainManager.Instance.FinalizarEscenaActual();
    }
}