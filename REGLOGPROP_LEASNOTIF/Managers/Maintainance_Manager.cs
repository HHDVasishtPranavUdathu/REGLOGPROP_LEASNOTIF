using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using REGLOGPROP_LEASNOTIF.Controller;
using REGLOGPROP_LEASNOTIF.Models;

namespace REGLOGPROP_LEASNOTIF.Managers
{
    public class Maintainance_Manager
    {
        public void mm()
        {
            var controller = new Controller_m();
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\nChoose an option:");
                Console.WriteLine("1. Insert a new maintainance request");
                Console.WriteLine("2. View all maintainance requests");
                Console.WriteLine("3. Get status by request ID");
                Console.WriteLine("4. Update status by request ID");
                Console.WriteLine("5. Get all maintainance requests by Tenant ID");
                Console.WriteLine("6. Exit");

                int choice;
                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            InsertMaintainance(controller);
                            break;
                        case 2:
                            ViewMaintainances(controller);
                            break;
                        case 3:
                            GetStatusByRequestId(controller);
                            break;
                        case 4:
                            UpdateStatus(controller);
                            break;
                        case 5:
                            GetMaintainancesByTenantId(controller);
                            break;
                        case 6:
                            exit = true;
                            Console.WriteLine("");
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

        static void InsertMaintainance(Controller_m controller)
        {
            Console.WriteLine("Enter Property ID:");
            int propertyId = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter Tenant ID:");
            string tenantId = Console.ReadLine();

            Console.WriteLine("Enter Description:");
            string description = Console.ReadLine();

            Console.WriteLine("Enter Image Path (optional):");
            string imagePath = Console.ReadLine();

            var maintainance = new Maintainance
            {
                PropertyId = propertyId,
                TenantId = tenantId,
                Description = description,
                Status = "Pending",
                ImagePath = imagePath
            };

            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(maintainance);
            if (!Validator.TryValidateObject(maintainance, validationContext, validationResults, true))
            {
                foreach (var validationResult in validationResults)
                {
                    Console.WriteLine(validationResult.ErrorMessage);
                }
            }
            else
            {
                controller.InsertMaintainance(propertyId, tenantId, description, "Pending", imagePath);
                Console.WriteLine("Maintainance request inserted successfully.");
            }
        }

        static void ViewMaintainances(Controller_m controller)
        {
            var maintainances = controller.ViewMaintainances();

            Console.WriteLine("\nAll Maintainance Requests:");
            foreach (var maintainance in maintainances)
            {
                Console.WriteLine($"RequestId: {maintainance.RequestId}, PropertyId: {maintainance.PropertyId}, TenantId: {maintainance.TenantId}, Description: {maintainance.Description}, Status: {maintainance.Status}, ImagePath: {maintainance.ImagePath}");
            }
        }

        static void GetStatusByRequestId(Controller_m controller)
        {
            Console.WriteLine("Enter Request ID to fetch status:");
            int requestId;
            if (int.TryParse(Console.ReadLine(), out requestId))
            {
                string status = controller.GetStatusByRequestId(requestId);
                if (status != null)
                {
                    Console.WriteLine($"Status for Request ID {requestId}: {status}");
                }
                else
                {
                    Console.WriteLine("Request ID not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid Request ID. Please enter a valid number.");
            }
        }

        static void UpdateStatus(Controller_m controller)
        {
            Console.WriteLine("Enter Request ID to update status:");
            int requestId;
            if (int.TryParse(Console.ReadLine(), out requestId))
            {
                Console.WriteLine("Enter new status:");
                string newStatus = Console.ReadLine();

                bool isUpdated = controller.UpdateStatus(requestId, newStatus);
                if (isUpdated)
                {
                    Console.WriteLine("Status updated successfully.");
                }
                else
                {
                    Console.WriteLine("request Id not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid Request ID. Please enter a valid number.");
            }
        }

        static void GetMaintainancesByTenantId(Controller_m controller)
        {
            Console.WriteLine("Enter Tenant ID to fetch all maintenance requests:");
            int tenantId;
            if (int.TryParse(Console.ReadLine(), out tenantId))
            {
                var tenantMaintainances = controller.GetMaintainancesByTenantId(tenantId);

                if (tenantMaintainances.Any())
                {
                    Console.WriteLine($"\nAll Maintenance Requests for Tenant ID {tenantId}:");
                    foreach (var maintainance in tenantMaintainances)
                    {
                        Console.WriteLine($"RequestId: {maintainance.RequestId}, PropertyId: {maintainance.PropertyId}, TenantId: {maintainance.TenantId}, Description: {maintainance.Description}, Status: {maintainance.Status}, ImagePath: {maintainance.ImagePath}");
                    }
                }
                else
                {
                    Console.WriteLine("No maintenance requests found for the given Tenant ID");
                }
            }
            else
            {
                Console.WriteLine("Invalid Tenant ID. Please enter a valid number.");
            }
        }
    }
}
