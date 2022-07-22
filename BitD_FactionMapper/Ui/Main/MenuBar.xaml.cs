using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BitD_FactionMapper.Model;

namespace BitD_FactionMapper.Ui.Main
{
    public partial class MenuBar : Menu
    {
        private readonly NodeDataManager _nodeDataManager = NodeDataManager.Instance;
        private readonly NodeFilterManager _nodeFilterManager = NodeFilterManager.Instance;
        
        public GraphControl.RedrawGraphDelegate RedrawGraph;
        public GraphControl.UpdateGraphDelegate UpdateGraph;
        
        public delegate void ShowProgressOverlayDelegate();
        public ShowProgressOverlayDelegate ShowProgressOverlay;
        
        public delegate void HideProgressOverlayDelegate();
        public HideProgressOverlayDelegate HideProgressOverlay;

        public MenuBar()
        {
            InitializeComponent();
        }

        private void OpenMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            ShowProgressOverlay();
            _nodeDataManager.LoadData();
            RedrawGraph?.Invoke();
            HideProgressOverlay();
        }

        private void SaveMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            ShowProgressOverlay();
            
            _nodeDataManager.SaveGraph();

            HideProgressOverlay();
        }

        private void RebuildMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Are you sure you wish to rebuild the graph? This will discard all changes that has been made.",
                "Rebuild Graph",
                MessageBoxButton.OKCancel
                );
            
            if(result is MessageBoxResult.OK)
            {
                ShowProgressOverlay();
                _nodeDataManager.RebuildNodeGraph();
                RedrawGraph?.Invoke();
                HideProgressOverlay();
            }
        }

        private void ExitMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void TxtDegrees_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var degrees = txtDegrees.Text;
            if (degrees.Trim() == "") degrees = "-1";
            
            var success = _nodeFilterManager.FilterDegreesOfSeparation(int.Parse(degrees));
            if (success)
            {
                RedrawGraph?.Invoke();
            }
        }

        private void TxtDegrees_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void ChkTwoWay_OnChecked(object sender, RoutedEventArgs e)
        {
            if (chkIsSource.IsChecked == false && chkIsTarget.IsChecked == false)
            {
                if (e.Source == chkIsSource)
                {
                    chkIsTarget.IsChecked = true;
                    return; // We return because setting IsChecked will call this event again.
                }
                else
                {
                    chkIsSource.IsChecked = true;
                    return; // We return because setting IsChecked will call this event again.
                }
            }
            
            var success = _nodeFilterManager.FilterTwoWay(chkIsSource?.IsChecked ?? false, chkIsTarget?.IsChecked ?? false);
            if (success)
            {
                RedrawGraph?.Invoke();
            }
        }
    }
}