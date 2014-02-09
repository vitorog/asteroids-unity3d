using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    public float max_time_alive_;
    private Material line_material_;
    private Color color_;
    private float time_alive_;

    // Use this for initialization
    void Start()
    {
        CreateLineMaterial();
        color_ = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        time_alive_ += Time.deltaTime;
        if (time_alive_ > max_time_alive_)
        {
            Destroy(gameObject);
        }
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
        Render();
    }

    void Render()
    {
        line_material_.SetPass(0);
        GL.PushMatrix();
        Matrix4x4 trs_matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
        GL.MultMatrix(trs_matrix);
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
}
