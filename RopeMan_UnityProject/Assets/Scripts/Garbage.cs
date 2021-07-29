using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garbage : MonoBehaviour
{
    public Vector2 speed;
    SpriteRenderer sr;
    Rigidbody2D rig;
    public bool is_alive = true;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

    }

    public void SetSprite(Sprite sp)
    {
        sr.sprite = sp;
        var p = GetComponent<PolygonCollider2D>();
        p.enabled = true;
        p.SetPath(0, sr.sprite.vertices);

    }    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
