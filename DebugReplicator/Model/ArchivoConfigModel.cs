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
        //public List<ClaveValorModel> ItemsClaveValor { get; set; }
        public ObservableCollection<ClaveValorControl> PropiedadesArchivoConfig { get; set; }
    }
}
