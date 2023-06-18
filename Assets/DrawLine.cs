using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawLine : MaskableGraphic
{
    private List<List<UIVertex>> vertexQuadList = new List<List<UIVertex>>();//存储整条线的所有四边形

    private Vector3 lastPoint;//记录上一帧的线的坐标
    public float lineWidth = 4;
    private bool isNewLine;
    Vector3 lastLeftPoint;
    Vector3 lastRightPoint;
    protected override void OnPopulateMesh(VertexHelper vh)
    {
     
        vh.Clear();

        for (int i = 0; i < vertexQuadList.Count; i++)
        {
            vh.AddUIVertexQuad(vertexQuadList[i].ToArray());
        }

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            vertexQuadList.Clear();
            lastPoint = ScreenPointToLocalPointInRectangle(Input.mousePosition);
            isNewLine = true;
        }
        else
        {
            if (Input.GetMouseButton(0)) {
                Vector3 currentPoint = ScreenPointToLocalPointInRectangle(Input.mousePosition);
                Vector3 vec = currentPoint - lastPoint;

                if (vec.magnitude < 10)
                {
                    return;
                }

                Vector3 normal = Vector3.Cross(vec.normalized, Vector3.forward).normalized;


                if (isNewLine)
                {
                    isNewLine = false;
                    lastLeftPoint = lastPoint + normal * lineWidth;
                    lastRightPoint = lastPoint - normal * lineWidth;

                }


                Vector3 currentLeftPoint = currentPoint + normal * lineWidth;
                Vector3 currentRightPoint = currentPoint - normal * lineWidth;

                List<UIVertex> vertexQuad = new List<UIVertex>();

                UIVertex uIVertex = new UIVertex();

                uIVertex.position = lastLeftPoint;
                uIVertex.color = color;
                vertexQuad.Add(uIVertex);

                UIVertex uIVertex1 = new UIVertex();

                uIVertex1.position = lastRightPoint;
                uIVertex1.color = color;
                vertexQuad.Add(uIVertex1);

                UIVertex uIVertex2 = new UIVertex();
                uIVertex2.position = currentRightPoint;
                uIVertex2.color = color;
                vertexQuad.Add(uIVertex2);

                UIVertex uIVertex3 = new UIVertex();
                uIVertex3.position = currentLeftPoint;
                uIVertex3.color = color;
                vertexQuad.Add(uIVertex3);



                vertexQuadList.Add(vertexQuad);


                lastLeftPoint = currentLeftPoint;
                lastRightPoint = currentRightPoint;
                lastPoint = currentPoint;

                SetVerticesDirty();
            }
        }
    }

    private Vector3 ScreenPointToLocalPointInRectangle(Vector3 screenPoint)
    {
        bool isTrue;
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 localPoint = Vector2.zero;
        switch (canvas.renderMode)
        {
            case RenderMode.ScreenSpaceOverlay:
              isTrue=   RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, null, out localPoint);

                break;
            case RenderMode.ScreenSpaceCamera:
                 isTrue = RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, canvas.worldCamera, out localPoint);

                break;
            case RenderMode.WorldSpace:
                 isTrue = RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, canvas.worldCamera, out localPoint);

                break;
            default:
                break;
        }


        return localPoint;

    }

}
