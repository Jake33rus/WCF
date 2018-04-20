using CommonLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Repositories
{
    public class ImmoRepos : CashedRepository<Immovables>
    {
        public byte[] GetVersion()
        {
            DataBaseContext db = new DataBaseContext();
            var max = db.Immovables.Max(x => x.Version);
            return max;
        }
    }
}
