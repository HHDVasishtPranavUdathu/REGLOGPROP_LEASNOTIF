using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using REGLOGPROP_LEASNOTIF.Models;

namespace REGLOGPROP_LEASNOTIF.Controller
{
    public class Controller_rlp
    {
        Context cc = new Context();

        public int LoginChk(string userId, string password, out bool isPasswordReset, string answer = null, string newPassword = null)
        {
            isPasswordReset = false;
            if (password != null)
            {
                var u = new SqlParameter("@UserName", userId);
                var p = new SqlParameter("@Password", password);
                var resParam = new SqlParameter("@res", System.Data.SqlDbType.Int)
                {
                    Direction = System.Data.ParameterDirection.Output
                };

                cc.Database.ExecuteSqlRaw("EXEC spGetLoginDetails @UserName, @Password, @res OUTPUT", u, p, resParam);

                return (int)resParam.Value;
            }
            else if (answer != null && newPassword != null)
            {
                var paak = new SqlParameter("@USERID", userId);
                var kaak = new SqlParameter("@pass", newPassword);
                var aak = new SqlParameter("@ans", answer);
                cc.Database.ExecuteSqlRaw("EXEC ChangePas @USERID, @pass, @ans", paak, kaak, aak);
                isPasswordReset = true;
                return 1;
            }
            return 0;


        }
        public bool Insertion(Registration a)
        {
            using (var con = new Context())
            {
                con.Add(a);
                con.SaveChanges();
                return true;
            }
        }

        public bool Inser(Property a)
        {
            using (var con = new Context())
            {
                con.Add(a);
                con.SaveChanges();
                return true;
            }
        }

        public bool DeleteProp(Prop kmm)
        {
            using (var con = new Context())
            {
                var ppp = con.Props.Find(kmm.Property_Id);
                if (ppp != null)
                {
                    con.Remove(ppp);
                    con.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public List<Registration> readData()
        {
            return cc.Registrations.ToList();
        }

        public List<Prop> viewData()
        {
            return cc.Props.ToList();
        }
    }
}
