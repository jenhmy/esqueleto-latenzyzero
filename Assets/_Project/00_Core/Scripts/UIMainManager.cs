using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // NECESARIO PARA CONTROLAR EL SLIDER

// =========================================================================
// >>> UIMAINMANAGER: Control de navegación, pausa y configuración de audio
// **Aquí van los botones compartidos, no los de minijuegos. Cada uno su propia UI por juego.** 
// =========================================================================

public class UIMainManager : MonoBehaviour
{
    [Header("Configuración de Audio (UI)")]
    public Slider sliderMusica; // Arrastra aquí el Slider de tu menú de pausa

    void Start()
    {
        // Al arrancar, si tenemos el slider y el manager de audio ya estaría
        if (sliderMusica != null && AudioManager.Instance != null)
        {
            // Ponemos el slider en la misma posición que esté el volumen actual
            sliderMusica.value = AudioManager.Instance.musicSource.volume;

            // Escuchamos cuando el usuario mueve el slider para cambiar el volumen
            sliderMusica.onValueChanged.AddListener(CambiarVolumenDesdeSlider);
        }
    }

    // ---------------------------------------------------------------------
    //                     ACCIONES DE AUDIO 
    // ---------------------------------------------------------------------

    public void CambiarVolumenDesdeSlider(float valor)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.musicSource.volume = valor;
        }
    }

    public void ReproducirSonidoBoton()
    {
        // Llama al sonido de "Click" de tu librería cada vez que pulses algo
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("Click");
        }
    }

    // ---------------------------------------------------------------------
    //                  PARA EL MENÚ PRINCIPAL 
    // ---------------------------------------------------------------------

    public void Boton_IniciarHistoria()
    {
        // 1. PRIMER TEST: ¿Llega el clic al script?
        Debug.Log("<color=cyan>UIMainManager:</color> Se ha pulsado el botón de Historia");

        ReproducirSonidoBoton();

        if (MainManager.Instance != null)
        {
            // 2. SEGUNDO TEST: ¿El MainManager existe?
            Debug.Log("<color=green>MainManager detectado.</color> Iniciando secuencia...");

            MainManager.Instance.modoHistoriaActivo = true;

            // Usamos ContinuarHistoria para que el cerebro decida qué escena toca
            MainManager.Instance.ContinuarHistoria();
        }
        else
        {
            // 3. TERCER TEST: Error crítico de referencia
            Debug.LogError("<color=red>Error:</color> No se encuentra el MainManager en la escena.");
        }
    }

    public void Boton_IrASeleccionNiveles()
    {
        ReproducirSonidoBoton();
        if (MainManager.Instance != null) MainManager.Instance.modoHistoriaActivo = false;
        SceneManager.LoadScene("MenuSeleccionJuegos");
    }

    public void Boton_SalirDelJuego()
    {
        ReproducirSonidoBoton();
        Debug.Log("Cerrando el juego...");
        Application.Quit();
    }

    // ---------------------------------------------------------------------
    //                  PARA EL SELECTOR DE NIVELES 
    // ---------------------------------------------------------------------

    public void CargarNivelSuelto(string nombreEscena)
    {
        ReproducirSonidoBoton();
        if (MainManager.Instance != null)
        {
            MainManager.Instance.modoHistoriaActivo = false;
        }
        SceneManager.LoadScene(nombreEscena);
    }

    public void Boton_JugarG1() => CargarNivelSuelto("G1_Inicial");
    public void Boton_JugarG2() => CargarNivelSuelto("G2_Inicial");
    public void Boton_JugarG3() => CargarNivelSuelto("G3_Inicial");
    public void Boton_JugarG4() => CargarNivelSuelto("G4_Inicial");
    public void Boton_JugarG5() => CargarNivelSuelto("G5_Inicial");

    // ---------------------------------------------------------------------
    //                     VOLVER ATRÁS Y CIERRE
    // ---------------------------------------------------------------------

    public void Boton_VolverAlMainMenu()
    {
        ReproducirSonidoBoton();
        SceneManager.LoadScene("MainMenu");
    }

    public void Boton_ResetTotal()
    {
        ReproducirSonidoBoton();
        if (MainManager.Instance != null)
        {
            MainManager.Instance.VolverAlMainMenu();
        }
    }

    public void Boton_AbandonarPartida()
    {
        ReproducirSonidoBoton();
        if (MainManager.Instance != null)
        {
            MainManager.Instance.VolverAlMenuSeleccion();
        }
    }

    public void Boton_FinalDelJuego()
    {
        ReproducirSonidoBoton();
        if (MainManager.Instance != null)
        {
            MainManager.Instance.FinalizarEscenaActual();
        }
    }

    // ---------------------------------------------------------------------
    //                      CONTROL DE FLUJO (HISTORIA)
    // ---------------------------------------------------------------------

    public void Boton_ContinuarHistoria()
    {
        ReproducirSonidoBoton();

        if (MainManager.Instance != null)
        {
            MainManager.Instance.ContinuarHistoria();
        }
    }

}