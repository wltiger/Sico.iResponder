using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Sico.iResponder.App.Common
{
    public interface INotifyKeyDown
    {
        void OnKeyDown(object sender, KeyEventArgs e);
    }
}
