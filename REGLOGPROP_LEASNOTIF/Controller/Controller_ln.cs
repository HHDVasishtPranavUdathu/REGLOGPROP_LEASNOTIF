using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using REGLOGPROP_LEASNOTIF.Models;

namespace REGLOGPROP_LEASNOTIF.Controller
{
    public class Controller_ln
    {
        public Context cc = new Context();

        // Lease-related methods
        public List<Lease> ReadData_Lease()
        {
            return cc.Leases.ToList();
        }

        public bool InsertData_Lease(Lease e)
        {
            cc.Leases.Add(e);
            cc.SaveChanges();
            return true;
        }

        public bool DeleteData_Lease(Lease e)
        {
            cc.Leases.Remove(e);
            cc.SaveChanges();
            return true;
        }

        // Notification-related methods
        public List<Notification> ReadData_Notification()
        {
            return cc.notifications.ToList();
        }

        public bool InsertData_Notification(Notification n)
        {
            cc.notifications.Add(n);
            cc.SaveChanges();
            return true;
        }

        public bool DeleteData_Notification(Notification n)
        {
            cc.notifications.Remove(n);
            cc.SaveChanges();
            return true;
        }

        // Registration-related methods
        public List<Registration> ReadData_Registration()
        {
            return cc.Registrations.ToList();
        }

        public bool InsertData_Registration(Registration r)
        {
            cc.Registrations.Add(r);
            cc.SaveChanges();
            return true;
        }

        public bool DeleteData_Registration(Registration r)
        {
            cc.Registrations.Remove(r);
            cc.SaveChanges();
            return true;
        }

        // Property-related methods
        public List<Prop> ReadData_Property()
        {
            return cc.Props.ToList();
        }

        public bool InsertData_Property(Prop p)
        {
            cc.Props.Add(p);
            cc.SaveChanges();
            return true;
        }

        public bool DeleteData_Property(Prop p)
        {
            cc.Props.Remove(p);
            cc.SaveChanges();
            return true;
        }

        public string GetOwnerIdByPropertyId(int? propertyId)
        {
            var property = cc.Props.FirstOrDefault(p => p.Property_Id == propertyId);
            return property?.Owner_Id;
        }
    }
}
