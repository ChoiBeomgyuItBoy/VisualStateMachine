using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine.UIElements;

namespace RainbowAssets.StateMachine.Editor
{
    public class StateMachineEditor : EditorWindow
    {
        public const string path = "Assets/Asset Packs/Rainbow Assets/Scripts/State Machine/Editor/";
        StateMachineView stateMachineView;

        [MenuItem("Window/State Machine Editor")]
        public static void ShowWindow()
        {
            GetWindow(typeof(StateMachineEditor), false, "State Machine Editor");
        }

        [OnOpenAsset]
        public static bool OnStateMachineOpened(int instanceID, int line)
        {
            StateMachine stateMachine = EditorUtility.InstanceIDToObject(instanceID) as StateMachine;

            if(stateMachine != null)
            {
                ShowWindow();
                return true;
            }

            return false;
        }

        void CreateGUI()
        {
            VisualElement root = rootVisualElement;

            VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(path + "StateMachineEditor.uxml");
            visualTree.CloneTree(root);

            stateMachineView = root.Q<StateMachineView>();
        }

        void OnSelectionChange()
        {
            StateMachine stateMachine = Selection.activeObject as StateMachine;

            if(stateMachine != null)
            {
                stateMachineView.Refresh(stateMachine);
            }
        }
    }
}
