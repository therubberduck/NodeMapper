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
        
        public MenuBar()
        {
            InitializeComponent();
        }

        private void OpenMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void SaveMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void ExitMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
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