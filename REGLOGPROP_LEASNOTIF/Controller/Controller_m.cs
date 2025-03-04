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

        public async Task InsertMaintenanceAsync(int propertyId, string tenantId, string description, string status, string imagePath)
        {
            var maintenance = new Maintenance
            {
                PropertyId = propertyId,
                TenantId = tenantId,
                Description = description,
                Status = status,
                ImagePath = imagePath
            };

            _context.Maintainances.Add(maintenance);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Maintenance>> ViewMaintenancesAsync()
        {
            return await _context.Maintainances.ToListAsync();
        }

        public async Task<string> GetStatusByRequestIdAsync(int requestId)
        {
            string status = null;
            using (var connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using (var command = new SqlCommand("GetStatusByRequestId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@RequestId", requestId));

                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            status = reader["Status"].ToString();
                        }
                    }
                }
            }
            return status;
        }

        public async Task<bool> UpdateStatusAsync(int requestId, string newStatus)
        {
            var maintenance = await _context.Maintainances.FindAsync(requestId);
            if (maintenance != null)
            {
                maintenance.Status = newStatus;
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<Maintenance>> GetMaintenancesByTenantIdAsync(string tenantId)
        {
            var maintenances = new List<Maintenance>();
            using (var connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using (var command = new SqlCommand("GetMaintenancesByTenantId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@TenantId", tenantId));

                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var maintenance = new Maintenance
                            {
                                RequestId = (int)reader["RequestId"],
                                PropertyId = (int)reader["PropertyId"],
                                TenantId = (string)reader["TenantId"],
                                Description = reader["Description"].ToString(),
                                Status = reader["Status"].ToString(),
                                ImagePath = reader["ImagePath"].ToString()
                            };
                            maintenances.Add(maintenance);
                        }
                    }
                }
            }
            return maintenances;
        }

        public async Task<(int PropertyId, string TenantId)?> GetPropertyAndTenantIdFromLeaseAsync(int leaseId)
        {
            using (var connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using (var command = new SqlCommand("GetPropertyAndTenantIdFromLease", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@LeaseId", leaseId));

                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            int propertyId = (int)reader["PropertyId"];
                            string tenantId = reader["TenantId"].ToString();
                            return (propertyId, tenantId);
                        }
                    }
                }
            }
            return null;
        }
    }
}
