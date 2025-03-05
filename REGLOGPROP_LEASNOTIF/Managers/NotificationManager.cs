using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using REGLOGPROP_LEASNOTIF.Models;

namespace REGLOGPROP_LEASNOTIF.Managers
{
    public class NotificationManager
    {
        public void InsertNotification(string sendersId, string receiversId, string notificationDescription)
        {
            using (var context = new Context())
            {
                context.Database.ExecuteSqlRaw("EXEC InsertIntoNotificcation @p0, @p1, @p2",
                                                sendersId, receiversId, notificationDescription);
            }
        }

        public void InsertNotificationForOne(string sendersId, string receiversId, string notificationDescription)
        {
            using (var context = new Context())
            {
                context.Database.ExecuteSqlRaw("EXEC InsertIntoNotificcationForOne @p0, @p1",
                                                sendersId, notificationDescription);
            }
        }
         
        //public void ReadNotifications()
        //{
        //    using (var context = new Context())
        //    {
        //        var notifications = context.Set<Notification>().ToList();
        //        if (notifications.Count > 0)
        //        {
        //            Console.WriteLine("\t\tNotifications:");
        //            foreach (var notification in notifications)
        //            {
        //                Console.WriteLine("-----------------------------------");
        //                Console.WriteLine($"Notification Id: {notification.Notification_Id}");
        //                Console.WriteLine($"Sender Id: {notification.sendersId}");
        //                Console.WriteLine($"Receiver Id: {notification.receiversId}");
        //                Console.WriteLine($"Created Date: {notification.CreatedDate}");
        //                Console.WriteLine($"Description: {notification.notification_Descpirtion}");
        //                Console.WriteLine("-----------------------------------");
        //            }
        //        }
        //        else
        //        {
        //            Console.WriteLine("No notifications found.");
        //        }
        //    }
        //}

        public void ReadNotifications(string tid, string ownerId)
        {
            using (var context = new Context())
            {
                var notifications = context.Set<Notification>().Where(n => n.sendersId == tid || n.sendersId == ownerId || n.receiversId == tid || n.receiversId == ownerId).ToList();
                if (notifications.Count > 0)
                {
                    Console.WriteLine("\t\tNotifications:");
                    foreach (var notification in notifications)
                    {
                        Console.WriteLine("-----------------------------------");
                        Console.WriteLine($"Notification Id: {notification.Notification_Id}");
                        Console.WriteLine($"Sender Id: {notification.sendersId}");
                        Console.WriteLine($"Receiver Id: {notification.receiversId}");
                        Console.WriteLine($"Created Date: {notification.CreatedDate}");
                        Console.WriteLine($"Description: {notification.notification_Descpirtion}");
                        Console.WriteLine("-----------------------------------");
                    }
                }
                else
                {
                    Console.WriteLine("No notifications found.");
                }
            }
        }
    }
}
