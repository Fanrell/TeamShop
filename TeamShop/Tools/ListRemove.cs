using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamShop.Models;

namespace TeamShop.Tools
{
    public static class ListRemove
    {
        public static void Remove(ref List<Product> list, int id)
        {
            List<Product> listToReturn = new List<Product>();
            foreach(var item in list)
            {
                if (item.Id != id)
                {
                    listToReturn.Add(item);
                    
                }
                else
                {
                    id = -1;
                }
            }
            list = listToReturn;
        }
    }
}
