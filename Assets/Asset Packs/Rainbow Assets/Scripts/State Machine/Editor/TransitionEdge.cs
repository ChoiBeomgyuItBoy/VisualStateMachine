using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

namespace RainbowAssets.StateMachine.Editor
{
    public class TransitionEdge : Edge
    {
        const float edgeOffset = 4;

        public TransitionEdge()
        {
            edgeControl.RegisterCallback<GeometryChangedEvent>(OnEdgeControlGeometryChanged);
        }

        void OnEdgeControlGeometryChanged(GeometryChangedEvent evt)
        {
            PointsAndTangents[1] = PointsAndTangents[0];
            PointsAndTangents[2] = PointsAndTangents[3];

            if(input != null && output != null)
            {
                if(input.node.GetPosition().y > output.node.GetPosition().y)
                {
                    PointsAndTangents[1].x += edgeOffset;
                    PointsAndTangents[2].x += edgeOffset;
                }

                if(input.node.GetPosition().y < output.node.GetPosition().y)
                {
                    PointsAndTangents[1].x -= edgeOffset;
                    PointsAndTangents[2].x -= edgeOffset;
                }

                if(input.node.GetPosition().x > output.node.GetPosition().x)
                {
                    PointsAndTangents[1].y -= edgeOffset;
                    PointsAndTangents[2].y -= edgeOffset;
                }
                
                if(input.node.GetPosition().x < output.node.GetPosition().x)
                {
                    PointsAndTangents[1].y += edgeOffset;
                    PointsAndTangents[2].y += edgeOffset;
                }
            }
        }
    
    }
}
