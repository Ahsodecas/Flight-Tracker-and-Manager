using System.Globalization;
using System.ComponentModel;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Mapsui.Projections;
using Avalonia.Rendering;

namespace ood_project1
{
    public class Program
    {
        public static void SetNumberDecimalSeparatorToDot()
        {
            CultureInfo newCulture = new CultureInfo(CultureInfo.InvariantCulture.Name);
            newCulture.NumberFormat.NumberDecimalSeparator = ".";
            CultureInfo.DefaultThreadCurrentCulture = newCulture;
            CultureInfo.DefaultThreadCurrentUICulture = newCulture;
        }
        public static void Main(string[] args)
        {
            SetNumberDecimalSeparatorToDot();
            Projects.Project();
        }
    }
}