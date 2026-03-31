using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class TransitionController : MonoBehaviour
{
    public TextMeshProUGUI textoUI;
    public Image imagenFondo;

    void Start()
    {
        if (MainManager.Instance != null)
        {
            // Ponemos el texto que nos da el MainManager
            textoUI.text = MainManager.Instance.ObtenerTextoHistoria();

            // Cambiamos el fondo
            Sprite fondoNuevo = MainManager.Instance.ObtenerFondoActual();
            if (fondoNuevo != null)
            {
                imagenFondo.sprite = fondoNuevo;
            }

            StartCoroutine(EsperarYPasar());
        }
        else
        {
            Debug.LogWarning("¡Ojo! No hay MainManager en la escena. Asegúrate de empezar desde el Menú.");
        }
    }

    IEnumerator EsperarYPasar()
    {
        yield return new WaitForSeconds(3f);
        // Volvemos al MainManager para que nos mande a la siguiente escena
        MainManager.Instance.ContinuarHistoria();
    }
}