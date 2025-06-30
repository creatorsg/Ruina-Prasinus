using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEditor.UIElements;

public class DynamicComponentGraphWindow : EditorWindow
{
    private ComponentGraphView graphView;

    [MenuItem("Window/Dynamic Component Graph")]
    public static void OpenWindow()
    {
        var window = GetWindow<DynamicComponentGraphWindow>("Component SO Graph");
        window.minSize = new Vector2(400, 300);
    }

    private void OnEnable()
    {
        ConstructGraphView();
        GenerateToolbar();
    }

    private void OnDisable()
    {
        rootVisualElement.Remove(graphView);
    }

    private void ConstructGraphView()
    {
        graphView = new ComponentGraphView { name = "Component Graph" };
        graphView.StretchToParentSize();
        rootVisualElement.Add(graphView);
    }

    private void GenerateToolbar()
    {
        var toolbar = new Toolbar();
        var addButton = new Button(() => graphView.AddNode()) { text = "Add Node" };
        toolbar.Add(addButton);
        rootVisualElement.Add(toolbar);
    }
}

public class ComponentGraphView : GraphView
{
    public new List<ComponentNode> nodes = new List<ComponentNode>();

    public ComponentGraphView()
    {
        var grid = new GridBackground();
        Insert(0, grid);
        grid.StretchToParentSize();
        style.flexGrow = 1;

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());
    }

    public void AddNode()
    {
        var node = new ComponentNode();
        node.title = "Slot";
        node.Initialize();
        node.SetPosition(new Rect(100, 100 + nodes.Count * 160, 300, 400));
        AddElement(node);
        nodes.Add(node);
    }
}

public class ComponentNode : Node
{
    private TextField labelField;
    private ObjectField objectField;
    private ScriptableObject assignedSO;
    private UnityEditor.Editor inspectorEditor;
    private IMGUIContainer imguiContainer;

    public void Initialize()
    {
        // 슬롯 라벨 입력
        labelField = new TextField("Label");
        mainContainer.Add(labelField);

        // ScriptableObject 할당 필드
        objectField = new ObjectField("SO Asset")
        {
            objectType = typeof(ScriptableObject),
            allowSceneObjects = false
        };
        objectField.RegisterValueChangedCallback(evt =>
        {
            assignedSO = evt.newValue as ScriptableObject;

            // 이전 IMGUI 컨테이너 및 에디터 제거
            if (imguiContainer != null)
                mainContainer.Remove(imguiContainer);
            if (inspectorEditor != null)
                UnityEngine.Object.DestroyImmediate(inspectorEditor);

            if (assignedSO != null)
            {
                // 새 에디터 및 IMGUI 컨테이너 생성
                inspectorEditor = UnityEditor.Editor.CreateEditor(assignedSO);
                imguiContainer = new IMGUIContainer(() =>
                {
                    if (inspectorEditor != null)
                        inspectorEditor.OnInspectorGUI();
                });
                imguiContainer.style.marginTop = 4;
                mainContainer.Add(imguiContainer);
            }
        });
        mainContainer.Add(objectField);

        // 노드 삭제 버튼
        var removeBtn = new Button(() => RemoveNode()) { text = "Remove" };
        titleButtonContainer.Add(removeBtn);

        RefreshExpandedState();
        RefreshPorts();
    }

    private void RemoveNode()
    {
        var graph = this.GetFirstAncestorOfType<ComponentGraphView>();
        if (graph != null)
        {
            graph.RemoveElement(this);
            graph.nodes.Remove(this);

            // 에디터 정리
            if (inspectorEditor != null)
                UnityEngine.Object.DestroyImmediate(inspectorEditor);
        }
    }
}

