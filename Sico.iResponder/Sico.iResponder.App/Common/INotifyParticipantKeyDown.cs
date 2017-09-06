using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sico.iResponder.App.Common
{
    public interface INotifyParticipantKeyDown
    {
        void OnParticipantKeyDown(object sender, ParticipantKey key);
    }
}
