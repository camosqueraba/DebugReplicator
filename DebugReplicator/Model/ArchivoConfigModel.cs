using DebugReplicator.View.UIControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebugReplicator.Model
{
    public class ArchivoConfigModel : IndexedFileModel
    {
        public ObservableCollection<ClaveValorControl> PropiedadesArchivoConfig { get; set; }
    }
}
