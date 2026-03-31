using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.XR.Management;

public class G5_GameManager : MonoBehaviour
{
    public int itemsParaGanar = 2;
    public TextMeshProUGUI textoPuntos;
    public GameObject panelVictoria;

    private int itemsActuales = 0;
    private int puntosTotales = 0;

    private void Awake()
    {
        // 1. Limpieza inmediata de cámaras huérfanas
        string[] intrusos = { "SimulationCamera", "XR Simulation Data" };
        foreach (string nombre in intrusos)
        {
            GameObject obj = GameObject.Find(nombre);
            if (obj != null) DestroyImmediate(obj);
        }

        // 2. IMPORTANTE: No arranques los subsistemas aquí si el MainManager ya los paró.
        // Deja que la Corrutina del Start se encargue de forma ordenada.
    }

    void Start()
    {
        // Iniciamos la resurrección del motor XR con tiempo de respiro
        StartCoroutine(ReactivarXR());
    }

    IEnumerator ReactivarXR()
    {
        Debug.Log("G5: Iniciando protocolo de seguridad...");

        Resources.UnloadUnusedAssets();
        System.GC.Collect();

        yield return new WaitForSecondsRealtime(0.5f);

        // --- MEJORA AQUÍ ---
        // Si por lo que sea el motor XR se quedó a medias o "tonto", 
        // forzamos una desinicialización limpia antes de intentar cargar el nuevo.
        if (XRGeneralSettings.Instance.Manager.isInitializationComplete)
        {
            XRGeneralSettings.Instance.Manager.StopSubsystems();
            XRGeneralSettings.Instance.Manager.DeinitializeLoader();
            yield return new WaitForSecondsRealtime(0.1f); // Un suspiro extra
        }
        // -------------------

        if (XRGeneralSettings.Instance.Manager.activeLoader == null)
        {
            Debug.Log("G5: Cargando Loader de VR...");
            yield return XRGeneralSettings.Instance.Manager.InitializeLoader();
        }

        if (XRGeneralSettings.Instance.Manager.activeLoader != null)
        {
            Debug.Log("G5: Activando Subsistemas VR.");
            XRGeneralSettings.Instance.Manager.StartSubsystems();
        }
        else
        {
            Debug.LogError("G5: No se pudo iniciar el VR.");
        }
    }

    // El resto de funciones (ItemRecogido, BotonVolver) se quedan igual
    public void ItemRecogido()
    {
        itemsActuales++;
        puntosTotales += 20;

        if (textoPuntos != null)
            textoPuntos.text = "Puntos: " + puntosTotales;

        MainManager main = Object.FindAnyObjectByType<MainManager>();
        if (main != null) main.SumarPuntoTemporal(20);

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
        MainManager main = Object.FindAnyObjectByType<MainManager>();
        if (main != null) main.FinalizarEscenaActual();
    }
}