using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private PlayerMovement movement;

    void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        movement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (animator == null || spriteRenderer == null || movement == null)
        {
            return;
        }

        Vector2 arahGerak = movement.ArahGerak;
        Vector2 arahTerakhir = movement.LastDirection;

        bool sedangBergerak = arahGerak != Vector2.zero;

        animator.SetBool("isMoving", sedangBergerak);

        // DITAMBAHKAN: Tentukan animasi berdasarkan arah terakhir
        if (arahTerakhir.x != 0)
        {
            animator.SetFloat("MoveX", 1f);
            animator.SetFloat("MoveY", 0f);

            // DITAMBAHKAN: Balik sprite untuk arah kiri
            spriteRenderer.flipX = arahTerakhir.x < 0;
        }
        else
        {
            animator.SetFloat("MoveX", 0f);
            animator.SetFloat("MoveY", arahTerakhir.y);

            // DITAMBAHKAN: Kembalikan sprite untuk arah depan/belakang
            spriteRenderer.flipX = false;
        }
    }
}

