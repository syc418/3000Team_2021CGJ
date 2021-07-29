using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundElement : MonoBehaviour
{
    public bool do_move;

    public Vector3 moving_offset;
    public float moving_speed;

    private Vector3 original_position;
    public Vector3 destination_position;

    public Collider2D collider;

    public void Start()
    {
        original_position = transform.position;
        destination_position = transform.position;
    }

    public void Update()
    {
        if (do_move) 
        {
            //get a new destination if already get to the end point
            if (Vector3.Distance(transform.position, destination_position) < 0.1) 
            {
                destination_position = new Vector3(original_position.x + Random.Range(-1,2) * moving_offset.x, original_position.y + Random.Range(-1,2) * moving_offset.y, original_position.z);
            }
            transform.position = Vector3.Lerp(transform.position, new Vector3(destination_position.x, destination_position.y, transform.position.z), Time.deltaTime * moving_speed);
        }
    }
}
