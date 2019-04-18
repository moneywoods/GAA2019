using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GridLineBehaviour : MonoBehaviour
{
    private Vector2 scale;
    private Vector2Int division;
    private Vector3 offset;
    [SerializeField]
    private Material material;

    private StarMaker.MapInfo currentMapInfo = null;
    public StarMaker.MapInfo CurrentMapInfo
    {
        get { return currentMapInfo; }
        set { currentMapInfo = value; }
    }
    private void Start()
    {
        
    }
    private void OnRenderObject()
    {
        if(CurrentMapInfo == null) // 万が一CurrentMapInfo参照がなかった場合戻す。
            return;
        scale = CurrentMapInfo.CellSize * CurrentMapInfo.CellCnt;// Vector2 * Vecto2 = element-wise product
        division = CurrentMapInfo.CellCnt;
        offset = new Vector3(0.0f, 0.0f, CurrentMapInfo.CellSize.y);
        Vector2 stepSize = scale / division;
        Vector2 halfScale = scale * 0.5f;

        material.SetPass(0);
        GL.PushMatrix();
        GL.MultMatrix(transform.localToWorldMatrix);
        for(int x = 0; x <= division.x; x++) // マスの辺毎に描く必要ありそうなので。
        {
            GL.Begin(GL.LINES);
            for(int y = 0; y < division.y; y++)
            {
                GL.Vertex(new Vector3(x * stepSize.x - halfScale.x + offset.x, 0f, y * stepSize.y - halfScale.y + offset.z));
                GL.Vertex(new Vector3(x * stepSize.x - halfScale.x + offset.x, 0f, (y + 1) * stepSize.y - halfScale.y + offset.z));
            }
            GL.End();
        }
        for(int y = 0; y <= division.y; y++)
        {
            GL.Begin(GL.LINES);
            for(int x = 0; x < division.x; x++)
            {
                GL.Vertex(new Vector3(x * stepSize.x - halfScale.x + offset.x, 0f, y * stepSize.y - halfScale.y + offset.z));
                GL.Vertex(new Vector3((x + 1) * stepSize.x - halfScale.x + offset.x, 0f, y * stepSize.y - halfScale.y + offset.z));
            }
            GL.End();
        }
        GL.PopMatrix();
    }
}
