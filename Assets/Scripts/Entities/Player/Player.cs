
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour, IDamageable
{
    public InputSystem_Actions inputSystem;
    private Rigidbody2D playerRb;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Vector2 moveInput;
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

    void Update()
    {
        moveInput = inputSystem.Player.Movement.ReadValue<Vector2>();
    }
    void FixedUpdate()
    {

        playerRb.linearVelocity = moveSpeed * moveInput;
    }

    public void TakeDamage(float damageAmount)
    {
        // TODO - Implement damage logic here~
        Debug.Log($"Player took {damageAmount} damage.");
        
    }
}

