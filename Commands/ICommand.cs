using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ood_project1;

public interface ICommand
{
    public string ObjectClass { get; set; }
    public List<string> Fields { get; set; }
    public string Conditions { get; set; }
    public void ExecuteCommand(string objectType, string command, Data data);
}
