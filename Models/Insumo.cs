using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ejercicio04.Models
{
    public class Insumo
    {
        public int codigo { get; set; }
        public string descripcion { get; set; }
        public string medida { get; set; }
        public decimal preCosto { get; set; }
        public int stock { get; set; }
    }
}