using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DebugReplicator.Model
{
    public class IndexedFileModel : FileModel
    {
        private string nombreIndexado;
        public string NombreIndexado
        {
            get { return nombreIndexado; }
            set
            {
                nombreIndexado = value;
                // Call OnPropertyChanged whenever the property is updated
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }
    }
}
