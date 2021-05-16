using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Take.UI.MVC.ToolsDashboard.Enum
{
    public enum EventLogTypeENUM
    {
        [Description(description: "Sucesso")]
        success = 'S',

        [Description(description: "Aviso")]
        warning = 'W',

        [Description(description: "Erro")]
        error = 'E',
    }
}
