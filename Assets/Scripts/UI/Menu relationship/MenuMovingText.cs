using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMovingText : MonoBehaviour
{
    float time = 0;
    float radius = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void ModifyMesh(UnityEngine.UI.VertexHelper vh)
    {
        if (!IsActive())
            return;

        List<UIVertex> vertices = new List<UIVertex>();
        vh.GetUIVertexStream(vertices);

        TextMove(ref vertices);

        vh.Clear();
        vh.AddUIVertexTriangleStream(vertices);
    }

    void TextMove(ref List<UIVertex> vertices)
    {
        for (int c = 0; c < vertices.Count; c += 6)
        {
            float rad = Random.Range(0, 360) * Mathf.Deg2Rad;
            Vector3 dir = new Vector3(radius * Mathf.Cos(rad), radius * Mathf.Sin(rad), 0);

            for (int i = 0; i < 6; i++)
            {
                var vert = vertices[c + i];
                vert.position = vert.position + dir;
                vertices[c + i] = vert;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
