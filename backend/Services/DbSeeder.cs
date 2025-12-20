using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PreClear.Api.Data;
using PreClear.Api.Models;

namespace PreClear.Api.Services
{
    public static class DbSeeder
    {
        public static void Seed(PreclearDbContext db)
        {
            try
            {
                // Verify database connection
                var canConnect = db.Database.CanConnect();
                if (!canConnect)
                {
                    System.Console.WriteLine("DbSeeder: Cannot connect to database");
                    return;
                }

                // Try to fix the database schema - clear incompatible data
                try
                {
                    // Commented out: Delete any shipments with NULL preclear_token first
                    // db.Database.ExecuteSqlRaw("DELETE FROM preclear.shipments WHERE preclear_token IS NULL OR preclear_token = '';");
                    // This was causing user-added shipments to be deleted on restart
                }
                catch (Exception ex)
                {
                    // Table might not exist yet, continue
                    System.Console.WriteLine($"DbSeeder: Could not clean shipments table: {ex.Message}");
                }

                // Safe check - only seed if demo user doesn't exist or profiles don't exist
                try
                {
                    var shipperExists = db.Users.Any(u => u.Email == "shipper@demo.com");
                    var brokerExists = db.Users.Any(u => u.Email == "broker@demo.com");
                    var shipperProfileExists = db.ShipperProfiles.Any();
                    var brokerProfileExists = db.BrokerProfiles.Any();

                    if (shipperExists && brokerExists && shipperProfileExists && brokerProfileExists)
                    {
                        System.Console.WriteLine("DbSeeder: Demo users and profiles already seeded, skipping");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine($"DbSeeder: Error checking existing users/profiles: {ex.Message}");
                    return;
                }

                db.Users.AddRange(
                    // Seed users with PBKDF2-SHA256 hashes (AuthService-compatible)
                    // Password: Shipper@123
                    new User {
                        FirstName = "Test",
                        LastName = "Shipper",
                        Email = "shipper@demo.com",
                        PasswordHash = "zSnH6rOY9NVho++vILLPqQ==:VDzgreYUcCIaLc2xFaiuMIU6GrjRYmIWFSOwgM1iSNw=",
                        Role = "shipper",
                        Phone = "+91-9876543210",
                        Company = "Test Exports Pvt Ltd",
                        TosAccepted = true,
                        TosAcceptedAt = DateTime.UtcNow,
                        EmailVerified = true,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    // Password: Broker@123
                    new User {
                        FirstName = "Demo",
                        LastName = "Broker",
                        Email = "broker@demo.com",
                        PasswordHash = "pFUOGIPZsowAVCRpDZNU+Q==:vllhDU76MMdeL+/AXD78pbK7pMzYHOlyEMWQ+WqUwgo=",
                        Role = "broker",
                        Phone = "+1-415-555-0199",
                        Company = "Global Customs Brokers LLC",
                        TosAccepted = true,
                        TosAcceptedAt = DateTime.UtcNow,
                        EmailVerified = true,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    // Password: Admin@123
                    new User {
                        FirstName = "System",
                        LastName = "Admin",
                        Email = "admin@demo.com",
                        PasswordHash = "bH2cUZllWeb6ktdihpygTQ==:v/RMYQyPbeVSqOzO4jBYigteEuPUvQzycOprO96ga1Q=",
                        Role = "admin",
                        Phone = "+1-800-742-5877",
                        Company = "UPS Operations",
                        TosAccepted = true,
                        TosAcceptedAt = DateTime.UtcNow,
                        EmailVerified = true,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    }
                );

                db.SaveChanges();

                // Create profiles for the seeded users
                var shipperUser = db.Users.FirstOrDefault(u => u.Email == "shipper@demo.com");
                var brokerUser = db.Users.FirstOrDefault(u => u.Email == "broker@demo.com");

                if (shipperUser != null)
                {
                    db.ShipperProfiles.Add(new ShipperProfile
                    {
                        UserId = shipperUser.Id,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    });
                }

                if (brokerUser != null)
                {
                    db.BrokerProfiles.Add(new BrokerProfile
                    {
                        UserId = brokerUser.Id,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    });
                }

                db.SaveChanges();
                System.Console.WriteLine("DbSeeder: Successfully seeded demo users and profiles");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"DbSeeder: FATAL ERROR - {ex.GetType().Name}: {ex.Message}");
                System.Console.WriteLine($"DbSeeder: Stack trace: {ex.StackTrace}");
                // Don't rethrow - let the application continue
            }
        }
    }
}
