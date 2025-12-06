
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour, IDamageable
{
    private InputSystem_Actions inputSystem;
    private Rigidbody2D playerRb;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Vector2 moveInput;
    [SerializeField, Range(1f, 50f)] private float debugRange;
    private void Awake()
    {
        inputSystem = new InputSystem_Actions();
        playerRb = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        inputSystem.Player.Enable();
    }
    private void OnDisable()
    {
        inputSystem.Player.Disable();
    }

    private void Update()
    {
        moveInput = inputSystem.Player.Movement.ReadValue<Vector2>();
    }
    private void FixedUpdate()
    {

        playerRb.linearVelocity = moveSpeed * moveInput;
    }

    public void TakeDamage(float damageAmount)
    {
        // TODO - Implement taking damage logic here


    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.purple;
        Gizmos.DrawWireSphere(transform.position, debugRange);
    }
}

