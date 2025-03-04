using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using REGLOGPROP_LEASNOTIF.Controller;
using REGLOGPROP_LEASNOTIF.Models;

namespace REGLOGPROP_LEASNOTIF.Managers
{
    public class MaintenanceManager
    {
        public async Task RunAsync()
        {
            var controller = new Controller_m();
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\nChoose an option:");
                Console.WriteLine("1. Insert a new maintenance request");
                Console.WriteLine("2. View all maintenance requests");
                Console.WriteLine("3. Get status by request ID");
                Console.WriteLine("4. Update status by request ID");
                Console.WriteLine("5. Get all maintenance requests by Tenant ID");
                Console.WriteLine("6. Fetch and display Property ID and Tenant ID from Lease");
                Console.WriteLine("7. Exit");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    switch (choice)
                    {
                        case 1:
                            await InsertMaintenanceAsync(controller);
                            break;
                        case 2:
                            await ViewMaintenancesAsync(controller);
                            break;
                        case 3:
                            await GetStatusByRequestIdAsync(controller);
                            break;
                        case 4:
                            await UpdateStatusAsync(controller);
                            break;
                        case 5:
                            await GetMaintenancesByTenantIdAsync(controller);
                            break;
                        case 6:
                            await FetchAndDisplayPropertyAndTenantIdAsync(controller);
                            break;
                        case 7:
                            exit = true;
                            Console.WriteLine("Exiting...");
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                }
            }
        }

        static async Task InsertMaintenanceAsync(Controller_m controller)
        {
            Console.WriteLine("Enter Property ID:");
            if (!int.TryParse(Console.ReadLine(), out int propertyId))
            {
                Console.WriteLine("Invalid Property ID. Please enter a valid number.");
                return;
            }

            Console.WriteLine("Enter Tenant ID:");
            string tenantId = Console.ReadLine();

            Console.WriteLine("Enter Description:");
            string description = Console.ReadLine();

            Console.WriteLine("Enter Image Path (optional):");
            string imagePath = Console.ReadLine();

            var maintenance = new Maintenance
            {
                PropertyId = propertyId,
                TenantId = tenantId,
                Description = description,
                Status = "Pending",
                ImagePath = imagePath
            };

            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(maintenance);
            if (!Validator.TryValidateObject(maintenance, validationContext, validationResults, true))
            {
                foreach (var validationResult in validationResults)
                {
                    Console.WriteLine(validationResult.ErrorMessage);
                }
            }
            else
            {
                await controller.InsertMaintenanceAsync(propertyId, tenantId, description, "Pending", imagePath);
                Console.WriteLine("Maintenance request inserted successfully.");
            }
        }

        static async Task ViewMaintenancesAsync(Controller_m controller)
        {
            var maintenances = await controller.ViewMaintenancesAsync();

            Console.WriteLine("\nAll Maintenance Requests:");
            foreach (var maintenance in maintenances)
            {
                Console.WriteLine($"RequestId: {maintenance.RequestId}, PropertyId: {maintenance.PropertyId}, TenantId: {maintenance.TenantId}, Description: {maintenance.Description}, Status: {maintenance.Status}, ImagePath: {maintenance.ImagePath}");
            }
        }

        static async Task GetStatusByRequestIdAsync(Controller_m controller)
        {
            Console.WriteLine("Enter Request ID to fetch status:");
            if (!int.TryParse(Console.ReadLine(), out int requestId))
            {
                Console.WriteLine("Invalid Request ID. Please enter a valid number.");
                return;
            }

            string status = await controller.GetStatusByRequestIdAsync(requestId);
            if (status != null)
            {
                Console.WriteLine($"Status for Request ID {requestId}: {status}");
            }
            else
            {
                Console.WriteLine("Request ID not found.");
            }
        }

        static async Task UpdateStatusAsync(Controller_m controller)
        {
            Console.WriteLine("Enter Request ID to update status:");
            if (!int.TryParse(Console.ReadLine(), out int requestId))
            {
                Console.WriteLine("Invalid Request ID. Please enter a valid number.");
                return;
            }

            Console.WriteLine("Enter new status:");
            string newStatus = Console.ReadLine();

            bool isUpdated = await controller.UpdateStatusAsync(requestId, newStatus);
            if (isUpdated)
            {
                Console.WriteLine("Status updated successfully.");
            }
            else
            {
                Console.WriteLine("Request ID not found.");
            }
        }

        static async Task GetMaintenancesByTenantIdAsync(Controller_m controller)
        {
            Console.WriteLine("Enter Tenant ID to fetch all maintenance requests:");
            string tenantId = Console.ReadLine();

            var tenantMaintenances = await controller.GetMaintenancesByTenantIdAsync(tenantId);

            if (tenantMaintenances.Any())
            {
                Console.WriteLine($"\nAll Maintenance Requests for Tenant ID {tenantId}:");
                foreach (var maintenance in tenantMaintenances)
                {
                    Console.WriteLine($"RequestId: {maintenance.RequestId}, PropertyId: {maintenance.PropertyId}, TenantId: {maintenance.TenantId}, Description: {maintenance.Description}, Status: {maintenance.Status}, ImagePath: {maintenance.ImagePath}");
                }
            }
            else
            {
                Console.WriteLine("No maintenance requests found for the given Tenant ID.");
            }
        }

        static async Task FetchAndDisplayPropertyAndTenantIdAsync(Controller_m controller)
        {
            Console.WriteLine("Enter Lease ID to fetch Property ID and Tenant ID:");
            if (!int.TryParse(Console.ReadLine(), out int leaseId))
            {
                Console.WriteLine("Invalid Lease ID. Please enter a valid number.");
                return;
            }

            var result = await controller.GetPropertyAndTenantIdFromLeaseAsync(leaseId);
            if (result.HasValue)
            {
                Console.WriteLine($"Property ID: {result.Value.PropertyId}, Tenant ID: {result.Value.TenantId}");
            }
            else
            {
                Console.WriteLine("Lease ID not found.");
            }
        }
    }
}
