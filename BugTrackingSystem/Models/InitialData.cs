using System.Linq;
using System.Threading.Tasks;
using BugTrackingSystem.Models.Repositories;
using BugTrackingSystem.ViewModels;

namespace BugTrackingSystem.Models
{
    public static class InitialData
    {
        public static async Task InitializeAsync(BugTrackingSystemContext context, AccountRepository accountRepository)
        {
            if (!context.Importances.Any())
            {
                await context.Importances.AddRangeAsync(
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
                        Name = "Change Request"
                    }
                );
            }

            if (!context.Priorities.Any())
            {
                await context.Priorities.AddRangeAsync(
                    new Priority
                    {
                        Name = "Very High"
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
                await context.Statuses.AddRangeAsync(
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

                await accountRepository.RegisterAsync(registerModel);
            }

            await context.SaveChangesAsync();
        }
    }
}