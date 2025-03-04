using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using REGLOGPROP_LEASNOTIF.Controller;


namespace REGLOGPROP_LEASNOTIF.Managers
{
    public class Login
    {
        private Controller_rlp controller;
        private Context context;

        public Login(Controller_rlp controller, Context context)
        {
            this.controller = controller;
            this.context = context;
        }

        public void ExecuteLogin()
        {
            Console.WriteLine("Enter UserID:");
            string userId = Console.ReadLine();
            Console.WriteLine("A-Enter Password");
            Console.WriteLine("B-Forget Password");
            char option = Char.Parse(Console.ReadLine());

            if (option == 'A' || option == 'a')
            {
                Console.WriteLine("Enter Password:");
                string password = Console.ReadLine();
                int result = controller.LoginChk(userId, password, out _);

                if (result == 1)
                {
                    Console.WriteLine("LOGIN SUCCESSFUL");
                }
                else
                {
                    Console.WriteLine("SORRY, LOGIN UNSUCCESSFUL DUE TO INCORRECT USERID OR PASSWORD");
                }
            }
            else
            {
                HandleForgotPassword(userId);
            }
        }

        private void HandleForgotPassword(string userId)
        {
            Console.WriteLine("1.Forget the Answer");
            Console.WriteLine("2.Enter Answer");
            int changeAnswer = int.Parse(Console.ReadLine());

            if (changeAnswer == 1)
            {
                Console.WriteLine("Enter User_Id");
                string userId1 = Console.ReadLine();
                Console.WriteLine("Enter New Answer");
                string newAnswer = Console.ReadLine();
                var paak = new SqlParameter("@USERID", userId1);
                var aak = new SqlParameter("@ans", newAnswer);
                context.Database.ExecuteSqlRaw("EXEC Changeans @USERID, @ans", paak, aak);
                Console.WriteLine("Answer with id {0} updated Successfully", userId1);
            }
            else
            {
                Console.WriteLine("Enter Answer");
                string answer = Console.ReadLine();
                Console.WriteLine("Enter New Password:");
                string newPassword = Console.ReadLine();
                int result = controller.LoginChk(userId, null, out bool isPasswordReset, answer, newPassword);

                if (isPasswordReset)
                {
                    Console.WriteLine("Password Updated Successfully");
                }
            }
        }
    }
}
