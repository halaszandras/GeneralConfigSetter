using System.Collections.ObjectModel;
using System.Linq;
using GeneralConfigSetter.Enums;
using GeneralConfigSetter.Models;
using Microsoft.Win32;
using WpfFramework.Core;

namespace GeneralConfigSetter.ViewModels
{
    public class FiledMapperViewModel : ViewModelBase
    {
        private string _queryTag = "";
        private string _selectedMode;
        private bool _isDiFaPaSelected = true;
        private bool _isDiMcSelected = false;
        private IContext _context;

        public string QueryTag
        {
            get { return _queryTag; }
            set
            {
                SetField(ref _queryTag, value, nameof(QueryTag));
            }
        }

        public ObservableCollection<string> Modes
        {
            get
            {
                if (IsDiFaPaSelected)
                {
                    return new ObservableCollection<string>(Services.FieldMappingService.DiFaPaModes);
                }
                else if (IsDiMcSelected)
                {
                    return new ObservableCollection<string>(Services.FieldMappingService.DiMcModes);
                }

                return new ObservableCollection<string>();
            }
        }
        public string SelectedMode
        {
            get { return _selectedMode; }
            set
            {
                SetField(ref _selectedMode, value, nameof(SelectedMode));
            }
        }

        public bool IsDiFaPaSelected
        {
            get { return _isDiFaPaSelected; }
            set
            {
                SetField(ref _isDiFaPaSelected, value, nameof(IsDiFaPaSelected));
                OnPropertyChanged(nameof(Modes));
            }
        }
        public bool IsDiMcSelected
        {
            get { return _isDiMcSelected; }
            set
            {
                SetField(ref _isDiMcSelected, value, nameof(IsDiMcSelected));
                OnPropertyChanged(nameof(Modes));
            }
        }

        public IContext Context
        {
            get { return _context; }
            set
            {
                SetField(ref _context, value, nameof(Context));
            }
        }
        public RelayCommand UpdateConfigCommand { get; set; }
        public RelayCommandGeneric<NotificationModel, bool> ShowMessageCommand { get; internal set; }

        public FiledMapperViewModel(IContext context)
        {
            UpdateConfigCommand = new RelayCommand(UpdateConfig, IsUpdateConfigEnabled);
            Context = context;
        }

        private void UpdateConfig()
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.Filter = "JSON files (*.json)|*.json";
            var result = openFileDialog.ShowDialog();

            if (result != null && result == true)
            {
                //Get the path of specified file
                string filePath = openFileDialog.FileName;

                Services.FieldMappingService.Do(filePath, SelectedMode, QueryTag);
                ShowMessageCommand.Execute(new NotificationModel("SUCCESS!!!!", NotificationType.Information));
            }
            else
            {
                ShowMessageCommand.Execute(new NotificationModel("But why? :(", NotificationType.Error));
            }
        }

        private bool IsUpdateConfigEnabled()
        {
            if (true)
            {
                return true;
            }
            return false;
        }
    }
}
