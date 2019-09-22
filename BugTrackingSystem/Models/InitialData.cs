using System;
using System.Linq;

namespace BugTrackingSystem.Models
{
    public static class InitialData
    {
        public static void Initialize(BugTrackingSystemContext context)
        {
            if (!context.Importances.Any())
            {
                context.Importances.AddRange(
                    new Importance
                    {
                        Name = "Critical"
                    },
                    new Importance
                    {
                        Name = "Important"
                    },
                    new Importance
                    {
                        Name = "Minor"
                    },
                    new Importance
                    {
                        Name = "ChangeRequest"
                    }
                );
            }

            if (!context.Priorities.Any())
            {
                context.Priorities.AddRange(
                    new Priority
                    {
                        Name = "VeryHigh"
                    },
                    new Priority
                    {
                        Name = "High"
                    },
                    new Priority
                    {
                        Name = "Middle"
                    },
                    new Priority
                    {
                        Name = "Low"
                    }
                );
            }

            if (!context.Statuses.Any())
            {
                context.Statuses.AddRange(
                    new Status
                    {
                        Name = "Created"
                    },
                    new Status
                    {
                        Name = "Opened"
                    },
                    new Status
                    {
                        Name = "Solved"
                    },
                    new Status
                    {
                        Name = "Closed"
                    }
                );
            }

            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new User
                    {
                        Email = "admin@gmail.com",
                        UserName = "admin",
                        Password = "Admin",
                        Role = "admin"
                    }
                );
            }

            context.SaveChanges();
        }
    }
}