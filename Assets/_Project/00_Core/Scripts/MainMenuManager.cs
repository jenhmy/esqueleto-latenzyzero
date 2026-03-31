using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void ClickHistoria()
    {
        // 1. Avisamos al MainManager que estamos en historia
        if (MainManager.Instance != null)
        {
            MainManager.Instance.modoHistoriaActivo = true;
            MainManager.Instance.pasoActual = 0;
            // 2. Cargamos la TRANSICIÓN, no el juego directo, para ver la Intro 1
            SceneManager.LoadScene("Transition");
        }
    }

    public void ClickSeleccion()
    {
        // Al ir a selección, nos aseguramos de que el modo historia esté apagado
        if (MainManager.Instance != null) MainManager.Instance.modoHistoriaActivo = false;
        SceneManager.LoadScene("MenuSeleccionJuegos");
    }

    public void SalirDelJuego()
    {
        Application.Quit();
    }
}