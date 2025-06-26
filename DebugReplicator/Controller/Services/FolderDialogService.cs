using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DebugReplicator.Controller.Services
{
    public class FolderDialogService : IFolderDialogService
    {
        public string SeleccionarCarpeta()
        {            
            string folderName = string.Empty;
            CommonOpenFileDialog openFileDialog = new CommonOpenFileDialog();
            openFileDialog.IsFolderPicker = true;
            openFileDialog.DefaultFileName = string.Empty;
            openFileDialog.InitialDirectory = Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + "Downloads";

            CommonFileDialogResult result = openFileDialog.ShowDialog();

            if (result == CommonFileDialogResult.Ok)            
                folderName = openFileDialog.FileName;

            return folderName;
        }
    }
}
