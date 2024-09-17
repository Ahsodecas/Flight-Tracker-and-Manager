using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ood_project1;
public interface IReportable
{
    public string Accept(INewsProviders visitor);
}
