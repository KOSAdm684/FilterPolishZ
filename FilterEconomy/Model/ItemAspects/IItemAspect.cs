using FilterPolishUtil;
using FilterPolishUtil.Reflection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace FilterEconomy.Model.ItemAspects
{
    public abstract class AbstractItemAspect : IItemAspect
    {
        static AbstractItemAspect()
        {
            var aspects = ReflectiveEnumerator.GetEnumerableOfType<AbstractItemAspect>().ToList();

            foreach (var item in aspects)
            {
                AvailableAspects.Add(item);
            }
        }

        public static ObservableCollection<AbstractItemAspect> AvailableAspects = new ObservableCollection<AbstractItemAspect>();

        public string Name => this.ToString().SubStringLast(".");
        public virtual string Group => "Ungrouped";
        public virtual SolidColorBrush Color => new SolidColorBrush(Colors.DimGray);
    }

    public interface IItemAspect
    {
        string Group { get; }
        string Name { get; }
        SolidColorBrush Color { get; }
    }

    public class HandledAspect : AbstractItemAspect
    {
        public override string Group => "Meta";
        public DateTime HandlingDate { get; set; }
        public float HanadlingPrice { get; set; }
    }

    public class IgnoreAspect : AbstractItemAspect
    {
        public override string Group => "Meta";
    }

    public class AnchorAspect : AbstractItemAspect
    {
        public override string Group => "Meta";
    }

    public class NonDropAspect : AbstractItemAspect
    {
        public override string Group => "DropType";
    }

    public class BossDropAspect : AbstractItemAspect
    {
        public override string Group => "DropType";
    }

    public class LeagueDropAspect : AbstractItemAspect
    {
        public override string Group => "DropType";
    }

    public class UncommonAspect : AbstractItemAspect
    {
        public override string Group => "DropType";
    }

    public class EarlyLeagueInterestAspect : AbstractItemAspect
    {
        public override string Group => "TemporalAspect";
    }

    public class MetaBiasAspect : AbstractItemAspect
    {
        public override string Group => "TemporalAspect";
    }

    public class ProphecyMaterialAspect : AbstractItemAspect
    {
        public override string Group => "Intent";
    }

    public class ProphecyResultAspect : AbstractItemAspect
    {
        public override string Group => "Intent";
    }

    public class HighVarietyAspect : AbstractItemAspect
    {
        public override string Group => "ItemProperties";
    }

    public class VarietyAspect : AbstractItemAspect
    {
        public override string Group => "ItemProperties";
    }
}
