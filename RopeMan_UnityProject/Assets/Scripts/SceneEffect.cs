using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneEffect : MonoBehaviour
{
    public float pos = 0;
    public GameObject[] f;
    public GameObject[] c;
    public GameObject[] b;
    public const float f_speed = 2;
    public const float c_speed = 1;
    public const float b_speed = 0.5f;
    public Player[] players;
    private float last_pos = 0;

    private void Awake()
    {
        players = GameObject.FindObjectsOfType<Player>();
        pos = (players[0].transform.position.x + players[1].transform.position.x) / 2;
        last_pos = pos;
    }

    public void Update()
    {
        pos = pos = (players[0].transform.position.x + players[1].transform.position.x) / 2;
        var d = pos - last_pos;
        foreach (var item in f)
        {
            item.transform.position -= Vector3.right * d * f_speed*.1f;
        }
        foreach (var item in c)
        {
            item.transform.position -= Vector3.right * d * c_speed*.1f;

        }
        foreach (var item in b)
        {
            item.transform.position -= Vector3.right * d * b_speed*.1f;

        }
        last_pos = pos;
    }
}
