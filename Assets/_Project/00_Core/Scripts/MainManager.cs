using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;

    public MainDataManager baseDeDatos;

    [Header("Marcadores de Puntos")]
    public int puntosEnEsteMinijuego = 0;
    public int puntosTotalesVisualizar;

    [Header("Configuración de Historia")]
    public List<string> flujoEscenas;
    public List<string> textosIntros;
    public List<string> textosFinales;
    public int pasoActual = 0;
    public bool modoHistoriaActivo = false;

    private bool mostrandoFinal = false;

    [Header("Visuales de Historia")]
    public List<Sprite> fondosIntros;
    public List<Sprite> fondosFinales;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("MainManager inicializado correctamente.");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // --- FUNCIONES PARA LA ESCENA TRANSITION ---

    public string ObtenerTextoHistoria()
    {
        if (mostrandoFinal) return textosFinales[pasoActual];
        return textosIntros[pasoActual];
    }

    public Sprite ObtenerFondoActual()
    {
        if (mostrandoFinal) return fondosFinales[pasoActual];
        return fondosIntros[pasoActual];
    }

    // --- LÓGICA DE FLUJO ---

    public void ContinuarHistoria()
    {
        if (!mostrandoFinal)
        {
            string nivelACargar = flujoEscenas[pasoActual];
            mostrandoFinal = true;
            SceneManager.LoadScene(nivelACargar);
        }
        else
        {
            pasoActual++;
            mostrandoFinal = false;

            if (pasoActual < flujoEscenas.Count)
            {
                SceneManager.LoadScene("Transition");
            }
            else
            {
                SceneManager.LoadScene("PuntuacionFinal");
            }
        }
    }

    public void FinalizarEscenaActual()
    {
        ConfirmarPuntosYGuardar();

        if (modoHistoriaActivo)
        {
            SceneManager.LoadScene("Transition");
        }
        else
        {
            VolverAlMenuSeleccion();
        }
    }

    // --- RUTAS DE SALIDA ---

    public void VolverAlMenuSeleccion()
    {
        ResetearValores();
        SceneManager.LoadScene("MenuSeleccionJuegos");
    }

    public void VolverAlMainMenu()
    {
        ResetearValores();
        SceneManager.LoadScene("MainMenu");
    }

    private void ResetearValores()
    {
        modoHistoriaActivo = false;
        pasoActual = 0;
        mostrandoFinal = false;
        puntosEnEsteMinijuego = 0;
        if (baseDeDatos != null) baseDeDatos.ResetearProgreso();
        puntosTotalesVisualizar = 0;
    }

    // --- GESTIÓN DE PUNTOS ---

    public void SumarPuntoTemporal(int cantidad) => puntosEnEsteMinijuego += cantidad;

    public void ConfirmarPuntosYGuardar()
    {
        if (modoHistoriaActivo && baseDeDatos != null)
        {
            baseDeDatos.SumarPuntos(puntosEnEsteMinijuego);

            // ACTUALIZAMOS EL VISUAL puntosTotalesVisualizar AQUÍ:
            puntosTotalesVisualizar = baseDeDatos.puntosTotales;
        }
        puntosEnEsteMinijuego = 0;
    }
}