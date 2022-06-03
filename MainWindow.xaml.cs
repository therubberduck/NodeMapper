using System.Windows;
using NodeMapper.Ui.Main;

namespace NodeMapper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly GraphViewModel _graphViewModel = new GraphViewModel();
        
        public MainWindow()
        {
            InitializeComponent();

            // _graphViewModel.CreateNewEdge();
            // graphControl.Graph = _graphViewModel.Graph;
        }

        private void btnCreateNode_Click(object sender, RoutedEventArgs e)
        {
            _graphViewModel.CreateNewEdge();
            graphControl.Update();
        }
    }
}