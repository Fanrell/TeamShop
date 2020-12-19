using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamShop.Models;

namespace TeamShop.Tools
{
    public static class Fill
    {
        public static List<Product> FillProducts(List<Product> productsList, string tag)
        {
            List<Product> productsToReturn = new List<Product>();
            foreach(var item in productsList)
            {
                if (item.Tags == tag)
                    productsToReturn.Add(item);
            }
            return productsToReturn;
        }
    }
}
