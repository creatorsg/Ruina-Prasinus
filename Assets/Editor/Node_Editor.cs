using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UIElements;

public class DialogueSystemEditorWindow : EditorWindow, ISerializationCallbackReceiver
{
    [Serializable]
    private class DialogueNodeData
    {
        public int id;
        public string text;
        public List<string> choices = new List<string>();
        public List<int> nextNodeIds = new List<int>();
        public Vector2 position;
    }

    [SerializeField]
    private List<DialogueNodeData> savedNodes = new List<DialogueNodeData>();

    private DialogueGraphView graphView;
    private int nextId = 1;
    private bool isRestoring = false;

    [MenuItem("Window/Dialogue System Editor")]
    public static void OpenWindow()
    {
        var window = GetWindow<DialogueSystemEditorWindow>("Dialogue System");
        window.minSize = new Vector2(600, 400);
    }

    private void OnEnable()
    {
        isRestoring = true;
        ConstructGraphView();
        GenerateToolbar();

        foreach (var data in savedNodes)
        {
            var node = graphView.CreateDialogueNode(data.id, data.text, data.choices);
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
        graphView = new DialogueGraphView(OnNodeChanged, OnNodeRemoved);
        graphView.StretchToParentSize();
        rootVisualElement.Add(graphView);
    }

    private void GenerateToolbar()
    {
        var toolbar = new Toolbar();
        var addButton = new Button(() =>
        {
            var node = graphView.CreateDialogueNode(nextId++, "New Dialogue", new List<string> { "Choice 1" });
            if (!isRestoring) RecordNode(node);
        })
        { text = "Add Dialogue Node" };
        toolbar.Add(addButton);
        rootVisualElement.Add(toolbar);
    }

    private void OnNodeChanged(DialogueNode node)
    {
        if (!isRestoring) RecordNode(node);
    }

    private void OnNodeRemoved(DialogueNode node)
    {
        if (!isRestoring) RemoveSavedNode(node);
    }

    private void RecordNode(DialogueNode node)
    {
        var data = new DialogueNodeData
        {
            id = node.Id,
            text = node.TextField.value,
            choices = node.ChoiceFields.Select(f => f.value).ToList(),
            nextNodeIds = node.OutputPorts.Select(p => p.portName.ToInt()).ToList(),
            position = node.GetPosition().position
        };

        var existing = savedNodes.FirstOrDefault(n => n.id == data.id);
        if (existing != null)
            savedNodes[savedNodes.IndexOf(existing)] = data;
        else
            savedNodes.Add(data);
    }

    private void RemoveSavedNode(DialogueNode node)
    {
        var existing = savedNodes.FirstOrDefault(n => n.id == node.Id);
        if (existing != null)
            savedNodes.Remove(existing);
    }

    public void OnBeforeSerialize() { }
    public void OnAfterDeserialize() { }
}

public class DialogueGraphView : GraphView
{
    private Action<DialogueNode> onNodeChanged;
    private Action<DialogueNode> onNodeRemoved;

    public DialogueGraphView(Action<DialogueNode> onNodeChanged, Action<DialogueNode> onNodeRemoved)
    {
        this.onNodeChanged = onNodeChanged;
        this.onNodeRemoved = onNodeRemoved;

        style.flexGrow = 1;
        var grid = new GridBackground();
        Insert(0, grid);
        grid.StretchToParentSize();

        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());
    }

    public DialogueNode CreateDialogueNode(int id, string text, List<string> choices)
    {
        var node = new DialogueNode(id, text, choices);
        node.Initialize(onNodeChanged, onNodeRemoved);
        AddElement(node);
        return node;
    }
}

public class DialogueNode : Node
{
    public int Id { get; private set; }
    public TextField TextField { get; private set; }
    public List<TextField> ChoiceFields { get; private set; } = new List<TextField>();
    public List<Port> OutputPorts { get; private set; } = new List<Port>();

    private Action<DialogueNode> onChanged;
    private Action<DialogueNode> onRemoved;

    public DialogueNode(int id, string text, List<string> choices)
    {
        Id = id;
        title = $"Node {id}";
        TextField = new TextField("Text") { value = text };
        mainContainer.Add(TextField);

        var choicesContainer = new VisualElement();
        choicesContainer.name = "ChoicesContainer";
        foreach (var choice in choices)
            AddChoiceField(choice, choicesContainer);
        mainContainer.Add(choicesContainer);

        var addChoiceBtn = new Button(() => AddChoiceField("New Choice", choicesContainer)) { text = "Add Choice" };
        mainContainer.Add(addChoiceBtn);

        var removeBtn = new Button(() => RemoveNode()) { text = "Remove Node" };
        titleButtonContainer.Add(removeBtn);

        RefreshExpandedState();
    }

    public void Initialize(Action<DialogueNode> onChanged, Action<DialogueNode> onRemoved)
    {
        this.onChanged = onChanged;
        this.onRemoved = onRemoved;

        TextField.RegisterValueChangedCallback(evt => onChanged(this));
        this.RegisterCallback<GeometryChangedEvent>(_ => onChanged(this));
    }

    private void AddChoiceField(string text, VisualElement container)
    {
        var choiceField = new TextField("") { value = text };
        choiceField.RegisterValueChangedCallback(evt => onChanged(this));
        container.Add(choiceField);
        ChoiceFields.Add(choiceField);

        var port = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(int));
        port.portName = Id.ToString();
        choiceField.AddManipulator(new ContextualMenuManipulator(evt => { }));
        outputContainer.Add(port);
        OutputPorts.Add(port);

        RefreshPorts();
    }

    private void RemoveNode()
    {
        onRemoved(this);
        var graph = GetFirstAncestorOfType<DialogueGraphView>();
        graph?.RemoveElement(this);
    }
}

static class StringExtensions
{
    public static int ToInt(this string s)
    {
        int.TryParse(s, out var result);
        return result;
    }
}
