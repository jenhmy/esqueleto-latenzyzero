using UnityEngine;

// Es el disco duro. Guarda solo datos (puntos y historia activa o no)

[CreateAssetMenu(fileName = "MainManager", menuName = "Sistema/Main Manager")]
public class MainDataManager : ScriptableObject
{
    [Header("Progreso Global")]
    public int puntosTotales = 0;

    [Header("Ajustes de Partida")]
    public bool modoHistoriaActivo = false;

    public void ResetearProgreso()
    {
        puntosTotales = 0;
    }

    // Esta función la llamarán tus 01_GameManager, 02_GameManager, etc.
    public void SumarPuntos(int puntosNivel)
    {
        puntosTotales += puntosNivel;
        Debug.Log("Puntos actualizados en el MainManager: " + puntosTotales);
    }
}