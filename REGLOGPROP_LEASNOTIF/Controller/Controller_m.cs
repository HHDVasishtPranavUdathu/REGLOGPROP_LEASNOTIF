using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using REGLOGPROP_LEASNOTIF.Models;

namespace REGLOGPROP_LEASNOTIF.Controller
{
    public class Controller_m
    {
        private readonly Context _context;

        public Controller_m()
        {
            _context = new Context();
        }

        // Method to insert a new maintainance request
        public void InsertMaintainance(int propertyId, string tenantId, string description, string status, string imagePath)
        {
            var maintainance = new Maintainance
            {
                PropertyId = propertyId,
                TenantId = tenantId,
                Description = description,
                Status = status,
                ImagePath = imagePath
            };

            _context.Maintainances.Add(maintainance);
            _context.SaveChanges();
        }


        public List<Maintainance> ViewMaintainances()
        {
            return _context.Maintainances.ToList();
        }


        public string GetStatusByRequestId(int requestId)
        {
            string status = null;
            using (var connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using (var command = new SqlCommand("GetStatusByRequestId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@RequestId", requestId));

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            status = reader["Status"].ToString();
                        }
                    }
                }
            }
            return status;
        }


        public bool UpdateStatus(int requestId, string newStatus)
        {
            var maintainance = _context.Maintainances.Find(requestId);
            if (maintainance != null)
            {
                maintainance.Status = newStatus;
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Maintainance> GetMaintainancesByTenantId(int tenantId)
        {
            var maintainances = new List<Maintainance>();
            using (var connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using (var command = new SqlCommand("GetMaintainancesByTenantId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@TenantId", tenantId));

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var maintainance = new Maintainance
                            {
                                RequestId = (int)reader["RequestId"],
                                PropertyId = (int)reader["PropertyId"],
                                TenantId = (string)reader["TenantId"],
                                Description = reader["Description"].ToString(),
                                Status = reader["Status"].ToString(),
                                ImagePath = reader["ImagePath"].ToString()
                            };
                            maintainances.Add(maintainance);
                        }
                    }
                }
            }
            return maintainances;
        }

    }
}
