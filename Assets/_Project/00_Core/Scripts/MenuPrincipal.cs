using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    // Función genérica para ahorrar código
    private void CargarNivelSuelto(string nombre)
    {
        if (MainManager.Instance != null)
        {
            MainManager.Instance.modoHistoriaActivo = false;
        }
        SceneManager.LoadScene(nombre);
    }

    public void JugarG1() => CargarNivelSuelto("G1_Inicial");
    public void JugarG2() => CargarNivelSuelto("G2_Inicial");
    public void JugarG3() => CargarNivelSuelto("G3_Inicial");
    public void JugarG4() => CargarNivelSuelto("G4_Inicial");
    public void JugarG5() => CargarNivelSuelto("G5_Inicial");

    public void VolverAlMainControl() => SceneManager.LoadScene("MainMenu");
}