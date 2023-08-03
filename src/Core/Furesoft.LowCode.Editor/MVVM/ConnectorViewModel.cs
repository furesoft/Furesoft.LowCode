using System.Reactive.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using ReactiveMarbles.PropertyChanged;

namespace Furesoft.LowCode.Editor.MVVM;

[ObservableObject]
public partial class ConnectorViewModel : IConnector
{
    [ObservableProperty] private IPin _end;
    [ObservableProperty] private string _name;
    [ObservableProperty] private double _offset = 30;
    [ObservableProperty] private ConnectorOrientation _orientation;
    [ObservableProperty] private IDrawingNode _parent;
    [ObservableProperty] private IPin _start;

    public ConnectorViewModel()
    {
        ObservePins();
    }

    public event EventHandler<ConnectorCreatedEventArgs> Created;

    public event EventHandler<ConnectorRemovedEventArgs> Removed;

    public event EventHandler<ConnectorSelectedEventArgs> Selected;

    public event EventHandler<ConnectorDeselectedEventArgs> Deselected;

    public event EventHandler<ConnectorStartChangedEventArgs> StartChanged;

    public event EventHandler<ConnectorEndChangedEventArgs> EndChanged;

    public virtual bool CanSelect()
    {
        return true;
    }

    public virtual bool CanRemove()
    {
        return true;
    }

    public void OnCreated()
    {
        Created?.Invoke(this, new(this));
    }

    public void OnRemoved()
    {
        Removed?.Invoke(this, new(this));
    }

    public void OnSelected()
    {
        Selected?.Invoke(this, new(this));
    }

    public void OnDeselected()
    {
        Deselected?.Invoke(this, new(this));
    }

    public void OnStartChanged()
    {
        StartChanged?.Invoke(this, new(this));
    }

    public void OnEndChanged()
    {
        EndChanged?.Invoke(this, new(this));
    }

    private void ObservePins()
    {
        this.WhenChanged(x => x.Start)
            .DistinctUntilChanged()
            .Subscribe(start =>
            {
                if (start?.Parent is not null)
                {
                    (start.Parent as NodeViewModel)?.WhenChanged(x => x.X).DistinctUntilChanged()
                        .Subscribe(_ => OnPropertyChanged(nameof(Start)));
                    (start.Parent as NodeViewModel)?.WhenChanged(x => x.Y).DistinctUntilChanged()
                        .Subscribe(_ => OnPropertyChanged(nameof(Start)));
                }
                else
                {
                    if (start is not null)
                    {
                        (start as PinViewModel)?.WhenChanged(x => x.X).DistinctUntilChanged()
                            .Subscribe(_ => OnPropertyChanged(nameof(Start)));
                        (start as PinViewModel)?.WhenChanged(x => x.Y).DistinctUntilChanged()
                            .Subscribe(_ => OnPropertyChanged(nameof(Start)));
                    }
                }

                (start as PinViewModel)?.WhenChanged(x => x.Alignment).DistinctUntilChanged()
                    .Subscribe(_ => OnPropertyChanged(nameof(Start)));
            });

        this.WhenChanged(x => x.End)
            .DistinctUntilChanged()
            .Subscribe(end =>
            {
                if (end?.Parent is not null)
                {
                    (end.Parent as NodeViewModel)?.WhenChanged(x => x.X).DistinctUntilChanged()
                        .Subscribe(_ => OnPropertyChanged(nameof(End)));
                    (end.Parent as NodeViewModel)?.WhenChanged(x => x.Y).DistinctUntilChanged()
                        .Subscribe(_ => OnPropertyChanged(nameof(End)));
                }
                else
                {
                    if (end is not null)
                    {
                        (end as PinViewModel)?.WhenChanged(x => x.X).DistinctUntilChanged()
                            .Subscribe(_ => OnPropertyChanged(nameof(End)));
                        (end as PinViewModel)?.WhenChanged(x => x.Y).DistinctUntilChanged()
                            .Subscribe(_ => OnPropertyChanged(nameof(End)));
                    }
                }

                if (end is not null)
                {
                    (end as PinViewModel)?.WhenChanged(x => x.Alignment).Subscribe(_ => OnPropertyChanged(nameof(End)));
                }
            });
    }
}
