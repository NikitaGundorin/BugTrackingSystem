using System;
using System.Linq;
using System.Threading.Tasks;
using BugTrackingSystem.Models.Repositories;
using BugTrackingSystem.ViewModels;

namespace BugTrackingSystem.Models
{
    public static class InitialData
    {
        public static void Initialize(BugTrackingSystemContext context, AccountRepository accountRepository)
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
                RegisterModel registerModel = new RegisterModel()
                {
                    UserName = "admin",
                    Email = "admin@gmail.com",
                    Password = "Admin",
                    ConfirmPassword = "Admin"
                };

                 accountRepository.Register(registerModel);
            }

            context.SaveChanges();
        }
    }
}