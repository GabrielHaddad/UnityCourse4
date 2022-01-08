using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    Rigidbody2D rb2d;
    [SerializeField] float moveSpeed = 1f;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        rb2d.velocity = new Vector2(moveSpeed, 0f);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        moveSpeed = -moveSpeed;
        FlipEnemy();
    }

    void FlipEnemy()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(rb2d.velocity.x)), 1f);
    }
}
