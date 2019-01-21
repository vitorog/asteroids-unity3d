using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AsteroidRenderer : MonoBehaviour {

    private List<Asteroid> asteroids_;
    private Color line_color_;
    private Color square_color_;
    private Material line_material_;
    private List<List<Vector3>> asteroids_vertices_;

    public void AddObject(Asteroid asteroid, int type)
    {        
        List<Vector3> vertices = new List<Vector3>();
        switch (type)
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


                    vertices.Add(p1);
                    vertices.Add(p2);
                    vertices.Add(p3);
                    vertices.Add(p4);
                    vertices.Add(p5);
                    vertices.Add(p6);
                    vertices.Add(p7);
                    vertices.Add(p8);
                    vertices.Add(p9);
                    vertices.Add(p10);
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

                    vertices.Add(p1);
                    vertices.Add(p2);
                    vertices.Add(p3);
                    vertices.Add(p4);
                    vertices.Add(p5);
                    vertices.Add(p6);
                    vertices.Add(p7);
                    vertices.Add(p8);
                    vertices.Add(p9);
                    vertices.Add(p10);
                    vertices.Add(p11);
                    vertices.Add(p12);
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

                    vertices.Add(p1);
                    vertices.Add(p2);
                    vertices.Add(p3);
                    vertices.Add(p4);
                    vertices.Add(p5);
                    vertices.Add(p6);
                    vertices.Add(p7);
                    vertices.Add(p8);
                    vertices.Add(p9);
                    vertices.Add(p10);
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

                    vertices.Add(p1);
                    vertices.Add(p2);
                    vertices.Add(p3);
                    vertices.Add(p4);
                    vertices.Add(p5);
                    vertices.Add(p6);
                    vertices.Add(p7);
                    vertices.Add(p8);
                    vertices.Add(p9);
                    vertices.Add(p10);
                }
                break;
        }
        asteroids_vertices_.Add(vertices);
        asteroids_.Add(asteroid);
    }

	// Use this for initialization
	void Awake () {
        CreateLineMaterial();
        line_color_ = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        square_color_ = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        asteroids_ = new List<Asteroid>();
        asteroids_vertices_ = new List< List<Vector3> >();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void CreateLineMaterial()
    {
        if (!line_material_)
        {
            line_material_ = new Material(Shader.Find("Lines/Colored Blended"));
            line_material_.hideFlags = HideFlags.HideAndDontSave;
            line_material_.shader.hideFlags = HideFlags.HideAndDontSave;
        }
    }

    void OnPostRender()
    {        
        for (int i = asteroids_.Count - 1; i >= 0; i--)
        {
            if (asteroids_[i] != null && asteroids_[i].IsAlive())
            {
                RenderAsteroid(asteroids_[i], i);
            }
            else
            {
                asteroids_.RemoveAt(i);
                asteroids_vertices_.RemoveAt(i);
            }
        }
        
    }

    void RenderAsteroid(Asteroid asteroid, int index)
    {
        line_material_.SetPass(0);
        GL.PushMatrix();
        Matrix4x4 trs_matrix = Matrix4x4.TRS(asteroid.gameObject.transform.position, asteroid.gameObject.transform.rotation, asteroid.gameObject.transform.localScale);
        GL.MultMatrix(trs_matrix);
        //DrawBoundingBox();
        DrawLines(index);
        DrawSquares(0.02f, index);
        GL.PopMatrix();
    }

    void DrawLines(int index)
    {
        GL.Color(line_color_);
        GL.Begin(GL.LINES);
        for (int i = 0; i < asteroids_vertices_[index].Count; i++)
        {
            GL.Vertex(asteroids_vertices_[index][i]);
            if (i < asteroids_vertices_[index].Count - 1)
            {
                GL.Vertex(asteroids_vertices_[index][i + 1]);
            }
        }
        GL.Vertex(asteroids_vertices_[index][0]);
        GL.End();
    }

    void DrawSquares(float size, int index)
    {
        float half_size = size / 2.0f;
        GL.Color(square_color_);
        GL.Begin(GL.QUADS);
        for (int i = 0; i < asteroids_vertices_[index].Count; i++)
        {
            Vector3 vertice = asteroids_vertices_[index][i];
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
        GL.Vertex3(bc.offset.x - bc.size.x / 2.0f, bc.offset.y - bc.size.y / 2.0f, 0.0f);
        GL.Vertex3(bc.offset.x + bc.size.x / 2.0f, bc.offset.y - bc.size.y / 2.0f, 0.0f);

        GL.Vertex3(bc.offset.x + bc.size.x / 2.0f, bc.offset.y - bc.size.y / 2.0f, 0.0f);
        GL.Vertex3(bc.offset.x + bc.size.x / 2.0f, bc.offset.y + bc.size.y / 2.0f, 0.0f);

        GL.Vertex3(bc.offset.x + bc.size.x / 2.0f, bc.offset.y + bc.size.y / 2.0f, 0.0f);
        GL.Vertex3(bc.offset.x - bc.size.x / 2.0f, bc.offset.y + bc.size.y / 2.0f, 0.0f);

        GL.Vertex3(bc.offset.x - bc.size.x / 2.0f, bc.offset.y + bc.size.y / 2.0f, 0.0f);
        GL.Vertex3(bc.offset.x - bc.size.x / 2.0f, bc.offset.y - bc.size.y / 2.0f, 0.0f);

        GL.End();
    }
}
