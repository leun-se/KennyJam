using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] LandInputSubscription GetInput;
    Rigidbody2D rb;


    Vector2 PlayerMovement;
    float speed = 5.0f;
    private void Update()
    {
        PlayerMovement = new Vector2(GetInput.MoveInput.x, GetInput.MoveInput.y);
        rb.linearVelocity = new Vector2(PlayerMovement.x, PlayerMovement.y) * speed;
    }
}
