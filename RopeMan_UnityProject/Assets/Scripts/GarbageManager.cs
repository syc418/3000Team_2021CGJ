using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Level
{
    ONE,
    TWO,
    THREE
}
public class GarbageManager : MonoBehaviour
{
    public Garbage res;
    public Level level;
    private float timer = 0;
    public float cd = 0.5f;
    public float timeRandomRange = 0.2f;
    public float randomCountMin = 1;
    public float randomCountMax = 3;

    public readonly Rect level1scanout = new Rect(0, 8, 19, 5);

    public void Update()
    {
        timer -= Time.deltaTime;
        if(timer<=0)
        {
            timer = cd + Random.Range(-timeRandomRange, timeRandomRange);
            for (int i = 0; i < Random.Range(randomCountMin,randomCountMax); i++)
            {
                switch (level)
                {
                    case Level.ONE:
                        CreateGarbage1();
                        break;
                    case Level.TWO:
                        CreateGarbage2();
                        break;
                    case Level.THREE:
                        CreateGarbage3();
                        break;
                    default:
                        break;
                }
                
            }

        }
    }


    private void CreateGarbage1()
    {

         GameObject g = GameObject.Instantiate(res.gameObject);
        Garbage gar = g.GetComponent<Garbage>();
        gar.is_alive = true;
        g.gameObject.SetActive(true);
        var x = Random.Range(level1scanout.x + level1scanout.width / 2, level1scanout.x - level1scanout.width / 2);
        var y = Random.Range(level1scanout.y + level1scanout.height / 2, level1scanout.y - level1scanout.height / 2);
     
        Vector3 randomV3 = new Vector3(x, y);
        g.transform.position = randomV3;
        var rig = g.GetComponent<Rigidbody2D>();
        rig.velocity = new Vector2(Random.Range(-0.5f,0.5f),-1)*Random.Range(2,3);
        rig.AddTorque(Random.Range(-.5f, .5f));
        var sp = ResMgr.Instance.garbages[Random.Range(0, ResMgr.Instance.garbages.Length)];
        gar.SetSprite(sp);
   
        
 
    }

    private void CreateGarbage2()
    {
        var dir = Random.Range(0, 4);
        float x=0;
        float y=0;
        float dir_x = 0; float dir_y = 0;float dir_xm = 0; float dir_ym = 0;
        float speedx = 0;
        float speedy = 0;
        switch (dir)
        {
            case 0:
                x = Random.Range(level1scanout.x + level1scanout.width / 2, level1scanout.x - level1scanout.width / 2);
                y = Random.Range(level1scanout.y + level1scanout.height / 2, level1scanout.y - level1scanout.height / 2);
                dir_x = -0.5f;
                dir_xm = 0.5f;
                dir_y = -1;
                dir_ym = -1.1f;
                break;
            case 1:
                x = Random.Range(level1scanout.x + level1scanout.width / 2, level1scanout.x - level1scanout.width / 2);
                y = -Random.Range(level1scanout.y + level1scanout.height / 2, level1scanout.y - level1scanout.height / 2);
                dir_x = -0.5f;
                dir_xm = 0.5f;
                dir_y = 1;
                dir_ym = 1.1f;
                break;
            case 2:
                x = Random.Range(-13f, -15f);
                y = -Random.Range(-5f, -5f);
                dir_y = -0.5f;
                dir_ym = 0.5f;
                dir_x = 1;
                dir_xm = 1.1f;
                break;
            case 3:
                x = Random.Range(13f, 15f);
                y = -Random.Range(-5f, 5f);
                dir_y = -0.5f;
                dir_ym = 0.5f;
                dir_x = -1;
                dir_xm = -1.1f;
                break;
            default:
                break;
        }

        GameObject g = GameObject.Instantiate(res.gameObject);
        Garbage gar = g.GetComponent<Garbage>();
        g.transform.localScale = Vector3.one * 0.6f;
        gar.is_alive = true;
        g.gameObject.SetActive(true);


        Vector3 randomV3 = new Vector3(x, y);
        g.transform.position = randomV3;
        var rig = g.GetComponent<Rigidbody2D>();
        rig.velocity = new Vector2(Random.Range(dir_x, dir_xm), Random.Range(dir_y, dir_ym)) * Random.Range(4, 5);
        rig.AddTorque(Random.Range(-1f, 1f));
        var sp = ResMgr.Instance.garbages[Random.Range(0, ResMgr.Instance.garbages.Length)];
        gar.SetSprite(sp);



    }

    private void CreateGarbage3()
    {

        GameObject g = GameObject.Instantiate(res.gameObject);
        Garbage gar = g.GetComponent<Garbage>();
        gar.is_alive = true;
        g.gameObject.SetActive(true);
        var x = Random.Range(level1scanout.x + level1scanout.width / 2, level1scanout.x - level1scanout.width / 2);
        var y = -Random.Range(level1scanout.y + level1scanout.height / 2, level1scanout.y - level1scanout.height / 2);

        Vector3 randomV3 = new Vector3(x, y);
        g.transform.position = randomV3;
        var rig = g.GetComponent<Rigidbody2D>();
        rig.velocity = new Vector2(Random.Range(-0.8f, 0.8f), 1) * Random.Range(2, 6);
        rig.AddTorque(Random.Range(-4f, 4f));
        var sp = ResMgr.Instance.garbages[Random.Range(0, ResMgr.Instance.garbages.Length)];
        gar.SetSprite(sp);



    }
}
