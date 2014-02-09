using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerShipRenderer : MonoBehaviour
{
    private Material line_material_;
    private List<Vector3> vertices_;
    private Color line_color_;
    private Color square_color_;

    // Use this for initialization
    void Start()
    {
        CreateLineMaterial();
        Vector3 p1 = new Vector3(-1.0f, -1.0f, 0.0f);
        Vector3 p2 = new Vector3(0.0f, 1.0f, 0.0f);
        Vector3 p3 = new Vector3(1.0f, -1.0f, 0.0f);
        Vector3 p4 = new Vector3(0.5f, -0.75f, 0.0f);
        Vector3 p5 = new Vector3(-0.5f, -0.75f, 0.0f);
        vertices_ = new List<Vector3>();
        vertices_.Add(p1);
        vertices_.Add(p2);
        vertices_.Add(p3);
        vertices_.Add(p4);
        vertices_.Add(p5);

        line_color_ = new Color(1.0f, 1.0f, 1.0f, 1.0f);
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
