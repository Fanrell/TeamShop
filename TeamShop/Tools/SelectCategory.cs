using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamShop.Models;

namespace TeamShop.Tools
{
    public static class SelectCategory
    {
        public static List<Product> AboutTags(List<Product> products, string[] tags, int id)
        {
            List<Product> toReturn = new List<Product>();
            string tag = tags[id];
            foreach (var item in products)
            {
                if (item.Tags == tag)
                    toReturn.Add(item);
            }
            return toReturn;
        }

        public static List<Product> AboutCategories(List<Product> products, string[] categories, int id)
        {
            List<Product> toReturn = new List<Product>();
            string cat = categories[id];
            foreach (var item in products)
            {
                if (item.Tags == cat)
                    toReturn.Add(item);
            }
            return toReturn;
        }
    }
}
