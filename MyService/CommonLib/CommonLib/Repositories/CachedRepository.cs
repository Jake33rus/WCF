using CommonLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Repositories
{
    public class CashedRepository<T> : SQLRepository<T> where T : class, IId, new()
    {
        Dictionary<int, T> myDict = new Dictionary<int, T>();
        public IEnumerable<T> LoadFromCacheByLinq(Func<T, bool> fwhere)
        {
            var selected = myDict.Values.Where(fwhere);
            return selected;
        }
        public override List<T> Load()
        {
            var list = base.Load();
            foreach (T t in list)
            {
                if (!myDict.ContainsKey(t.Id))
                {
                    T obj = t;
                    myDict.Add(obj.Id, obj);
                }
            }
            return list;
        }
        public T LoadByID(int id)
        {
            T obj = new T();
            if (myDict.TryGetValue(id, out obj))
            {
                return obj;
            }
            else
            {
                obj = db.Set<T>().Where(c => c.Id == id).FirstOrDefault();
                myDict.Add(obj.Id, obj);
                return obj;
            }
        }
    }
}
