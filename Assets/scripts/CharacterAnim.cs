using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerCharacter))]
[RequireComponent(typeof(Animator))]
public class CharacterAnim : MonoBehaviour
{
    private PlayerCharacter character;
    private Animator animator;

    void Awake()
    {
        character = GetComponent<PlayerCharacter>();
        animator = GetComponent<Animator>();

        character.onJump += OnJump;
        character.onCrouch += OnCrouch;
    }

    void Update()
    {
        if (character == null || animator == null) return;

        // Set running speed using horizontal speed (avoids issues with vertical movement)
        float hSpeed = character.GetHorizontalSpeed();
        animator.SetFloat("Speed", hSpeed);

        // Jumping & falling
        animator.SetBool("Jumping", character.IsJumping());
        animator.SetBool("InAir", !character.IsGrounded());

        // Crouching
        animator.SetBool("Crouching", character.IsCrouching());
    }

    // Triggers for jump animation
    private void OnJump()
    {
        animator.SetTrigger("Jump");
    }

    // Triggers for crouch animation
    private void OnCrouch()
    {
        animator.SetTrigger("Crouch");
    }
}
