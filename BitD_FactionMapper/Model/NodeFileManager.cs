using BitD_FactionMapper.DataRepository;
using BitD_FactionMapper.Ui.Main;
using Microsoft.Win32;

namespace BitD_FactionMapper.Model
{
    public class NodeFileManager
    {
        private static NodeFileManager _instance;
        private static readonly object Padlock = new object();
        public static NodeFileManager Instance
        {
            get
            {
                lock (Padlock)
                {
                    return _instance ?? (_instance = new NodeFileManager());
                }
            }
        }
        
        public GraphControl.RedrawGraphDelegate RedrawGraph;
        public GraphControl.UpdateGraphDelegate UpdateGraph;
        
        public delegate void ShowProgressOverlayDelegate();
        public ShowProgressOverlayDelegate ShowProgressOverlay;
        
        public delegate void HideProgressOverlayDelegate();
        public HideProgressOverlayDelegate HideProgressOverlay;

        
        private readonly NodeDataManager _nodeDataManager = NodeDataManager.Instance;
        private readonly DbRepository _repo = new DbRepository();

        public void FirstLoad()
        {
            if (_repo.RepoInitialized())
            {
                Reopen();
            }
            else
            {
                Rebuild();
            }
        }

        public void Rebuild()
        {
            var initializer = new GraphInitializer();
            var result = initializer.GetDuskvol();
            _nodeDataManager.LoadData(result);
            RedrawGraph?.Invoke();
        }

        public void Reopen()
        {
            ShowProgressOverlay();
            var result = _repo.Load();
            _nodeDataManager.LoadData(result);
            RedrawGraph?.Invoke();
            HideProgressOverlay();
        }
        
        public void OpenDialog()
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
                StoreSaveFilePath(dialog.FileName);
                var result = _repo.Load(dialog.FileName);
                _nodeDataManager.LoadData(result);
                RedrawGraph?.Invoke();
                HideProgressOverlay();
            }
        }

        public void Save()
        {
            ShowProgressOverlay();
            if (!_repo.RepoInitialized())
            {
                SaveDialog();
            }
            else
            {
                _repo.SaveGraph(_nodeDataManager.Nodes, _nodeDataManager.Edges, Properties.Settings.Default.SaveFile);
            }
            HideProgressOverlay();
        }

        public void SaveDialog()
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
                ShowProgressOverlay();
                StoreSaveFilePath(dialog.FileName);
                _repo.SaveGraph(_nodeDataManager.Nodes, _nodeDataManager.Edges, dialog.FileName);
                HideProgressOverlay();
            }
        }

        private void StoreSaveFilePath(string filePath)
        {
            Properties.Settings.Default.SaveFile = filePath;
            Properties.Settings.Default.Save();
        }
    }
}