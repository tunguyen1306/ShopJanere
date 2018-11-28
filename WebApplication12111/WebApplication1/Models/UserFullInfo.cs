using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class AllLoggedUserInfo
    {
        public user user;
        public user_role user_role;
        public role role;
        public userdata userdata;
        public AllLoggedUserInfo(user tem)
        {
            user = new user();
            user_role = new user_role();
            userdata = new userdata();

            veebdbEntities data = new veebdbEntities();
             user=data.users.Where(m=>m.Id == tem.Id).FirstOrDefault();
             user_role= data.user_role.Where(m => m.userid == user.Id).FirstOrDefault();
             userdata = data.userdatas.Where(m => m.userid == user.Id).FirstOrDefault();
             role = data.roles.Where(m => m.Id == user_role.roleid).FirstOrDefault();
        }
        public AllLoggedUserInfo()
        {
            user = new user();
            user_role = new user_role();
            userdata = new userdata();
            role = new role();

            
        }
    }
}