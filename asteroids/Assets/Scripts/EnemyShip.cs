using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyShip : MonoBehaviour
{
    public float velocity_;
    public float projectile_speed_;
    public float shooting_delay_;
    public Projectile projectile_;
    public AudioClip projectile_sound_;
    public ParticleSystem ship_explosion_prefab_;    
    public float big_ufo_min_angle_;
    public float big_ufo_max_angle_;
    public float small_ufo_min_angle_;
    public float small_ufo_max_angle_;
    public int small_ufo_score_;
    public int big_ufo_score_;    

    private Material line_material_;
    private List<Vector3> vertices_;
    private Color line_color_;
    private Color square_color_;
    private float min_angle_;
    private float max_angle_;
    private int score_;

    void Awake()
    {
        CreateLineMaterial();
        Vector3 p1 = new Vector3(-0.5f, -1.0f, 0.0f);
        Vector3 p2 = new Vector3(-1.0f, -0.25f, 0.0f);
        Vector3 p3 = new Vector3(-0.5f, 0.25f, 0.0f);
        Vector3 p4 = new Vector3(-0.25f, 1.0f, 0.0f);
        Vector3 p5 = new Vector3(0.25f, 1.0f, 0.0f);
        Vector3 p6 = new Vector3(0.5f, 0.25f, 0.0f);
        Vector3 p7 = new Vector3(1.0f, -0.25f, 0.0f);
        Vector3 p8 = new Vector3(0.5f, -1.0f, 0.0f);


        vertices_ = new List<Vector3>();
        vertices_.Add(p1);
        vertices_.Add(p2);
        vertices_.Add(p3);
        vertices_.Add(p4);
        vertices_.Add(p5);
        vertices_.Add(p6);
        vertices_.Add(p7);
        vertices_.Add(p8);

        line_color_ = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        square_color_ = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        min_angle_ = big_ufo_min_angle_;
        max_angle_ = big_ufo_max_angle_;
        score_ = big_ufo_score_;
    }

    // Use this for initialization
    void Start()
    {        
        InvokeRepeating("Shoot", 1.0f, shooting_delay_);        
    }

    public void SetSmall()
    {        
        transform.localScale = transform.localScale / 1.5f;
        min_angle_ = small_ufo_min_angle_;
        max_angle_ = small_ufo_max_angle_;
        score_ = small_ufo_score_;
    }    

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {

    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.tag == "Asteroid" || c.gameObject.tag == "PlayerProjectile" || c.gameObject.tag == "Player")
        {
            ParticleSystem ship_explosion_instance = (ParticleSystem)Instantiate(ship_explosion_prefab_, transform.position, transform.rotation);
            ship_explosion_instance.gameObject.GetComponent<AudioSource>().Play();
            if (c.gameObject.tag == "PlayerProjectile" || c.gameObject.tag == "Player")
            {
                GameObject.Find("GameController").GetComponent<GameController>().AddScore(score_);
            }
            Destroy(gameObject);            
        }
    }

    void Shoot()
    {
        GameObject player_ship = GameObject.FindGameObjectWithTag("Player");
        Vector3 dir = new Vector3(0.0f, 1.0f, 0.0f);
        if (player_ship != null)
        {
            dir = player_ship.transform.position - transform.position;
            dir.Normalize();
        }
        Projectile p = (Projectile)GameObject.Instantiate(projectile_, transform.position, transform.rotation);
        p.tag = "EnemyProjectile";
        p.gameObject.layer = 12;

        float projectile_precision = Random.Range(min_angle_, max_angle_);                
        dir = Quaternion.Euler(0.0f, 0.0f, projectile_precision) * dir;
        p.rigidbody2D.velocity = dir * projectile_speed_ * Time.deltaTime;
        p.GetComponent<Projectile>().max_time_alive_ = 3.0f;

        gameObject.GetComponents<AudioSource>()[0].Play();        
    }

    void CreateLineMaterial()
    {
        if (!line_material_)
        {
            line_material_ = new Material("Shader \"Lines/Colored Blended\" {" +
                    "SubShader { Pass { " +
                    "    Blend SrcAlpha OneMinusSrcAlpha " +
                    "    ZWrite Off Cull Off Fog { Mode Off } " +
                    "    BindChannels {" +
                    "      Bind \"vertex\", vertex Bind \"color\", color }" +
                    "} } }");
            line_material_.hideFlags = HideFlags.HideAndDontSave;
            line_material_.shader.hideFlags = HideFlags.HideAndDontSave;
        }
    }

    void OnGUI()
    {
        RenderShip();
    }

    void RenderShip()
    {
        line_material_.SetPass(0);
        GL.PushMatrix();
        Matrix4x4 trs_matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
        GL.MultMatrix(trs_matrix);
        //DrawBoundingBox();
        DrawLines();
        //DrawSquares(0.1f);
        GL.PopMatrix();
    }

    void DrawLines()
    {
        GL.Color(line_color_);
        GL.Begin(GL.LINES);
        for (int i = 0; i < vertices_.Count; i++)
        {
            GL.Vertex(vertices_[i]);
            if (i < vertices_.Count - 1)
            {
                GL.Vertex(vertices_[i + 1]);
            }
        }
        GL.Vertex(vertices_[0]);
        GL.Vertex(vertices_[1]);
        GL.Vertex(vertices_[6]);

        GL.Vertex(vertices_[2]);
        GL.Vertex(vertices_[5]);
        GL.End();
    }

    void DrawSquares(float size)
    {
        float half_size = size / 2.0f;
        GL.Color(square_color_);
        GL.Begin(GL.QUADS);
        for (int i = 0; i < vertices_.Count; i++)
        {
            Vector3 vertice = vertices_[i];
            GL.Vertex3(vertice.x - half_size, vertice.y - half_size, 0.0f);
            GL.Vertex3(vertice.x + half_size, vertice.y - half_size, 0.0f);
            GL.Vertex3(vertice.x + half_size, vertice.y + half_size, 0.0f);
            GL.Vertex3(vertice.x - half_size, vertice.y + half_size, 0.0f);
        }
        GL.End();
    }

    void DrawBoundingBox()
    {
        BoxCollider2D bc = (BoxCollider2D)gameObject.GetComponent<BoxCollider2D>();

        GL.Color(square_color_);
        GL.Begin(GL.LINES);
        GL.Vertex3(bc.center.x - bc.size.x / 2.0f, bc.center.y - bc.size.y / 2.0f, 0.0f);
        GL.Vertex3(bc.center.x + bc.size.x / 2.0f, bc.center.y - bc.size.y / 2.0f, 0.0f);

        GL.Vertex3(bc.center.x + bc.size.x / 2.0f, bc.center.y - bc.size.y / 2.0f, 0.0f);
        GL.Vertex3(bc.center.x + bc.size.x / 2.0f, bc.center.y + bc.size.y / 2.0f, 0.0f);

        GL.Vertex3(bc.center.x + bc.size.x / 2.0f, bc.center.y + bc.size.y / 2.0f, 0.0f);
        GL.Vertex3(bc.center.x - bc.size.x / 2.0f, bc.center.y + bc.size.y / 2.0f, 0.0f);

        GL.Vertex3(bc.center.x - bc.size.x / 2.0f, bc.center.y + bc.size.y / 2.0f, 0.0f);
        GL.Vertex3(bc.center.x - bc.size.x / 2.0f, bc.center.y - bc.size.y / 2.0f, 0.0f);

        GL.End();
    }

}

