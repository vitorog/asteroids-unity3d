using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Asteroid : MonoBehaviour
{

    private Material line_material_;
    private List<Vector3> vertices_;
    private Color line_color_;
    private Color square_color_;
    private int type_;

    // Use this for initialization
    void Start()
    {
        CreateLineMaterial();
        type_ = Random.Range(1, 5);        
        vertices_ = new List<Vector3>();
        switch (type_)
        {
            case 1:
                {
                    Vector3 p1 = new Vector3(-0.5f, -1.0f, 0.0f);
                    Vector3 p2 = new Vector3(-1.0f, -0.5f, 0.0f);
                    Vector3 p3 = new Vector3(-1.0f, 0.5f, 0.0f);
                    Vector3 p4 = new Vector3(-0.5f, 1.0f, 0.0f);
                    Vector3 p5 = new Vector3(0.0f, 0.5f, 0.0f);
                    Vector3 p6 = new Vector3(0.5f, 1.0f, 0.0f);
                    Vector3 p7 = new Vector3(1.0f, 0.75f, 0.0f);
                    Vector3 p8 = new Vector3(0.75f, 0.1f, 0.0f);
                    Vector3 p9 = new Vector3(1.0f, -0.5f, 0.0f);
                    Vector3 p10 = new Vector3(0.5f, -1.0f, 0.0f);


                    vertices_.Add(p1);
                    vertices_.Add(p2);
                    vertices_.Add(p3);
                    vertices_.Add(p4);
                    vertices_.Add(p5);
                    vertices_.Add(p6);
                    vertices_.Add(p7);
                    vertices_.Add(p8);
                    vertices_.Add(p9);
                    vertices_.Add(p10);
                }
                break;
            case 2:
                {
                    Vector3 p1 = new Vector3(-0.5f, -1.0f, 0.0f);
                    Vector3 p2 = new Vector3(-1.0f, -0.5f, 0.0f);
                    Vector3 p3 = new Vector3(-0.75f, 0.0f, 0.0f);
                    Vector3 p4 = new Vector3(-1.0f, 0.5f, 0.0f);
                    Vector3 p5 = new Vector3(-0.5f, 1.0f, 0.0f);
                    Vector3 p6 = new Vector3(0.0f, 0.75f, 0.0f);
                    Vector3 p7 = new Vector3(0.5f, 1.0f, 0.0f);
                    Vector3 p8 = new Vector3(1.0f, 0.5f, 0.0f);
                    Vector3 p9 = new Vector3(0.5f, 0.25f, 0.0f);
                    Vector3 p10 = new Vector3(1.0f, -0.25f, 0.0f);
                    Vector3 p11 = new Vector3(0.5f, -1.0f, 0.0f);
                    Vector3 p12 = new Vector3(-0.25f, -0.75f, 0.0f);

                    vertices_.Add(p1);
                    vertices_.Add(p2);
                    vertices_.Add(p3);
                    vertices_.Add(p4);
                    vertices_.Add(p5);
                    vertices_.Add(p6);
                    vertices_.Add(p7);
                    vertices_.Add(p8);
                    vertices_.Add(p9);
                    vertices_.Add(p10);
                    vertices_.Add(p11);
                    vertices_.Add(p12);
                }
                break;
            case 3:
                {
                    Vector3 p1 = new Vector3(-0.5f, -1.0f, 0.0f);
                    Vector3 p2 = new Vector3(-1.0f, -0.25f, 0.0f);
                    Vector3 p3 = new Vector3(-0.5f, 0.0f, 0.0f);
                    Vector3 p4 = new Vector3(-1.0f, 0.25f, 0.0f);
                    Vector3 p5 = new Vector3(-0.5f, 1.0f, 0.0f);
                    Vector3 p6 = new Vector3(0.5f, 1.0f, 0.0f);
                    Vector3 p7 = new Vector3(1.0f, 0.25f, 0.0f);
                    Vector3 p8 = new Vector3(0.5f, -1.0f, 0.0f);
                    Vector3 p9 = new Vector3(0.0f, -1.0f, 0.0f);
                    Vector3 p10 = new Vector3(0.0f, -0.25f, 0.0f);

                    vertices_.Add(p1);
                    vertices_.Add(p2);
                    vertices_.Add(p3);
                    vertices_.Add(p4);
                    vertices_.Add(p5);
                    vertices_.Add(p6);
                    vertices_.Add(p7);
                    vertices_.Add(p8);
                    vertices_.Add(p9);
                    vertices_.Add(p10);
                }
                break;
            case 4:
                {
                    Vector3 p1 = new Vector3(-0.5f, -1.0f, 0.0f);
                    Vector3 p2 = new Vector3(-1.0f, -0.25f, 0.0f);
                    Vector3 p3 = new Vector3(-1.0f, 0.5f, 0.0f);
                    Vector3 p4 = new Vector3(-0.25f, 0.5f, 0.0f);
                    Vector3 p5 = new Vector3(-0.5f, 1.0f, 0.0f);                    
                    Vector3 p6 = new Vector3(0.25f, 1.0f, 0.0f);
                    Vector3 p7 = new Vector3(1.0f, 0.5f, 0.0f);
                    Vector3 p8 = new Vector3(0.25f, 0.0f, 0.0f);
                    Vector3 p9 = new Vector3(1.0f, -0.5f, 0.0f);
                    Vector3 p10 = new Vector3(0.25f, -1.0f, 0.0f);

                    vertices_.Add(p1);
                    vertices_.Add(p2);
                    vertices_.Add(p3);
                    vertices_.Add(p4);
                    vertices_.Add(p5);
                    vertices_.Add(p6);
                    vertices_.Add(p7);
                    vertices_.Add(p8);
                    vertices_.Add(p9);
                    vertices_.Add(p10);                    
                }
                break;
        }


        line_color_ = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        square_color_ = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {

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
        DrawLines();
        DrawSquares(0.02f);
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
}
