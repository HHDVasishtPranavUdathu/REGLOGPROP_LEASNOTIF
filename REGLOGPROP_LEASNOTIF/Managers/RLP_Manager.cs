using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using REGLOGPROP_LEASNOTIF.Models;
using REGLOGPROP_LEASNOTIF.Controller;
using REGLOGPROP_LEASNOTIF.Managers;

namespace REGLOGPROP_LEASNOTIF.Managers
{
    public class RLP_Manager
    {
        public void rlp()
        {
            Console.WriteLine("Hello Dear Customer");
            var controller = new Controller_rlp();
        }
        public static bool TryValidateObject(object obj, out List<ValidationResult> results)
        {
            var context = new ValidationContext(obj, serviceProvider: null, items: null);
            results = new List<ValidationResult>();
            return Validator.TryValidateObject(obj, context, results, validateAllProperties: true);
        }
        public static void InsertOwner()
        {
            try
            {
                Console.WriteLine("Enter Owner_Id");
                string Owner_Id = Console.ReadLine();
                Console.WriteLine("Enter Owner_Name");
                string Owner_Name = Console.ReadLine();
                Console.WriteLine("Enter Owner_Phonenumber");
                long Owner_Phonenumber = long.Parse(Console.ReadLine());
                Console.WriteLine("Enter Owner_DOB");
                DateOnly Owner_DOB = DateOnly.Parse(Console.ReadLine());
                Console.WriteLine("Enter Password");
                string Owner_Password = Console.ReadLine();
                Console.WriteLine("Enter Answer");
                string Owner_Answer = Console.ReadLine();

                Registration aa = new Registration()
                {
                    ID = "O_" + Owner_Id,
                    Name = Owner_Name,
                    PhoneNumber = Owner_Phonenumber,
                    D_O_B = Owner_DOB,
                    Password = Owner_Password,
                    Answer = Owner_Answer
                };

                // Validate the object
                List<ValidationResult> validationResults;
                bool isValid = TryValidateObject(aa, out validationResults);

                if (isValid)
                {
                    var inse = new Controller_rlp();
                    if (inse.Insertion(aa))
                    {
                        Console.WriteLine("Owner Data Inserted Successfully");
                        Console.WriteLine("|");
                        Console.WriteLine("V");
                    }
                }
                else
                {
                    // Display validation errors
                    foreach (var validationResult in validationResults)
                    {
                        Console.WriteLine($"Validation Error: {validationResult.ErrorMessage}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while inserting owner: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }
        }

        public static void InsertTenant()
        {
            try
            {
                Console.WriteLine("Enter Tenant_Id");
                string Tenant_Id = Console.ReadLine();
                Console.WriteLine("Enter Tenant_Name");
                string Tenant_Name = Console.ReadLine();
                Console.WriteLine("Enter Tenant_Phonenumber");
                long Tenant_Phonenumber = long.Parse(Console.ReadLine());
                Console.WriteLine("Enter Tenant_DOB");
                DateOnly Tenant_DOB = DateOnly.Parse(Console.ReadLine());
                Console.WriteLine("Enter Password");
                string Tenant_Password = Console.ReadLine();
                Console.WriteLine("Enter Answer");
                string Tenant_Answer = Console.ReadLine();

                Registration aa = new Registration()
                {
                    ID = "T_" + Tenant_Id,
                    Name = Tenant_Name,
                    PhoneNumber = Tenant_Phonenumber,
                    D_O_B = Tenant_DOB,
                    Password = Tenant_Password,
                    Answer = Tenant_Answer
                };

                // Validate the object
                List<ValidationResult> validationResults;
                bool isValid = TryValidateObject(aa, out validationResults);

                if (isValid)
                {
                    var inse = new Controller_rlp();
                    if (inse.Insertion(aa))
                    {
                        Console.WriteLine("Tenant Data Inserted Successfully");
                        Console.WriteLine("|");
                        Console.WriteLine("V");
                    }
                }
                else
                {
                    // Display validation errors
                    foreach (var validationResult in validationResults)
                    {
                        Console.WriteLine($"Validation Error: {validationResult.ErrorMessage}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while inserting tenant: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }
        }

        public static void InsertProp(string userid)
        {
            try
            {
                //Console.WriteLine("Enter Proper_Id");
                //string prop_id=Console.ReadLine();
                Console.WriteLine("Enter Property Address");
                string address = Console.ReadLine();
                Console.WriteLine("Enter Property Description");
                string description = Console.ReadLine();
                Console.WriteLine("Is the property available? (true/false)");
                bool availableStatus = bool.Parse(Console.ReadLine());
                string ownerId = userid;

                using (var context = new Context())
                {
                    var registration = context.Registrations.FirstOrDefault(r => r.ID == ownerId);
                    if (registration == null)
                    {
                        Console.WriteLine("Owner ID not found.");
                        return;
                    }

                    Prop prop = new Prop()
                    {
                        //Property_Id = prop_id,
                        Address = address,
                        Description = description,
                        AvailableStatus = availableStatus,
                        Owner_Id = ownerId,
                        Registration = registration
                    };

                    // Validate the object
                    List<ValidationResult> validationResults;
                    bool isValid = TryValidateObject(prop, out validationResults);

                    if (isValid)
                    {
                        context.Props.Add(prop);
                        context.SaveChanges();
                        Console.WriteLine("Property Data Inserted Successfully");
                    }
                    else
                    {
                        // Display validation errors
                        foreach (var validationResult in validationResults)
                        {
                            Console.WriteLine($"Validation Error: {validationResult.ErrorMessage}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public static void DisplayRegistrations()
        {
            try
            {
                Controller_rlp cnt = new Controller_rlp();
                List<Registration> list = cnt.readData();
                foreach (Registration e in list)
                {
                    Console.WriteLine("------------------");
                    Console.WriteLine(e.ID + "\n" + e.Name + "\n" + e.PhoneNumber + "\n" + e.D_O_B + "\n" + e.Password + "\n" + e.Answer);
                    Console.WriteLine("------------------");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while displaying registrations: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }
        }

        public static void DisplayProps()
        {
            try
            {
                using (var context = new Context())
                {
                    var props = context.Props
                        .Include(p => p.Registration)
                        .ToList();

                    foreach (var e in props)
                    {
                        Console.WriteLine("------------------");
                        Console.WriteLine($"{e.Property_Id}\n{e.Address}\n{e.Description}\n{e.AvailableStatus}\n{e.Owner_Id}\n{e.Owner_Name}\n{e.Owner_PhoneNumber}");
                        Console.WriteLine("------------------");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while displaying properties: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }
        }

    }
}
