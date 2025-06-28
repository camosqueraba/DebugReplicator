using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace DebugReplicator.Model
{
    public class FileModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        

        // Using icon because it's easier

        public Icon Icon { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

        public FileType Type { get; set; }

        public long SizeBytes { get; set; }

        private bool seleccionado;
        public bool Seleccionado
        {
            get { return seleccionado; }
            set
            {
                seleccionado = value;
                // Call OnPropertyChanged whenever the property is updated
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        // Create the OnPropertyChanged method to raise the event
        // The calling member's name will be used as the parameter.
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public FileModel Contenido {  get; set; } 

        public bool IsFile => Type == FileType.File;
        public bool IsFolder => Type == FileType.Folder;
        public bool IsDrive => Type == FileType.Drive;
        public bool IsShortcut => Type == FileType.Shortcut;
    }
}
