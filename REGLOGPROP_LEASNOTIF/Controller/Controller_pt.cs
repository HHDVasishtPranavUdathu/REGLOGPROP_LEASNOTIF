using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using REGLOGPROP_LEASNOTIF.Models;

namespace REGLOGPROP_LEASNOTIF.Controller
{
    public class Controller_pt
    {
        private Context context;

        public Controller_pt(Context context)
        {
            this.context = context;
        }

        public Controller_pt()
        {
        }

        public void InsertPayment(Paymennts payment)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(payment, serviceProvider: null, items: null);

            if (!Validator.TryValidateObject(payment, validationContext, validationResults, validateAllProperties: true))
            {
                foreach (var validationResult in validationResults)
                {
                    Console.WriteLine(validationResult.ErrorMessage);
                }
                return;
            }

            using (var context = new Context())
            {
                // Fetch tenant details from the Registration table
                var tenantDetails = context.Registrations.SingleOrDefault(r => r.ID == payment.Tenant_Id);
                if (tenantDetails == null)
                {
                    Console.WriteLine("Tenant not found. Please try again.");
                    return;
                }

                // Display tenant details
                Console.WriteLine($"Tenant ID: {tenantDetails.ID}");
                Console.WriteLine($"Tenant Name: {tenantDetails.Name}");
                Console.WriteLine($"Tenant Phone Number: {tenantDetails.PhoneNumber}");
                Console.WriteLine($"Tenant Date of Birth: {tenantDetails.D_O_B}");
                Console.WriteLine($"Tenant Answer: {tenantDetails.Answer}");

                context.Add(payment);
                context.SaveChanges();
            }
        }

        public List<Paymennts> GetAllPayments()
        {
            using (var context = new Context())
            {
                return context.payments.ToList();
            }
        }

        public bool UpdatePaymentStatus(int paymentId, string status)
        {
            using (var context = new Context())
            {
                var command = context.Database.GetDbConnection().CreateCommand();
                command.CommandText = "UpdatePaymentStatus";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var param1 = command.CreateParameter();
                param1.ParameterName = "@PaymentID";
                param1.Value = paymentId;
                command.Parameters.Add(param1);

                var param2 = command.CreateParameter();
                param2.ParameterName = "@Status";
                param2.Value = status;
                command.Parameters.Add(param2);

                context.Database.OpenConnection();
                int rowsAffected = command.ExecuteNonQuery();
                context.Database.CloseConnection();

                return rowsAffected > 0;
            }
        }

        // Get tenant details from Registration table
        public Registration GetTenantDetailsFromRegistration(string tenantId)
        {
            using (var context = new Context())
            {
                return context.Registrations.SingleOrDefault(r => r.ID == tenantId);
            }
        }

        public void FillTenantHistory(string tenantId)
        {
            using (var context = new Context())
            {
                var command = context.Database.GetDbConnection().CreateCommand();
                command.CommandText = "FillTenantHistory";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var param = command.CreateParameter();
                param.ParameterName = "@TenantID";
                param.Value = tenantId;
                command.Parameters.Add(param);

                context.Database.OpenConnection();
                command.ExecuteNonQuery();
                context.Database.CloseConnection();

                Console.WriteLine("Tenant history filled successfully if any payment has Ownerstatus set to True.");
            }
        }

        public void ShowAllPayments()
        {
            using (var context = new Context())
            {
                var payments = context.payments.ToList();
                foreach (var payment in payments)
                {
                    Console.WriteLine($"PaymentID: {payment.PaymentID}, TenantID: {payment.Tenant_Id}, PropertyID: {payment.PropertyId}, Amount: {payment.Amount}, PaymentDate: {payment.PaymentDate}, Status: {payment.Status}, Ownerstatus: {payment.Ownerstatus}");
                }
            }
        }
    }
}
