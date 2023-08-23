using Furesoft.LowCode.Designer.ViewModels;

namespace Furesoft.LowCode.Designer.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif

        Closing += (_, _) =>
        {
            if (DataContext is MainViewViewModel vm)
            {
                vm.CloseLayout();
            }
        };
    }
}
