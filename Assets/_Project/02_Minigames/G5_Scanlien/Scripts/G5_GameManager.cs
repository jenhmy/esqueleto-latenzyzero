using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.XR.Management;
using UnityEngine.SceneManagement;

// =========================================================================
// >>> G5_GAMEMANAGER: Control del nivel VR y transición al final del juego
// =========================================================================

public class G5_GameManager : MonoBehaviour
{
    [Header("Configuración de Juego")]
    public int itemsParaGanar = 2;
    public TextMeshProUGUI textoPuntos;
    public GameObject panelVictoria;

    [Header("Botones de Victoria (UI)")]
    public GameObject botonContinuar; // El que lleva a la foto final
    public GameObject botonSalir;     // El que vuelve al menú

    private int itemsActuales = 0;
    private int puntosTemporalesG5 = 0;

    private void Awake()
    {
        // Limpieza de cámaras sobrantes del simulador XR
        string[] intrusos = { "SimulationCamera", "XR Simulation Data" };
        foreach (string nombre in intrusos)
        {
            GameObject obj = GameObject.Find(nombre);
            if (obj != null) DestroyImmediate(obj);
        }
    }

    void Start()
    {
        // Al empezar el nivel, nos aseguramos de que el VR esté activo
        StartCoroutine(ReactivarXR());

        // Escondemos el panel de victoria por si acaso
        if (panelVictoria != null) panelVictoria.SetActive(false);
    }

    IEnumerator ReactivarXR()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        if (XRGeneralSettings.Instance.Manager.activeLoader == null)
        {
            yield return XRGeneralSettings.Instance.Manager.InitializeLoader();
        }

        if (XRGeneralSettings.Instance.Manager.activeLoader != null)
        {
            XRGeneralSettings.Instance.Manager.StartSubsystems();
        }
    }

    // Función que se llama cada vez que recoges algo en el G5
    public void ItemRecogido()
    {
        itemsActuales++;
        puntosTemporalesG5 += 20;

        if (textoPuntos != null)
            textoPuntos.text = "Puntos: " + puntosTemporalesG5;

        // Mandamos los puntos al MainManager para que se acumulen
        if (MainManager.Instance != null)
            MainManager.Instance.SumarPuntoTemporal(20);

        // Si ya tenemos todos los items, ganamos
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

            // Liberamos el ratón para poder hacer clic en el menú
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            if (MainManager.Instance != null)
            {
                // LEEMOS SI ESTAMOS EN HISTORIA
                bool esHistoria = MainManager.Instance.modoHistoriaActivo;

                // LÓGICA DE BOTONES:
                // El botón CONTINUAR solo se activa si es Modo Historia
                if (botonContinuar != null)
                    botonContinuar.SetActive(esHistoria);

                // El botón SALIR se activa siempre (para volver al menú)
                if (botonSalir != null)
                    botonSalir.SetActive(true);
            }
        }
    }

    // Esta función la conectaremos al botón "CONTINUAR" a través del UIMainManager
    // (O directamente aquí si lo prefieres)
    public void FinalizarNivel()
    {
        if (MainManager.Instance != null)
        {
            // Esta función del MainManager guarda puntos, apaga VR 
            // y decide si ir a "Transition" o al menú de niveles.
            MainManager.Instance.FinalizarEscenaActual();
        }
    }
}