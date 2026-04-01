using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.XR.Management;

// ==============================================================================
// >>> MAINMANAGER: Gestor global del juego (escenas, puntos, historia, Limpieza)
// ==============================================================================

public class MainManager : MonoBehaviour
{
    // REFERENCIAS
    public static MainManager Instance; // Para que el MainManager sobreviva (patrón Singleton) Para llamarlo: "MainManager.Instance.NombreFuncion()"
    public MainDataManager baseDeDatos; // Base de datos conectada

    // =========================================================================
    //                      CONFIGURACIÓN DEl INSPECTOR
    // =========================================================================
    [Header("Marcadores de Puntos")]
    public int puntosEnEsteMinijuego = 0; // Puntos temporales, aún no guardados
    public int puntosTotalesVisualizar; // Puntos a mostrar en la interfaz

    [Header("Lista de NOMBRES de escenas iniciales de niveles")]
    public List<string> listaEscenasIniciales; // Lista de nombres de escenas iniciales por nivel

    [Header("Configuración escenas")]
    public int indiceEscenasIniciales = 0; // ¿En qué número de la lista vamos?
    public bool modoHistoriaActivo = false; // ¿Estamos en modo historia o jugando un nivel suelto?
    private bool mostrandoFinal = false; // Controla si toca leer el texto de "antes" o "después" del nivel

    [Header("Recursos Visuales Intro")]
    public List<Sprite> fondosIntros; // Añadir los fondos introductorios
    public List<string> textosIntros; // Textos para las escenas de introducción por minijuego

    [Header("Recursos Visuales Final")]
    public List<Sprite> fondosFinales; // Añadir los fondos finales
    public List<string> textosFinales; // Textos para las escenas finales por minijuego
    

    private void Awake()
    {
    // ====================================================================================
    // 1. NÚCLEO DEL SISTEMA (SINGLETON) para MainManager: para que sobreviva entre escenas
    // ====================================================================================
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Sobrevivirá
        }
        else
        {
            Destroy(gameObject); // Se destruirá cualquier otro MainManager si se encuentra con él
        }
    }

    // ===============================================================================================
    // MODO HISTORIA: GESTIÓN DE ESCENAS DE TRANSICIÓN (Intro -> Juego -> Final -> Siguiente Intro)
    // ===============================================================================================
    public void ContinuarHistoria()
    {
        // CASO A: Estamos en la INTRO (mostrandoFinal es false)
        if (!mostrandoFinal)
        {            
            string nivelACargar = listaEscenasIniciales[indiceEscenasIniciales]; // 1. Busca el nombre del minijuego en la listaEscenasIniciales
            SceneManager.LoadScene(nivelACargar); // 2. Carga ese minijuego            
            mostrandoFinal = true; // 3. Cambia el bool para que lo próximo que veamos sea el texto final
        }
        else // CASO B: Estamos en el FINAL de un nivel (mostrandoFinal es true)
        {
            indiceEscenasIniciales++; // 1. Pasamos al siguiente número de la lista
            mostrandoFinal = false; // 2. Volvemos a poner el interruptor en "Intro" para el nuevo nivel

            if (indiceEscenasIniciales < listaEscenasIniciales.Count) // 3. ¿Quedan más niveles en la lista?
            {
                SceneManager.LoadScene("Transition"); // Sí: Vamos a la pantalla de transición para ver la intro del siguiente
            }
            else
            {
                SceneManager.LoadScene("PuntuacionFinal"); // No: Hemos terminado la lista, vamos a la pantalla de puntos finales
            }
        }
    }

    // Devuelve el TEXTO que se tiene que mostrar (escrito en el Inspector) mirando el indiceEscenasIniciales y mostrandoFinal true/false.
    public string ObtenerTextoHistoria()
    {
        if (mostrandoFinal) return textosFinales[indiceEscenasIniciales]; // Si el nivel ya terminó, devuelve el texto de "final" del nivel actual
        return textosIntros[indiceEscenasIniciales]; // Si el nivel va a empezar, devuelve el texto de "introducción" del nivel actual
    }

    // Devuelve el FONDO que se tiene que mostrar (sprite del Inspector) mirando el indiceEscenasIniciales y mostrandoFinal true/false.
    public Sprite ObtenerFondoActual()
    {
        if (mostrandoFinal) return fondosFinales[indiceEscenasIniciales]; // Si el nivel ya terminó, devuelve el fondo de "final"
        return fondosIntros[indiceEscenasIniciales]; // Si el nivel va a empezar, devuelve el fondo de "introducción"
    }

    // ===============================================================================================
    //                                 GESTIÓN DE CIERRE DE ESCENAS
    // =============================================================================================== 
    public void FinalizarEscenaActual()
    {
        // 1. PUNTOS: Pasa los puntos ganados en el nivel a la Base de Datos
        ConfirmarPuntosYGuardar();

        // 2. Limpieza de hardware y memoria
        LimpiarHardwareYMemoria();

        // 3. RUTA: Decide si vas a la siguiente parte de la Historia o te expulsa al Menú
        if (modoHistoriaActivo)
        {            
            SceneManager.LoadScene("Transition"); // Si hay historia, carga la escena de transición para ver el texto final
        }
        else
        {            
            VolverAlMenuSeleccion(); // Si no hay historia, limpia todo y te manda al selector de minijuegos
        }
    }

    // ===============================================================================================
    //                                 GESTIÓN DE PUNTOS
    // =============================================================================================== 

    // Acumula puntos durante el minijuego para ver por pantalla
    public void SumarPuntoTemporal(int cantidad)
    {
        puntosEnEsteMinijuego += cantidad;

        // Conexión coherente:
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("Punto"); 
        }
    }

    // Guarda los puntos en la "base de datos" 
    public void ConfirmarPuntosYGuardar()
    {
        if (modoHistoriaActivo && baseDeDatos != null)
        {
            baseDeDatos.SumarPuntos(puntosEnEsteMinijuego); // Sumamos a la "base de datos"
            puntosTotalesVisualizar = baseDeDatos.puntosTotales; // Actualizamos lo que se ve en pantalla
        }
        puntosEnEsteMinijuego = 0; // Vaciamos el contador temporal
    }

    // ===============================================================================================
    //                   RUTAS DE SALIDA Y REINICIO DE VALORES (RESET)
    // ===============================================================================================

    
    public void VolverAlMenuSeleccion() // Esta función es para cuando se utiliza el Modo Selección 
    {
        ResetearValores(); // Limpia todo el progreso antes de salir
        SceneManager.LoadScene("MenuSeleccionJuegos");
    }

    public void VolverAlMainMenu() // Esta función es para cuando se utiliza el Modo Historia y se acaba
    {
        ResetearValores(); // Limpia todo el progreso antes de salir
        SceneManager.LoadScene("MainMenu");
    }

    private void ResetearValores() // Limpia por completo el estado del juego a su estado inicial en cuanto a valores
    {
        LimpiarHardwareYMemoria(); //Limpieza de seguridad por si se sale del juego antes de lo previsto
        modoHistoriaActivo = false;
        indiceEscenasIniciales = 0;
        mostrandoFinal = false;
        puntosEnEsteMinijuego = 0;
        if (baseDeDatos != null) baseDeDatos.ResetearProgreso(); // Borra los puntos de la "base de datos"
        puntosTotalesVisualizar = 0;
    }

    private void LimpiarHardwareYMemoria()
    {
        // Verificamos si el motor XR (AR/VR) está inicializado y funcionando
        if (XRGeneralSettings.Instance != null && XRGeneralSettings.Instance.Manager.isInitializationComplete)
        {
            // IMPORTANTE: Solo detenemos los subsistemas (cámara, rastreo, renderizado).
            // Esto pone el hardware en reposo pero MANTIENE el driver cargado en RAM.
            XRGeneralSettings.Instance.Manager.StopSubsystems();

            // -------------------------------------------------------------------------
            // ADVERTENCIA: NO DESCOMENTAR 'DeinitializeLoader()'
            // -------------------------------------------------------------------------
            // XRGeneralSettings.Instance.Manager.DeinitializeLoader();
            // Si se desinicializa el Loader aquí, el motor tarda demasiado en cerrarse.
            // Cuando el siguiente nivel (Modo Historia) intenta arrancar el motor de nuevo,
            // se produce un conflicto de hardware que CONGELA la aplicación o da pantalla negra.
        }

        Resources.UnloadUnusedAssets(); // Libera texturas y modelos 3D que ya no se usan
        System.GC.Collect(); // Fuerza al GarbageCollector a limpiar la RAM
    }

}