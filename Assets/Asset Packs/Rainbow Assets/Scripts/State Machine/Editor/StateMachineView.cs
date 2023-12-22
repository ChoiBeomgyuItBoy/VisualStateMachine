using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

namespace RainbowAssets.StateMachine.Editor
{
    public class StateMachineView : GraphView
    {
        new class UxmlFactory : UxmlFactory<StateMachineView, UxmlTraits> { }

        public StateMachineView()
        {
            Insert(0, new GridBackground());

            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());

            StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(StateMachineEditor.path + "StateMachineEditor.uss");
            styleSheets.Add(styleSheet);
        }
    }
}