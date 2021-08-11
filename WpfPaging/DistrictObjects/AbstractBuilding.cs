using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DistrictSupplySolution.DistrictObjects
{
    /// <summary>
    /// Класс для хранения информации  о здании для рассчёта коэфициента участия в максимуме и нагрузки микрорайноа
    /// </summary>
    public class AbstractBuilding : BindableBase
    {
        public Guid Id { get; set; }
        public string PlanNumber { get; set; }
        public string Type { get; set; }
        public string SideNote { get; set; }
        public double ActivePower { get; set; }
        public double ReactivePower { get; set; }
        public double FullPower { get; set; }
        public double CoefficientOfMax { get; set; }

        public ObservableCollection<SpecialCoefficientOfMax> SpecialConsumerCoefficientsOfMax { get; set; } = new ObservableCollection<SpecialCoefficientOfMax>();

        public AbstractBuilding()
        {
            SpecialConsumerCoefficientsOfMax = new ObservableCollection<SpecialCoefficientOfMax> 
            {
                new SpecialCoefficientOfMax {Type = "Житлові будинки з електроплитами" },
                new SpecialCoefficientOfMax {Type = "Житлові будинки з газовими плитами або на твердому паливі" },
                new SpecialCoefficientOfMax {Type = "Підприємства громадського харчування (їдальні, ресторани, кафе)" },
                new SpecialCoefficientOfMax {Type = "Школи, середні навчальні заклади, ПТУ, бібліотеки" },
                new SpecialCoefficientOfMax {Type = "Торгові підприємства одно-, півтора- та двозмінні" },
                new SpecialCoefficientOfMax {Type = "Установи адміністративного управління, фінансові, проектно-конструкторські організації" },
                new SpecialCoefficientOfMax {Type = "Готелі" },
                new SpecialCoefficientOfMax {Type = "Поліклініки" },
                new SpecialCoefficientOfMax {Type = "Ательє та інші підприємства побутового обслуговування" },
                new SpecialCoefficientOfMax {Type = "Культові, культурно-видовищні та дозвіллєві заклади" },

            };
        }

    }
}
