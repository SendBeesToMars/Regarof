using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // private Rigidbody2D rigid_body;
    private Vector3 direction;

    public Animator animator;
    public float speed;

    void Start() {
        // rigid_body = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        direction = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0.0f);

        // Debug.Log($"dir_x: {direction.x}, speed: {Mathf.Abs(direction.x) + Mathf.Abs(direction.y)}");

        if(direction != Vector3.zero){
            MoveCharacter();
        }
        else{
            animator.SetFloat("speed", Mathf.Abs(direction.x) + Mathf.Abs(direction.y));
        }
    }

    private void MoveCharacter() {
        animator.SetFloat("direction_x", direction.x);
        animator.SetFloat("direction_y", direction.y);
        animator.SetFloat("speed", Mathf.Abs(direction.x) + Mathf.Abs(direction.y));

        // flips sprite depending on which way the player is heading
        if(direction.x < 0) {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
            
        } else if(direction.x > 0) {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        // rigid_body.MovePosition(transform.position + direction.normalized * speed * Time.deltaTime);
        transform.position = transform.position + direction.normalized * speed * Time.deltaTime;
    }
}
