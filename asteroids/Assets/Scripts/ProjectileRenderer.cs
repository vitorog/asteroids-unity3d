using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProjectileRenderer : MonoBehaviour {

    private List<Projectile> projectiles_;
    private Material line_material_;
    private Color color_;

    public void AddProjectile(Projectile p)
    {
        projectiles_.Add(p);
    }

	void Awake () {
        projectiles_ = new List<Projectile>();
        CreateLineMaterial();
        color_ = new Color(1.0f, 1.0f, 1.0f, 1.0f);
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
        for (int i = 0; i < projectiles_.Count; i++)
        {
            if (projectiles_[i] != null)
            {
                RenderProjectile(projectiles_[i]);
            }
        }
        projectiles_.RemoveAll(projectile => projectile == null);
    }

    void RenderProjectile(Projectile p)
    {
        line_material_.SetPass(0);
        GL.PushMatrix();
        Matrix4x4 trs_matrix = Matrix4x4.TRS(p.gameObject.transform.position, p.gameObject.transform.rotation, p.gameObject.transform.localScale);
        GL.MultMatrix(trs_matrix);
        //DrawBoundingBox();
        DrawSquare(1.0f);
        GL.PopMatrix();
    }


    void DrawSquare(float size)
    {
        float half_size = size / 2.0f;
        GL.Color(color_);
        GL.Begin(GL.QUADS);
        GL.Vertex3(-half_size, -half_size, 0.0f);
        GL.Vertex3(half_size, -half_size, 0.0f);
        GL.Vertex3(half_size, half_size, 0.0f);
        GL.Vertex3(-half_size, half_size, 0.0f);
        GL.End();
    }  

    void DrawBoundingBox()
    {
        BoxCollider2D bc = (BoxCollider2D)gameObject.GetComponent<BoxCollider2D>();

        GL.Color(new Color(1.0f, 1.0f, 1.0f, 1.0f));
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
