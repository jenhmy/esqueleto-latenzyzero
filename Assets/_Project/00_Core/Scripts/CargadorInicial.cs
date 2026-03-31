using UnityEngine;
using UnityEngine.SceneManagement;

public class CargadorInicial : MonoBehaviour
{
    void Start()
    {
        // Al usar Start, nos aseguramos de que el MainManager ya despertó en su Awake
        Debug.Log("Saltando al MainMenu...");
        SceneManager.LoadScene("MainMenu");
    }
}