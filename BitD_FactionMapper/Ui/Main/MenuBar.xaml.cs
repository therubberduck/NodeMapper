using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BitD_FactionMapper.Model;
using Microsoft.Win32;

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

        private void LoadMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog()
            {
                InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory,
                Filter = "Save File (*.sav)|*.sav",
                AddExtension = true,
                DefaultExt = "sav"
            };
            if (dialog.ShowDialog() == true)
            {
                ShowProgressOverlay();
                _nodeDataManager.Load(dialog.FileName);
                RedrawGraph?.Invoke();
                HideProgressOverlay();
            }
        }

        private void SaveAsMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory,
                OverwritePrompt = true,
                Filter = "Save File (*.sav)|*.sav",
                AddExtension = true,
                DefaultExt = "sav"
            };
            if (dialog.ShowDialog() == true)
            {
                _nodeDataManager.SaveGraphAs(dialog.FileName);
            }
        }

        private void SaveMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            ShowProgressOverlay();
            
            _nodeDataManager.SaveGraph();

            HideProgressOverlay();
        }

        private void ReloadMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            
            var result = MessageBox.Show(
                "Are you sure you wish to reload the graph? This will discard all changes that has been made since your last save.",
                "Reload Graph",
                MessageBoxButton.OKCancel
            );

            if (result is MessageBoxResult.OK)
            {
                ShowProgressOverlay();
                _nodeDataManager.LoadData();
                RedrawGraph?.Invoke();
                HideProgressOverlay();
            }
        }

        private void RebuildMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Are you sure you wish to rebuild the graph? This will discard all changes that has been made EVER.",
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