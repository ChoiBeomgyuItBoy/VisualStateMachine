using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

namespace RainbowAssets.StateMachine.Editor
{
    public class TransitionEdge : Edge
    {
        const float edgeOffset = 4;
        const float arrowWidth = 12;
        const float selfArrowOffset = 35;

        public TransitionEdge()
        {
            edgeControl.RegisterCallback<GeometryChangedEvent>(OnEdgeGeometryChanged);
            generateVisualContent += DrawArrow;
        }

        void OnEdgeGeometryChanged(GeometryChangedEvent evt)
        {
            PointsAndTangents[1] = PointsAndTangents[0];
            PointsAndTangents[2] = PointsAndTangents[3];

            if(input != null && output != null)
            {   
                AddHorizontalOffset();
                AddVerticalOffset();
            }

            MarkDirtyRepaint();
        }

        void AddHorizontalOffset()
        {
            if(input.node.GetPosition().x > output.node.GetPosition().x)
            {
                PointsAndTangents[1].y -= edgeOffset;
                PointsAndTangents[2].y -= edgeOffset;
            }
            else if(input.node.GetPosition().x < output.node.GetPosition().x)
            {
                PointsAndTangents[1].y += edgeOffset;
                PointsAndTangents[2].y += edgeOffset;
            }
        }

        void AddVerticalOffset()
        {
            if(input.node.GetPosition().y > output.node.GetPosition().y)
            {
                PointsAndTangents[1].x += edgeOffset;
                PointsAndTangents[2].x += edgeOffset;
            }
            else if(input.node.GetPosition().y < output.node.GetPosition().y)
            {
                PointsAndTangents[1].x -= edgeOffset;
                PointsAndTangents[2].x -= edgeOffset;
            }
        }

        void DrawArrow(MeshGenerationContext context)
        {
            Vector2 start = PointsAndTangents[PointsAndTangents.Length / 2 - 1];
            Vector2 end = PointsAndTangents[PointsAndTangents.Length / 2];
            Vector2 mid = (start + end) / 2;
            Vector2 direction = end - start;

            if(IsSelfTransition())
            {
                mid = PointsAndTangents[0] + Vector2.up * selfArrowOffset;
                direction = Vector2.down;
            }

            float distanceFromMid = arrowWidth * Mathf.Sqrt(3) / 4;
            float angle = Vector2.SignedAngle(Vector2.right, direction);
            float perpendicularLength = GetPerpendicularLength(angle);

            Vector2 perpendicular = new Vector2(-direction.y, direction.x).normalized * perpendicularLength;

            if(IsSelfTransition())
            {
                perpendicular = Vector2.right * perpendicularLength;
            }

            MeshWriteData mesh = context.Allocate(3, 3);
            Vertex[] vertices = new Vertex[3];

            vertices[0].position = mid + (direction.normalized * distanceFromMid);
            vertices[1].position = mid + (-direction.normalized * distanceFromMid) + (perpendicular.normalized * arrowWidth / 2);
            vertices[2].position = mid + (-direction.normalized * distanceFromMid) + (-perpendicular.normalized * arrowWidth / 2);

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].position += Vector3.forward * Vertex.nearZ;
                vertices[i].tint = GetColor();
            }

            mesh.SetAllVertices(vertices);
            mesh.SetAllIndices(new ushort[] { 0, 1, 2 });
        }

        bool IsSelfTransition()
        {   
            if(input != null && output != null)
            {
                return input.node == output.node;
            }

            return false;
        }

        float GetPerpendicularLength(float angle)
        {
            if(angle < 60 && angle > 0)
            {
                return arrowWidth / (Mathf.Sin(Mathf.Deg2Rad * (angle + 120)) * 2);
            }
            else if(angle > -120 && angle < -60)
            {
                return arrowWidth / (Mathf.Sin(Mathf.Deg2Rad * (angle - 120)) * 2);
            }
            else if(angle > -60 && angle < 0)
            {
                return arrowWidth / (Mathf.Sin(Mathf.Deg2Rad * (angle + 60)) * 2);
            }

            return arrowWidth / (Mathf.Sin(Mathf.Deg2Rad * (angle - 60)) * 2);
        }

        Color GetColor()
        {
            Color color = defaultColor;

            if(selected)
            {
                color = selectedColor;
            }
            else if(isGhostEdge)
            {
                color = ghostColor;
            }

            return color;
        }
    
    }
}
