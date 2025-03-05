using REGLOGPROP_LEASNOTIF;
using REGLOGPROP_LEASNOTIF.Controller;
using REGLOGPROP_LEASNOTIF.Managers;
using REGLOGPROP_LEASNOTIF.Models;

namespace REGLOGPROP_LEASNOTIF
{
    class Program
    {
        public static void Main(string[] args)
        {
            // Initialize context
            var context = new Context();
            var code_java = false;
            string name ="";

            // Initialize controllers and managers
            Controller_ln cr = new Controller_ln();
            var controller = new Controller_rlp();
            RLP_Manager rlpm = new RLP_Manager();
            LeaseManager leaseManager = new LeaseManager(cr);
            NotificationManager notificationManager = new NotificationManager();
            PT_Manager ptm = new PT_Manager(context);
            MaintenanceManager mm = new MaintenanceManager();

            try
            {
                Console.WriteLine("1. Register");
                Console.WriteLine("2. Login");
                var choice = int.Parse(Console.ReadLine());

                if (choice == 1)
                { 
                    Console.WriteLine("Are you looking to Register has a (1)owner or (2)tenant");
                    var Chio = int.Parse(Console.ReadLine());
                    if (Chio == 1) { 
                        RLP_Manager.InsertOwner();
                    }
                    else if(Chio == 2) 
                    {
                        RLP_Manager.InsertTenant();
                        
                    }
                    else {
                        Console.WriteLine("invalid ra guudi puka choosi kottara 1 or 2 antey ");
                        }
                }

                else if (choice == 2)
                {
                    Console.WriteLine("enter userid");
                    string userId = Console.ReadLine();
                    name = userId;
                    var li = new Controller_rlp();
                    var cock = new Context();
                    var log = new Login(li, cock);
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
                            code_java = true;
                            
                        }
                        else
                        {
                            Console.WriteLine("SORRY, LOGIN UNSUCCESSFUL DUE TO INCORRECT USERID OR PASSWORD");
                        }
                    }
                    else
                    {
                       log.HandleForgotPassword(userId);
                    }
                    log.ExecuteLogin();
                    
                    if (userId.StartsWith("T_") && code_java==true)
                    {
                        bool tenantExit = false;
                        while (!tenantExit)
                        {
                            Console.WriteLine("Choose an option:");
                            Console.WriteLine("1. Display property details");
                            Console.WriteLine("2. Insert into lease");
                            Console.WriteLine("3. Notifications");
                            Console.WriteLine("4. Payment methods");
                            Console.WriteLine("5. Maintenance");
                            Console.WriteLine("6. Exit");
                            Console.WriteLine(name);

                            var tenantChoice = Console.ReadLine();
                            switch (tenantChoice)
                            {
                                case "1":
                                    RLP_Manager.DisplayProps();
                                    break;
                                
                                case "2":
                                    leaseManager.Lease(name);
                                    break;
                                case "3":
                                    notificationManager.ReadNotifications();
                                    break;
                                case "4":
                                    ptm.pt();
                                    break;
                                case "5":
                                    mm.RunAsync();
                                    break;
                                case "6":
                                    tenantExit = true;
                                    break;
                                default:
                                    Console.WriteLine("Invalid choice. Please try again.");
                                    break;
                            }
                        }
                    }
                    else if (userId.StartsWith("O_") && code_java==true)
                    {
                        bool ownerExit = false;
                        while (!ownerExit)
                        {
                            Console.WriteLine("Choose an option:");
                            Console.WriteLine("1. Insert property");
                            Console.WriteLine("2. Delete property");
                            Console.WriteLine("3. Display property");
                            Console.WriteLine("4. Insert into lease");
                            Console.WriteLine("5. Payment management");
                            Console.WriteLine("6. Maintenance");
                            Console.WriteLine("7. Exit");

                            var ownerChoice = Console.ReadLine();
                            switch (ownerChoice)
                            {
      
                                case "1":
                                    RLP_Manager.InsertProp(); 
                                    break;
                                case "2":
                                    Console.WriteLine("Enter the Property_Id to delete");
                                    int proprty_id = int.Parse(Console.ReadLine());
                                    var dts = new Prop()
                                    {
                                        Property_Id = proprty_id
                                    };
                                    Controller_rlp kkkk = new Controller_rlp();
                                    if (kkkk.DeleteProp(dts))
                                    {

                                        Console.WriteLine("Property With Id {0} Deleted Sucessfully", proprty_id);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Property With Id {0} Doesn't Exist.", proprty_id);
                                    }
                                    break;
                                    
                                case "3":
                                    RLP_Manager.DisplayProps();
                                    break;

                                case "4":
                                    leaseManager.DisplayLeasesByOwner(name);
                                    Console.WriteLine("Enter Lease ID:");
                                        int leaseId = Convert.ToInt32(Console.ReadLine());
                                        
                                        bool success = leaseManager.UpdateOwnerSignatureAndFinalizeLease(leaseId, name);

                                        if (success)
                                        {
                                            Console.WriteLine("The lease has been successfully finalized with the owner's signature.");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failed to finalize the lease. Please check the inputs or try again.");
                                        }
                                        break;
                                    

                                case "5":
                                    ptm.pt();
                                    break;
                                case "6":
                                    mm.RunAsync();
                                    break;
                                case "7":
                                    ownerExit = true;
                                    break;
                                default:
                                    Console.WriteLine("Invalid choice. Please try again.");
                                    break;
                            }
                        }
                    }
                  
                }
                else
                {
                    Console.WriteLine("Invalid option.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }
        }
    }
}
//hi
