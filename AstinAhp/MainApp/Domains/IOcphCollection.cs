using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.Domains
{
  public  interface IOcphCollection<T>:ICollection<T>
    {
        bool Update(T t);
        T SelectedItem { get; set; }
    }
}
