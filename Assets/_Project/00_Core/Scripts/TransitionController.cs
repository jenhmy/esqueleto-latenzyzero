using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.XR.Management;

public class TransitionController : MonoBehaviour
{
    public TextMeshProUGUI textoUI;
    public Image imagenFondo;

    void Start()
    {
        if (MainManager.Instance != null)
        {
            textoUI.text = MainManager.Instance.ObtenerTextoHistoria();

            Sprite fondoNuevo = MainManager.Instance.ObtenerFondoActual();
            if (fondoNuevo != null)
            {
                imagenFondo.sprite = fondoNuevo;
            }

            // Ejecutamos la limpieza nada más empezar la transición
            StartCoroutine(LimpiarYPasar());
        }
        else
        {
            Debug.LogWarning("No hay MainManager.");
        }
    }

    IEnumerator LimpiarYPasar()
    {
        Debug.Log("Transición: Limpiando memoria...");

        // Limpieza de basura de la escena anterior
        Resources.UnloadUnusedAssets();
        System.GC.Collect();

        yield return new WaitForSeconds(3f); // Tiempo para leer la historia

        if (MainManager.Instance != null)
        {
            MainManager.Instance.ContinuarHistoria();
        }
    }
}