using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.XR.Management; // <--- AÑADE ESTO

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
        Debug.Log("Transición: Silenciando componentes de AR...");

        // 1. DESACTIVACIÓN QUIRÚRGICA
        // Buscamos todos los scripts activos
        MonoBehaviour[] todosLosScripts = Object.FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Exclude);

        foreach (var script in todosLosScripts)
        {
            if (script == null) continue;

            string ns = script.GetType().Namespace;
            // Si el script es de AR Foundation, lo desactivamos a la fuerza
            if (!string.IsNullOrEmpty(ns) && ns.Contains("UnityEngine.XR.ARFoundation"))
            {
                script.enabled = false;
            }
        }

        // 2. APAGÓN DE MOTOR (Igual que antes)
        if (XRGeneralSettings.Instance != null && XRGeneralSettings.Instance.Manager.isInitializationComplete)
        {
            XRGeneralSettings.Instance.Manager.StopSubsystems();
            XRGeneralSettings.Instance.Manager.DeinitializeLoader();
        }

        // 3. LIMPIEZA DE MEMORIA
        Resources.UnloadUnusedAssets();
        System.GC.Collect();

        yield return new WaitForSeconds(3f);
        MainManager.Instance.ContinuarHistoria();
    }
}