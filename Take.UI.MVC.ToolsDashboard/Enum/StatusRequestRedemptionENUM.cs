using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Take.UI.MVC.ToolsDashboard.Enum
{
    public enum StatusRequestRedemptionENUM
    {
        [Description(description: "Em processo")]
        pending = 1,

        [Description(description: "Concluido")]
        complete = 2,

        [Description(description: "Não foi possível completa a transferência,")]
        error = 3
    }
}
