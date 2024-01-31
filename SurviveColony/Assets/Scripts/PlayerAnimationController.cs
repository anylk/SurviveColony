using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetSpeed(Vector2 velocity, float dampTime, float deltaTime)
    {
        animator.SetFloat("velocityX", velocity.x, dampTime, deltaTime);
        animator.SetFloat("velocityY", velocity.y, dampTime, deltaTime);
    }

    public void Jump()
    {
        animator.SetTrigger("jump");
    }
}