using System.ComponentModel;
using System.Text;
using CommunityToolkit.Mvvm.Input;
using Dock.Model.Mvvm.Controls;
using Furesoft.LowCode.Attributes;

namespace Furesoft.LowCode.Designer.Layout.ViewModels.Tools;

public partial class ToolboxToolViewModel : Tool, INodeTemplatesHost
{
    private readonly Dictionary<string, List<INodeTemplate>> _categorizedNodeTemplates = new();
    [ObservableProperty] private string _searchTerm = string.Empty;

    public ToolboxToolViewModel(NodeFactory factory)
    {
        PropertyChanged += OnPropertyChanged;

        var templates = factory.CreateTemplates();
        CategorizeTemplates(templates);
        TransformToTree();

        Id = "Toolbox";
        Title = Id;
    }

    public ObservableCollection<object> Templates { get; set; } = new();

    private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(SearchTerm))
        {
            Search(SearchTerm);
        }
    }

    private void CategorizeTemplates(IList<INodeTemplate> templates)
    {
        foreach (var nodeTemplate in templates.Select(_ => (_, (CustomNodeViewModel)_.Template)))
        {
            var category = nodeTemplate.Item2.DefiningNode.GetAttribute<NodeCategoryAttribute>()?.Category ?? "General";

            if (!_categorizedNodeTemplates.ContainsKey(category))
            {
                _categorizedNodeTemplates.Add(category, new());
            }

            _categorizedNodeTemplates[category].Add(nodeTemplate._);
        }
    }

    [RelayCommand]
    public void Search(string term)
    {
        if (string.IsNullOrEmpty(term))
        {
            CollapseAll(Templates);
            return;
        }

        Search(Templates);
    }

    private void CollapseAll(IEnumerable<object> templates)
    {
        foreach (var template in templates)
        {
            switch (template)
            {
                case TreeViewItem tvi:
                    tvi.IsExpanded = false;
                    tvi.IsVisible = true;

                    CollapseAll(tvi.Items);
                    break;

                case NodeTemplateViewModel vm:
                    vm.IsVisible = true;
                    break;
            }
        }
    }

    private void Search(IEnumerable<object> children)
    {
        foreach (var item in children)
        {
            switch (item)
            {
                case NodeTemplateViewModel vm:
                    vm.IsVisible = string.IsNullOrEmpty(SearchTerm) ||
                                   vm.Title.IndexOf(SearchTerm, StringComparison.OrdinalIgnoreCase) >= 0;
                    break;
                case TreeViewItem tvi:
                    Search(tvi.Items);

                    tvi.IsVisible = tvi.Items.OfType<NodeTemplateViewModel>().Any(child => child.IsVisible);
                    tvi.IsExpanded = tvi.IsVisible;
                    break;
            }
        }
    }

    private void TransformToTree()
    {
        var treeCache = new Dictionary<string, TreeViewItem>();
        var currentPathBuilder = new StringBuilder();

        foreach (var nodeTemplate in _categorizedNodeTemplates)
        {
            var spl = nodeTemplate.Key.Split("/");
            TreeViewItem parentItem = null;
            currentPathBuilder.Clear();

            parentItem = CreateTreeViewItem(spl, currentPathBuilder, treeCache, parentItem);

            parentItem.Header = spl[^1];

            foreach (var node in nodeTemplate.Value)
            {
                parentItem.Items.Add(node);
            }
        }
    }

    private TreeViewItem CreateTreeViewItem(string[] spl, StringBuilder currentPathBuilder,
        Dictionary<string, TreeViewItem> treeCache,
        TreeViewItem parentItem)
    {
        for (var index = 0; index < spl.Length; index++)
        {
            var s = spl[index];
            currentPathBuilder.Append(index == 0 ? "" : "/").Append(s);
            var currentPath = currentPathBuilder.ToString();

            if (!treeCache.TryGetValue(currentPath, out var treeViewItem))
            {
                treeViewItem = new() {Header = s};
                if (index == 0)
                {
                    Templates.Insert(0, treeViewItem);
                }
                else
                {
                    treeCache[
                            currentPathBuilder.Remove(currentPath.LastIndexOf('/'),
                                currentPath.Length - currentPath.LastIndexOf('/')).ToString()].Items
                        .Insert(0, treeViewItem);
                }

                treeCache.Add(currentPath, treeViewItem);
            }

            parentItem = treeViewItem;
        }

        return parentItem;
    }
}
