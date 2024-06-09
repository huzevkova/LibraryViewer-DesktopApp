using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KniznicaWpfApp
{
     //často používané konštanty
     internal record StringKonst
     { 
          public const string HlavnyZdroj = "Resources/";
          public const string AutoriSubor = "autori.json";
          public const string KnihySubor = "knihy.json";
          public const string ObrazkyZdrojKnihy = HlavnyZdroj + "KnihyObrazky/";
          public const string ObrazkyZdrojAutori = HlavnyZdroj + "AutoriObrazky/";
          public const string KnihyDefault = "defaultB.png";
          public const string AutoriDeafult = "defaultA.png";
          public const string AutoriNadpis = "AUTORI";
          public const string ObrazkyKnihyKos = ObrazkyZdrojKnihy + "Kos/";
          public const string ObrazkyAutoriKos = ObrazkyZdrojKnihy + "Kos/";
     }
}
