using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instancia;
    public Vector2 moveDirection;
    private MainInputControls controls;

    private void Awake()
    {
        if (Instancia == null)
        {
            Instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        controls = new MainInputControls();
        controls.Player.Move.performed += ctx => moveDirection = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveDirection = Vector2.zero;
    }

    private void OnEnable() => controls?.Enable();
    private void OnDisable() => controls?.Disable();

    public Vector2 GetMovimiento() => moveDirection;
}