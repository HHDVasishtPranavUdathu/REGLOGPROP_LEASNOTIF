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

        public (string leaseId, string ownerId)? Lease(string? id)
        {
            Console.WriteLine("Enter Property Id:");
            int? propertyId = Convert.ToInt32(Console.ReadLine());

            var existingLease = cr.cc.Leases.FirstOrDefault(l => l.Property_Id == propertyId && l.Lease_status == true);
            if (existingLease != null)
            {
                Console.WriteLine("This property is already under an active lease. A new lease cannot be created.");
                return null;
            }

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
                        if (endDate > startDate)
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("End date must be greater than start date");
                        }
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
                    Owner_Signature = false,
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

                notificationManager.InsertNotification(id, ownerId, "Tenant signed the lease");

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

                cr.InsertData_Lease(lease);

                Console.WriteLine("Lease data inserted successfully.");
                DisplayLeaseDetails(lease);

                notificationManager.InsertNotification(id, ownerId, "Tenant signed the lease. Awaiting owner signature.");
                return (lease.ID, ownerId);
            }
            else
            {
                Console.WriteLine("Property not found.");
                return null;
            }
        }

        public bool UpdateOwnerSignatureAndFinalizeLease(int leaseId, string ownerId)
        {
            
            // Find the lease by its ID
            var lease = cr.cc.Leases.FirstOrDefault(l => l.LeaseId == leaseId);
            if (lease == null)
            {
                Console.WriteLine("Lease not found.");
                return false;
            }

            // Validate that the lease has not been finalized
            if (lease.Lease_status == true)
            {
                Console.WriteLine("The lease is already finalized.");
                return false;
            }

            // Get the expected owner signature from the database
            var expectedOwnerSignature = cr.cc.Registrations.FirstOrDefault(r => r.ID == ownerId)?.Signature;
            
            // Prompt for the owner's signature
            Console.WriteLine("Enter Owner Signature:");
            string? inputOwnerSignature = Console.ReadLine();

            if (inputOwnerSignature != expectedOwnerSignature)
            {
                Console.WriteLine("Owner signature validation failed.");
                return false;
            }

            // Update owner signature and lease status
            lease.Owner_Signature = true;
            lease.Lease_status = true;

            // Save changes to the database
            cr.cc.SaveChanges();

            // Send notifications
            notificationManager.InsertNotification(ownerId, lease.ID, "Owner signed the lease and the lease is finalized.");
            notificationManager.InsertNotification(ownerId, lease.ID, $"Lease {leaseId} is finalized.");

            Console.WriteLine("Owner signature added and lease finalized successfully.");
            return true;
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


        public void DisplayLeasesByOwner(string ownerId)
        {
            // Get all properties owned by the owner
            var properties = cr.cc.Props.Where(p => p.Owner_Id == ownerId).ToList();

            if (properties.Count == 0)
            {
                Console.WriteLine("No properties found for this owner.");
                return;
            }

            // Extract the Property IDs into a list
            var propertyIds = properties.Select(p => p.Property_Id).ToList();

            // Get all leases associated with the owner's properties
            var leases = cr.cc.Leases
                .Include(l => l.Prop)
                .Include(l => l.Tenant)
                .Where(l => propertyIds.Contains((int)l.Property_Id))
                .ToList();

            if (leases.Count == 0)
            {
                Console.WriteLine("No leases found for the properties owned by this owner.");
                return;
            }

            // Display the leases
            Console.WriteLine($"Leases under the access of owner (ID: {ownerId}):");
            foreach (var lease in leases)
            {
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine($"Lease ID: {lease.LeaseId}");
                Console.WriteLine($"Tenant ID: {lease.ID}");
                Console.WriteLine($"Property ID: {lease.Property_Id}");
                Console.WriteLine($"Start Date: {lease.StartDate?.ToString("yyyy-MM-dd")}");
                Console.WriteLine($"End Date: {lease.EndDate?.ToString("yyyy-MM-dd")}");
                Console.WriteLine($"Tenant Signature: {lease.Tenant_Signature}");
                Console.WriteLine($"Owner Signature: {lease.Owner_Signature}");
                Console.WriteLine($"Lease Status: {lease.Lease_status}");
                Console.WriteLine("--------------------------------------------");
            }
        }

    }
}
