//using REGLOGPROP_LEASNOTIF;
//using REGLOGPROP_LEASNOTIF.Controller;
//using REGLOGPROP_LEASNOTIF.Managers;
//using REGLOGPROP_LEASNOTIF.Models;

//// Initialize context
//var context = new Context();

//// Initialize controllers and managers
//Controller_ln cr = new Controller_ln();
//var controller = new Controller_rlp();
//RLP_Manager rlpm = new RLP_Manager();
//LeaseManager leaseManager = new LeaseManager(cr);
//NotificationManager notificationManager = new NotificationManager();
//PT_Manager ptm = new PT_Manager(context);
//Maintainance_Manager mm = new Maintainance_Manager();

//try
//{
//    int TT = 0;
//    while (TT != 100)
//    {
//        Console.WriteLine("0. Insert into Owner");
//        Console.WriteLine("1. Insert into Tenant");
//        Console.WriteLine("2. Insert into Property");
//        Console.WriteLine("3. Delete from Property");
//        Console.WriteLine("4. Display Registration Details");
//        Console.WriteLine("5. Display Property Details");
//        Console.WriteLine("6. Insert into Lease");
//        Console.WriteLine("7. Display Notifications");
//        Console.WriteLine("8. Payment Management");
//        Console.WriteLine("9. Maintaince");
//        Console.WriteLine("10. Login");
//        Console.WriteLine("100. Exit");
//        TT = int.Parse(Console.ReadLine());

//        switch (TT)
//        {
//            case 0:
//                RLP_Manager.InsertOwner();
//                break;
//            case 1:
//                RLP_Manager.InsertTenant();
//                break;
//            case 2:
//                RLP_Manager.InsertProp();
//                break;
//            case 3:
//                Console.WriteLine("Enter the Property_Id to delete");
//                int proprty_id = int.Parse(Console.ReadLine());
//                var dts = new Prop()
//                {
//                    Property_Id = proprty_id
//                };

//                if (controller.DeleteProp(dts))
//                {
//                    Console.WriteLine("Property With Id {0} Deleted Sucessfully", proprty_id);
//                }
//                else
//                {
//                    Console.WriteLine("Property With Id {0} Doesn't Exist.", proprty_id);
//                }
//                break;
//            case 4:
//                RLP_Manager.DisplayRegistrations();
//                break;
//            case 5:
//                RLP_Manager.DisplayProps();
//                break;
//            case 6:
//                leaseManager.Lease();
//                break;
//            case 7:
//                notificationManager.ReadNotifications();
//                break;
//            case 8:
//                ptm.pt();
//                break;
//            case 9:
//                mm.mm(); 
//                break;
//            case 10:
//                var li = new Controller_rlp();
//                var cock = new Context();
//                var log = new Login(li, cock);
//                log.ExecuteLogin();
//                break;
//            case 100:
//                string smile = "\u263A";
//                Console.WriteLine("Thanks For Your Time." + smile);
//                break;
//            default:
//                Console.WriteLine("Invalid option.");
//                break;
//        }
//    }
//}
//catch (Exception ex)
//{
//    Console.WriteLine($"An error occurred: {ex.Message}");
//    if (ex.InnerException != null)
//    {
//        Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
//    }
//}


