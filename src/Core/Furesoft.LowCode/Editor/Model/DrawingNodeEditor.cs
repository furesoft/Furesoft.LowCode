namespace Furesoft.LowCode.Editor.Model;

public sealed class DrawingNodeEditor(IDrawingNode node, IDrawingNodeFactory factory)
{
    private string _clipboard;
    private IConnector _connector;
    private double _pressedX = double.NaN;
    private double _pressedY = double.NaN;

    public T Clone<T>(T source)
    {
        var serialize = node.GetSerializer();
        if (serialize is null)
        {
            return default;
        }

        var text = serialize.Serialize(source);

        return serialize.Deserialize<T>(text);
    }

    public bool IsPinConnected(IPin pin)
    {
        if (node.Connectors is not null)
        {
            foreach (var connector in node.Connectors)
            {
                if (connector.Start == pin || connector.End == pin)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool IsConnectorMoving()
    {
        if (_connector is not null)
        {
            return true;
        }

        return false;
    }

    public void CancelConnector()
    {
        if (_connector is not null)
        {
            if (node.Connectors is not null)
            {
                node.Connectors.Remove(_connector);
            }

            _connector = null;
        }
    }

    public bool CanSelectNodes()
    {
        if (_connector is not null)
        {
            return false;
        }

        return true;
    }

    public bool CanSelectConnectors()
    {
        if (_connector is not null)
        {
            return false;
        }

        return true;
    }

    public bool CanConnectPin(IPin pin)
    {
        return !IsPinConnected(pin) || pin.CanConnectToMultiplePins;
    }

    private void NotifyPinsRemoved(INode node)
    {
        if (node.Pins is not null)
        {
            foreach (var pin in node.Pins)
            {
                pin.OnRemoved();
            }
        }
    }

    public void DrawingLeftPressed(double x, double y)
    {
        if (IsConnectorMoving())
        {
            CancelConnector();
        }
    }

    public void DrawingRightPressed(double x, double y)
    {
        _pressedX = x;
        _pressedY = y;

        if (IsConnectorMoving())
        {
            CancelConnector();
        }
    }

    public void ConnectorLeftPressed(IPin pin, bool showWhenMoving)
    {
        if (node.Connectors is null)
        {
            return;
        }

        if (!CanConnectPin(pin) || !pin.CanConnect())
        {
            return;
        }

        if (_connector is null)
        {
            var x = pin.X;
            var y = pin.Y;

            if (pin.Parent is not null)
            {
                x += pin.Parent.X;
                y += pin.Parent.Y;
            }

            var end = factory.CreatePin();
            end.Parent = null;
            end.X = x;
            end.Y = y;
            end.Width = pin.Width;
            end.Height = pin.Height;
            end.OnCreated();

            var connector = factory.CreateConnector();
            connector.Parent = node;
            connector.Start = pin;
            connector.End = end;
            pin.OnConnected();
            end.OnConnected();
            connector.OnCreated();

            if (showWhenMoving)
            {
                node.Connectors.Add(connector);
            }

            _connector = connector;
        }
        else
        {
            if (_connector.Start != pin && _connector.Start.Mode != pin.Mode
                                        && !node.Connectors.Any(_ => _.Start == _connector.Start && _.End == pin)
                                        && !node.Connectors.Any(_ => _.Start == pin && _.End == _connector.Start))
            {
                var end = _connector.End;
                _connector.End = pin;
                end?.OnDisconnected();
                pin.OnConnected();

                if (!showWhenMoving)
                {
                    node.Connectors ??= factory.CreateList<IConnector>();
                    node.Connectors.Add(_connector);
                }

                _connector = null;
            }
        }
    }

    public void ConnectorMove(double x, double y)
    {
        if (_connector is {End: not null})
        {
            _connector.End.X = x;
            _connector.End.Y = y;
            _connector.End.OnMoved();
        }
    }

    public void CutNodes()
    {
        var serializer = node.GetSerializer();
        if (serializer is null)
        {
            return;
        }

        var selectedNodes = node.GetSelectedNodes();
        var selectedConnectors = GetConnectionsForNodes(node.GetSelectedNodes());

        if (selectedNodes is not {Count: > 0} && selectedConnectors is not {Count: > 0})
        {
            return;
        }

        var clipboard = new Clipboard {SelectedNodes = selectedNodes, SelectedConnectors = selectedConnectors};

        _clipboard = serializer.Serialize(clipboard);

        if (clipboard.SelectedNodes is not null)
        {
            foreach (var node1 in clipboard.SelectedNodes)
            {
                if (node1.CanRemove())
                {
                    node.Nodes?.Remove(node1);
                    node1.OnRemoved();
                    NotifyPinsRemoved(node1);
                }
            }
        }

        if (clipboard.SelectedConnectors is not null)
        {
            foreach (var connector in clipboard.SelectedConnectors)
            {
                if (connector.CanRemove())
                {
                    node.Connectors?.Remove(connector);
                    connector.OnRemoved();
                }
            }
        }

        node.NotifyDeselectedNodes();
        node.NotifyDeselectedConnectors();

        node.SetSelectedNodes(null);
        node.SetSelectedConnectors(null);
        node.NotifySelectionChanged();
    }

    public void CopyNodes()
    {
        var serializer = node.GetSerializer();
        if (serializer is null)
        {
            return;
        }

        var selectedNodes = node.GetSelectedNodes();
        var selectedConnectors = node.GetSelectedConnectors();

        if (selectedNodes is not {Count: > 0} && selectedConnectors is not {Count: > 0})
        {
            return;
        }

        var clipboard = new Clipboard {SelectedNodes = selectedNodes, SelectedConnectors = selectedConnectors};

        _clipboard = serializer.Serialize(clipboard);
    }

    public void PasteNodes()
    {
        var serializer = node.GetSerializer();
        if (serializer is null)
        {
            return;
        }

        if (_clipboard is null)
        {
            return;
        }

        var pressedX = _pressedX;
        var pressedY = _pressedY;

        var clipboard = serializer.Deserialize<Clipboard>(_clipboard);
        if (clipboard is null)
        {
            return;
        }

        node.NotifyDeselectedNodes();
        node.NotifyDeselectedConnectors();

        node.SetSelectedNodes(null);
        node.SetSelectedConnectors(null);

        var selectedNodes = new HashSet<INode>();
        var selectedConnectors = new HashSet<IConnector>();

        if (clipboard.SelectedNodes is {Count: > 0})
        {
            var minX = 0.0;
            var minY = 0.0;
            var i = 0;

            foreach (var node in clipboard.SelectedNodes)
            {
                minX = i == 0 ? node.X : Math.Min(minX, node.X);
                minY = i == 0 ? node.Y : Math.Min(minY, node.Y);
                i++;
            }

            var deltaX = double.IsNaN(pressedX) ? 0.0 : pressedX - minX;
            var deltaY = double.IsNaN(pressedY) ? 0.0 : pressedY - minY;

            foreach (var node1 in clipboard.SelectedNodes)
            {
                if (node1.CanMove())
                {
                    node1.Move(deltaX, deltaY);
                }

                node1.Parent = node;

                node.Nodes?.Add(node1);
                node1.OnCreated();

                if (node1.CanSelect())
                {
                    selectedNodes.Add(node1);
                    node1.OnSelected();
                }
            }
        }

        if (clipboard.SelectedConnectors is {Count: > 0})
        {
            foreach (var connector in clipboard.SelectedConnectors)
            {
                connector.Parent = node;

                node.Connectors?.Add(connector);
                connector.OnCreated();

                if (connector.CanSelect())
                {
                    selectedConnectors.Add(connector);
                    connector.OnSelected();
                }
            }
        }

        node.NotifyDeselectedNodes();

        if (selectedNodes.Count > 0)
        {
            node.SetSelectedNodes(selectedNodes);
        }
        else
        {
            node.SetSelectedNodes(null);
        }

        node.NotifyDeselectedConnectors();

        if (selectedConnectors.Count > 0)
        {
            node.SetSelectedConnectors(selectedConnectors);
        }
        else
        {
            node.SetSelectedConnectors(null);
        }

        node.NotifySelectionChanged();

        _pressedX = double.NaN;
        _pressedY = double.NaN;
    }

    public void DuplicateNodes()
    {
        _pressedX = double.NaN;
        _pressedY = double.NaN;

        CopyNodes();
        PasteNodes();
    }

    private IEnumerable<IConnector> GetConnectionsForNode(INode node1)
    {
        foreach (var connector in node.Connectors)
        {
            if (connector.Start.Parent == node1 || connector.End.Parent == node1)
            {
                yield return connector;
            }
        }
    }

    private List<IConnector> GetConnectionsForNodes(IEnumerable<INode> nodes)
    {
        if (nodes == null)
        {
            return new();
        }

        var result = new List<IConnector>();

        foreach (var node in nodes)
        {
            var connections = GetConnectionsForNode(node);
            result.AddRange(connections);
        }

        return result;
    }

    public void DeleteNodes()
    {
        var selectedNodes = node.GetSelectedNodes();
        var selectedConnectors = GetConnectionsForNodes(node.GetSelectedNodes());
        var notify = false;

        if (selectedNodes is {Count: > 0})
        {
            foreach (var node1 in selectedNodes)
            {
                if (node1.CanRemove())
                {
                    node.Nodes?.Remove(node1);
                    node1.OnRemoved();
                    NotifyPinsRemoved(node1);
                }
            }

            node.NotifyDeselectedNodes();

            node.SetSelectedNodes(null);
            notify = true;
        }

        if (selectedConnectors is {Count: > 0})
        {
            foreach (var connector in selectedConnectors)
            {
                if (connector.CanRemove())
                {
                    node.Connectors?.Remove(connector);
                    connector.OnRemoved();
                }
            }

            node.NotifyDeselectedConnectors();

            node.SetSelectedConnectors(null);
            notify = true;
        }

        if (notify)
        {
            node.NotifySelectionChanged();
        }
    }

    public void SelectAllNodes()
    {
        var notify = false;

        if (node.Nodes is not null)
        {
            node.NotifyDeselectedNodes();

            node.SetSelectedNodes(null);

            var selectedNodes = new HashSet<INode>();
            var nodes = node.Nodes;

            foreach (var node in nodes)
            {
                if (node.CanSelect())
                {
                    selectedNodes.Add(node);
                    node.OnSelected();
                }
            }

            if (selectedNodes.Count > 0)
            {
                node.SetSelectedNodes(selectedNodes);
                notify = true;
            }
        }

        if (node.Connectors is not null)
        {
            node.NotifyDeselectedConnectors();

            node.SetSelectedConnectors(null);

            var selectedConnectors = new HashSet<IConnector>();
            var connectors = node.Connectors;

            foreach (var connector in connectors)
            {
                if (connector.CanSelect())
                {
                    selectedConnectors.Add(connector);
                    connector.OnSelected();
                }
            }

            if (selectedConnectors.Count > 0)
            {
                node.SetSelectedConnectors(selectedConnectors);
                notify = true;
            }
        }

        if (notify)
        {
            node.NotifySelectionChanged();
        }
    }

    public void DeselectAllNodes()
    {
        node.NotifyDeselectedNodes();
        node.NotifyDeselectedConnectors();

        node.SetSelectedNodes(null);
        node.SetSelectedConnectors(null);
        node.NotifySelectionChanged();

        if (IsConnectorMoving())
        {
            CancelConnector();
        }
    }

    public class Clipboard
    {
        public ISet<INode> SelectedNodes { get; set; }
        public ICollection<IConnector> SelectedConnectors { get; set; }
    }
}
