using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour
{

    public Rigidbody2D block;
    private Vector2 startPosition;
    public Rigidbody2D player;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = block.position;
        block.velocity = new Vector2(0f, -2f);
    }

    // Update is called once per frame
    void Update()
    {
        if (block.position.y > 8.5)
        {
            block.velocity = new Vector2(0f, -2f);
        }

        if (block.position.y < 3)
        {
            block.velocity = new Vector2(0f, 4f);
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Car")
        {
            player.position = new Vector2(0.39f, -0.49f);
            player.velocity = new Vector2(0f, 0f);
        }
    }

    public void startBlock()
    {
        block.velocity = new Vector2(0f, -2f);
    }
}
