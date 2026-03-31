using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.XR.Management;

public class G5_GameManager : MonoBehaviour
{
    [Header("Configuración de Juego")]
    public int itemsParaGanar = 2;
    public TextMeshProUGUI textoPuntos;
    public GameObject panelVictoria;

    private int itemsActuales = 0;
    private int puntosTotales = 0;

    private void Awake()
    {
        // Limpieza rápida por si quedaron restos visuales del editor (Simulation)
        string[] intrusos = { "SimulationCamera", "XR Simulation Data" };
        foreach (string nombre in intrusos)
        {
            GameObject obj = GameObject.Find(nombre);
            if (obj != null) DestroyImmediate(obj);
        }
    }

    void Start()
    {
        // Iniciamos el motor XR (VR) de forma controlada
        StartCoroutine(ReactivarXR());
    }

    IEnumerator ReactivarXR()
    {
        Debug.Log("G5: Iniciando VR para Modo Historia...");

        // Un pequeño respiro igual que en G4
        yield return new WaitForSecondsRealtime(0.5f);

        // Si el motor está "tonto" o inicializado a medias, lo limpiamos
        if (XRGeneralSettings.Instance.Manager.isInitializationComplete)
        {
            XRGeneralSettings.Instance.Manager.StopSubsystems();
            XRGeneralSettings.Instance.Manager.DeinitializeLoader();
            yield return new WaitForSecondsRealtime(0.1f);
        }

        // Cargamos el Loader de VR (PC o Móvil)
        if (XRGeneralSettings.Instance.Manager.activeLoader == null)
        {
            yield return XRGeneralSettings.Instance.Manager.InitializeLoader();
        }

        // Si todo ok, arrancamos subsistemas
        if (XRGeneralSettings.Instance.Manager.activeLoader != null)
        {
            XRGeneralSettings.Instance.Manager.StartSubsystems();
            Debug.Log("G5: VR Activado.");
        }
        else
        {
            Debug.LogError("G5: No se pudo iniciar el motor VR.");
        }
    }

    public void ItemRecogido()
    {
        itemsActuales++;
        puntosTotales += 20;

        if (textoPuntos != null)
            textoPuntos.text = "Puntos: " + puntosTotales;

        // Usamos la instancia directa del MainManager que ya sabemos que existe
        if (MainManager.Instance != null)
            MainManager.Instance.SumarPuntoTemporal(20);

        if (itemsActuales >= itemsParaGanar)
        {
            if (panelVictoria != null)
            {
                panelVictoria.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }

    public void BotonVolver()
    {
        if (MainManager.Instance != null)
            MainManager.Instance.FinalizarEscenaActual();
    }
}