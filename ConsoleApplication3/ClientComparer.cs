using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication3
{
    public class ClientComparer : IEqualityComparer<Client>
    {
        public bool Equals(Client x, Client y)
        {
            return x.Name == y.Name;
        }

        public int GetHashCode(Client obj)
        {
            return obj.Name.GetHashCode();
        }
    }
}
