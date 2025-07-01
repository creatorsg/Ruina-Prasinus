using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UIElements;

public class DynamicComponentGraphWindow : EditorWindow, ISerializationCallbackReceiver
{
    // 노드 정보를 담는 직렬화 가능한 내부 클래스
    [Serializable]
    private class NodeData
    {
        public string label;
        public string assetPath;
        public Vector2 position;
    }

    [SerializeField]
    private List<NodeData> savedNodes = new List<NodeData>();

    private ComponentGraphView graphView;
    private bool isRestoring = false;

    [MenuItem("Window/Dynamic Component Graph")]
    public static void OpenWindow()
    {
        var window = GetWindow<DynamicComponentGraphWindow>("Component SO Graph");
        window.minSize = new Vector2(400, 300);
    }

    private void OnEnable()
    {
        isRestoring = true;
        ConstructGraphView();
        GenerateToolbar();

        // 직렬화된 데이터 복사본으로 순회
        var nodesToRestore = new List<NodeData>(savedNodes);
        foreach (var data in nodesToRestore)
        {
            var node = graphView.AddNode(data.label);
            node.objectField.value = AssetDatabase.LoadAssetAtPath<ScriptableObject>(data.assetPath);
            node.SetPosition(new Rect(data.position, node.layout.size));
        }

        isRestoring = false;
    }

    private void OnDisable()
    {
        rootVisualElement.Remove(graphView);
    }

    private void ConstructGraphView()
    {
        graphView = new ComponentGraphView(OnNodeChanged, OnNodeRemoved);
        graphView.StretchToParentSize();
        rootVisualElement.Add(graphView);
    }

    private void GenerateToolbar()
    {
        var toolbar = new Toolbar();
        var addButton = new Button(() =>
        {
            var node = graphView.AddNode("Slot");
            if (!isRestoring) RecordNode(node);
        })
        { text = "Add Node" };
        toolbar.Add(addButton);
        rootVisualElement.Add(toolbar);
    }

    private void OnNodeChanged(ComponentNode node)
    {
        if (!isRestoring)
            RecordNode(node);
    }

    private void OnNodeRemoved(ComponentNode node)
    {
        if (!isRestoring)
            RemoveSavedNodeData(node);
    }

    private void RecordNode(ComponentNode node)
    {
        var path = AssetDatabase.GetAssetPath(node.objectField.value);
        var data = new NodeData
        {
            label = node.labelField.text,
            assetPath = path,
            position = node.GetPosition().position
        };

        var existing = savedNodes.FirstOrDefault(n => n.assetPath == data.assetPath && n.label == data.label);
        if (existing != null)
            savedNodes[savedNodes.IndexOf(existing)] = data;
        else
            savedNodes.Add(data);
    }

    private void RemoveSavedNodeData(ComponentNode node)
    {
        var path = AssetDatabase.GetAssetPath(node.objectField.value);
        var label = node.labelField.text;
        var existing = savedNodes.FirstOrDefault(n => n.assetPath == path && n.label == label);
        if (existing != null)
            savedNodes.Remove(existing);
    }

    public void OnBeforeSerialize() { }
    public void OnAfterDeserialize() { }
}

public class ComponentGraphView : GraphView
{
    private Action<ComponentNode> onNodeChanged;
    private Action<ComponentNode> onNodeRemoved;

    public ComponentGraphView(Action<ComponentNode> onNodeChanged, Action<ComponentNode> onNodeRemoved)
    {
        this.onNodeChanged = onNodeChanged;
        this.onNodeRemoved = onNodeRemoved;

        var grid = new GridBackground();
        Insert(0, grid);
        grid.StretchToParentSize();
        style.flexGrow = 1;

        AddManipulators();
    }

    private void AddManipulators()
    {
        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());
    }

    public ComponentNode AddNode(string title)
    {
        var node = new ComponentNode(title);
        node.Initialize(onNodeChanged, onNodeRemoved);
        AddElement(node);
        return node;
    }
}

public class ComponentNode : Node
{
    public TextField labelField;
    public ObjectField objectField;
    private ScriptableObject assignedSO;
    private UnityEditor.Editor inspectorEditor;
    private IMGUIContainer imguiContainer;

    private Action<ComponentNode> onChanged;
    private Action<ComponentNode> onRemoved;

    public ComponentNode(string title)
    {
        this.title = title;
    }

    public void Initialize(Action<ComponentNode> onChanged, Action<ComponentNode> onRemoved)
    {
        this.onChanged = onChanged;
        this.onRemoved = onRemoved;

        labelField = new TextField("Label");
        labelField.value = title;
        labelField.RegisterValueChangedCallback(evt => onChanged(this));
        mainContainer.Add(labelField);

        objectField = new ObjectField("SO Asset") { objectType = typeof(ScriptableObject), allowSceneObjects = false };
        objectField.RegisterValueChangedCallback(evt =>
        {
            assignedSO = evt.newValue as ScriptableObject;
            onChanged(this);

            if (imguiContainer != null) mainContainer.Remove(imguiContainer);
            if (inspectorEditor != null) UnityEngine.Object.DestroyImmediate(inspectorEditor);

            if (assignedSO != null)
            {
                inspectorEditor = UnityEditor.Editor.CreateEditor(assignedSO);
                imguiContainer = new IMGUIContainer(() => inspectorEditor.OnInspectorGUI());
                imguiContainer.style.marginTop = 4;
                mainContainer.Add(imguiContainer);
            }
        });
        mainContainer.Add(objectField);

        this.RegisterCallback<GeometryChangedEvent>(_ => onChanged(this));

        var removeBtn = new Button(() => RemoveNode()) { text = "Remove" };
        titleButtonContainer.Add(removeBtn);

        RefreshExpandedState();
        RefreshPorts();
    }

    private void RemoveNode()
    {
        onRemoved(this);
        var graph = this.GetFirstAncestorOfType<ComponentGraphView>();
        graph?.RemoveElement(this);

        if (inspectorEditor != null) UnityEngine.Object.DestroyImmediate(inspectorEditor);
    }
}
