using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public static class Helper
    {
        public static readonly Dictionary<int, string> RoleList
        = new Dictionary<int, string>
        {
            { 1, "super_admin" },
            { 2, "sale_manager" },
            { 3, "sale" },
            { 4, "seo" },
            { 5, "credit_user" },
            { 6, "register_user" },
            { 7, "visitor" },
        };
    }
    
}