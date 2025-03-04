using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using REGLOGPROP_LEASNOTIF.Controller;
using REGLOGPROP_LEASNOTIF.Models;
using REGLOGPROP_LEASNOTIF.Managers;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;

namespace REGLOGPROP_LEASNOTIF.Managers
{
    public class PT_Manager
    {
        private readonly Controller_pt tenantController;
        private readonly Controller_pt paymentController;
        public readonly Controller_ln cr;

        public PT_Manager(Context context)
        {
            cr = new Controller_ln();
            tenantController = new Controller_pt(context);
            paymentController = new Controller_pt(context);
        }

        public void pt()
        {
            bool exit = false;

            while (!exit)
            {
                try
                {
                    Console.WriteLine("Enter your ID (Tenant ID starts with 'T_', Owner ID starts with 'O_'):");
                    string userId = Console.ReadLine();

                    if (userId.StartsWith("T_"))
                    {
                        var tenant = tenantController.GetTenantDetailsFromRegistration(userId);
                        if (tenant == null)
                        {
                            Console.WriteLine("Tenant not found. Please try again.");
                            continue;
                        }

                        Console.WriteLine($"Welcome, {tenant.Name} (ID: {userId})");

                        bool tenantExit = false;
                        while (!tenantExit)
                        {
                            Console.WriteLine("Choose an option:");
                            Console.WriteLine("1. Make a payment");
                            Console.WriteLine("2. Check payment history");
                            Console.WriteLine("3. Exit");

                            var choice = Console.ReadLine();
                            switch (choice)
                            {
                                case "1":
                                    MakePayment(tenantController, paymentController, userId);
                                    break;
                                case "2":
                                    var blue = new Controller_pt();
                                    blue.ShowAllPayments();
                                    break;
                                case "3":
                                    tenantExit = true;
                                    break;
                                default:
                                    Console.WriteLine("Invalid choice. Please enter 1, 2, or 3.");
                                    break;
                            }
                        }
                    }
                    else if (userId.StartsWith("O_"))
                    {
                        // Owner Login
                        var tenant = tenantController.GetTenantDetailsFromRegistration(userId);
                        if (tenant == null)
                        {
                            Console.WriteLine("Tenant not found. Please try again.");
                            continue;
                        }

                        Console.WriteLine($"Welcome, {tenant.Name} (ID: {userId})");

                        bool ownerExit = false;
                        while (!ownerExit)
                        {
                            Console.WriteLine("Choose an option:");
                            Console.WriteLine("1. Update payment status");
                            Console.WriteLine("2. Exit");

                            var choice = Console.ReadLine();
                            switch (choice)
                            {
                                case "1":
                                    var blue = new Controller_pt();
                                    blue.ShowAllPayments();
                                    UpdatePaymentStatus(paymentController);
                                    break;
                                case "2":
                                    ownerExit = true;
                                    break;
                                default:
                                    Console.WriteLine("Invalid choice. Please enter 1 or 2.");
                                    break;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid ID. Please try again.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }

        static void MakePayment(Controller_pt tenantController, Controller_pt paymentController, string tenantId)
        {
            NotificationManager notificationManager = new NotificationManager();
            Controller_ln cr = new Controller_ln();
            // Fetch tenant details from the Registration table
            var tenantDetails = tenantController.GetTenantDetailsFromRegistration(tenantId);

            if (tenantDetails == null)
            {
                Console.WriteLine("Tenant not found. Please try again.");
                return;
            }

            Console.WriteLine($"Tenant ID: {tenantDetails.ID}");
            Console.WriteLine($"Tenant Name: {tenantDetails.Name}");
            Console.WriteLine($"Tenant Phone Number: {tenantDetails.PhoneNumber}");
            Console.WriteLine($"Tenant Date of Birth: {tenantDetails.D_O_B}");
            Console.WriteLine($"Tenant Answer: {tenantDetails.Answer}");
            Console.WriteLine("Enter Property ID:");
            int propertyId = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter Amount:");
            decimal amount = Convert.ToDecimal(Console.ReadLine());
            Console.WriteLine("Enter status:");
            var status = Console.ReadLine();

            var payment = new Paymennts
            {
                Tenant_Id = tenantId,
                PropertyId = propertyId,
                Amount = amount,
                Status = status,
                Ownerstatus = "False"
            };
            paymentController.InsertPayment(payment);
            Console.WriteLine("Payment inserted successfully.");
            var ownerId = cr.cc.Props.Include(p => p.Registration).FirstOrDefault(p => p.Property_Id == propertyId)?.Owner_Id;
            notificationManager.InsertNotification(tenantDetails.ID, ownerId, "Made payment by tenant, waiting for owner verification");
            
        }

        static void UpdatePaymentStatus(Controller_pt paymentController)
        {


            Console.WriteLine("Enter Payment ID:");
            int paymentId = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter new payment status (done/yes/false):");
            string status = Console.ReadLine();

            var statusUpdated = paymentController.UpdatePaymentStatus(paymentId, status);
            if (statusUpdated)
            {
                Console.WriteLine("Payment status updated successfully.");
                
            }
            else
            {
                Console.WriteLine("Payment ID not found or status update failed.");
            }
        }
    }
}
