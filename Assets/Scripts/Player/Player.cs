
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public InputSystem_Actions inputSystem;
    private Rigidbody2D playerRb;

    [SerializeField] private float moveSpeed = 1f;
    void Awake()
    {
        inputSystem = new InputSystem_Actions();
        playerRb = GetComponent<Rigidbody2D>();
    }
    void OnEnable()
    {
        inputSystem.Player.Enable();
    }
    void OnDisable()
    {
        inputSystem.Player.Disable();
    }

    void FixedUpdate()
    {
        Vector2 movementInput = inputSystem.Player.Movement.ReadValue<Vector2>();
        playerRb.linearVelocity = moveSpeed * movementInput;
    }

}

