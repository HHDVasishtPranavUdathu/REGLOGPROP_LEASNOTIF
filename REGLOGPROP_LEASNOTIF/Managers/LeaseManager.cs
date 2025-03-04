using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using REGLOGPROP_LEASNOTIF.Controller;
using REGLOGPROP_LEASNOTIF.Models;

namespace REGLOGPROP_LEASNOTIF.Managers
{
    public class LeaseManager
    {
        private readonly Controller_ln cr;
        private readonly NotificationManager notificationManager;

        public LeaseManager(Controller_ln controller)
        {
            cr = controller;
            notificationManager = new NotificationManager();
        }

        public (string leaseId, string ownerId)? Lease()
        {
            Console.WriteLine("Enter Id:");
            string? id = Console.ReadLine();

            Console.WriteLine("Enter Property Id:");
            int? propertyId = Convert.ToInt32(Console.ReadLine());
            var property = cr.cc.Props.Include(p => p.Registration).FirstOrDefault(p => p.Property_Id == propertyId);
            if (property != null)
            {
                Console.WriteLine($"Owner Name: {property.Owner_Name}");
                Console.WriteLine($"Owner Phone: {property.Owner_PhoneNumber}");
                DateTime startDate;
                while (true)
                {
                    Console.WriteLine("Enter Start Date (yyyy-mm-dd):");
                    if (DateTime.TryParse(Console.ReadLine(), out startDate))
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid date format. Please enter a valid date in yyyy-mm-dd format.");
                    }
                }

                DateTime endDate;
                while (true)
                {
                    Console.WriteLine("Enter End Date (yyyy-mm-dd):");
                    if (DateTime.TryParse(Console.ReadLine(), out endDate))
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid date format. Please enter a valid date in yyyy-mm-dd format.");
                    }
                }

                Lease lease = new Lease
                {
                    ID = id,
                    Property_Id = propertyId,
                    StartDate = startDate,
                    EndDate = endDate,
                    Lease_status = false
                };

                Console.WriteLine("Enter Tenant Signature:");
                string? inputTenantSignature = Console.ReadLine();
                var signatureMatchTenant = cr.cc.Registrations.FirstOrDefault(r => r.ID == id)?.Signature;
                if (inputTenantSignature != signatureMatchTenant)
                {
                    Console.WriteLine("Tenant signature validation failed. Lease data not inserted.");
                    return null;
                }
                lease.Tenant_Signature = true;
                string? ownerId = cr.GetOwnerIdByPropertyId(propertyId);
                var signatureMatchOwner = cr.cc.Registrations.FirstOrDefault(r => r.ID == ownerId)?.Signature;
                notificationManager.InsertNotification(id, ownerId, "Tenant signed the lease");

                Console.WriteLine("Enter Owner Signature:");
                string? tempOwnerSignature = Console.ReadLine();

                if (tempOwnerSignature != signatureMatchOwner)
                {
                    Console.WriteLine("Owner signature validation failed. Lease data not inserted.");
                    return null;
                }
                lease.Owner_Signature = true;
                notificationManager.InsertNotification(id, ownerId, "Owner signed the lease");

                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(lease);
                bool isValidEntity = Validator.TryValidateObject(lease, validationContext, validationResults, true);

                if (!isValidEntity)
                {
                    Console.WriteLine("Validation failed. Lease data not inserted.");
                    foreach (var validationResult in validationResults)
                    {
                        Console.WriteLine(validationResult.ErrorMessage);
                    }
                    return null;
                }

                lease.Lease_status = true;

                cr.InsertData_Lease(lease);

                Console.WriteLine("Lease data inserted successfully.");
                DisplayLeaseDetails(lease);

                notificationManager.InsertNotification("sys", "sys", "Both parties signed the lease and it is successfully inserted");
                return (lease.ID, ownerId);
            }
            else
            {
                Console.WriteLine("Property not found.");
                return null;
            }
        }

        public void DisplayLeaseDetails(Lease lease)
        {
            Console.WriteLine("Lease Details:");
            Console.WriteLine($"Tenant Id: {lease.ID}");
            Console.WriteLine($"Property Id: {lease.Property_Id}");
            Console.WriteLine($"Start Date: {lease.StartDate}");
            Console.WriteLine($"End Date: {lease.EndDate}");
            Console.WriteLine($"Tenant Signature: {lease.Tenant_Signature}");
            Console.WriteLine($"Owner Signature: {lease.Owner_Signature}");
            Console.WriteLine($"Lease Status: {lease.Lease_status}");
        }
    }

}
