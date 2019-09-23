using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BugTrackingSystem.ViewModels;

namespace BugTrackingSystem.Models.Repositories
{
    public class BugRepository
    {
        private readonly BugTrackingSystemContext db;

        public BugRepository(BugTrackingSystemContext db)
        {
            this.db = db;
        }


        public async Task<Bug> GetBugAsync(int id)
        {
            Bug bug = await db.Bugs.FirstOrDefaultAsync(b => b.Id == id);

            if (bug is null)
            {
                return null;
            }

            return bug;
        }

        public async Task<BugViewModel> GetFullBugAsync(int bugId)
        {
            Bug foundBug = await GetBugAsync(bugId);

            if (foundBug is null)
            {
                return null;
            }

            List<BugChangelogViewModel> bugChangelog = await GetBugChangelogs(bugId);

            BugViewModel bug = new BugViewModel()
            {
                Id = foundBug.Id,
                CreationDate = foundBug.CreationDate.ToString("dd.MM.yyy hh:mm"),
                ShortDescription = foundBug.ShortDescription,
                FullDescription = foundBug.FullDescription,
                Importance = db.Importances.FirstOrDefault(i => i.Id == foundBug.ImportanceId).Name,
                Priority = db.Priorities.FirstOrDefault(p => p.Id == foundBug.PriorityId).Name,
                Status = db.Statuses.FirstOrDefault(s => s.Id == foundBug.StatusId).Name,
                UserName = db.Users.FirstOrDefault(u => u.Id == foundBug.UserId).UserName,
                BugChangeLog = bugChangelog
            };

            return bug;
        }

        public List<Bug> GetBugsList()
        {
            List<Bug> bugsList = new List<Bug>();

            foreach (Bug bug in db.Bugs)
            {
                bugsList.Add(bug);
            }

            return bugsList;
        }

        public async Task<Bug> CreateBug(CreateBugViewModel bug, User user)
        {
            Bug newBug = new Bug()
            {
                CreationDate = DateTime.Now,
                ShortDescription = bug.ShortDescription,
                FullDescription = bug.FullDescription,
                Importance = await db.Importances.FirstOrDefaultAsync(i => i.Id == bug.ImportanceId),
                ImportanceId = bug.ImportanceId,
                Priority = await db.Priorities.FirstOrDefaultAsync(p => p.Id == bug.PriorityId),
                PriorityId = bug.PriorityId,
                Status = await db.Statuses.FirstOrDefaultAsync(p => p.Id == 1),
                StatusId = 1,
                User = user,
                UserId = user.Id
            };


            await db.Bugs.AddAsync(newBug);
            await db.SaveChangesAsync();

            BugChangelog bugChangelog = new BugChangelog()
            {
                BugId = newBug.Id,
                Bug = newBug,
                Date = newBug.CreationDate,
                StatusId = 1,
                Comment = "Bug created.",
                UserId = newBug.UserId
            };

            await db.BugChangelogs.AddAsync(bugChangelog);
            await db.SaveChangesAsync();

            return newBug;
        }

        public async Task<Bug> UpdateBugStatusAsync(int id, int newStatusId, string comment, int userId)
        {
            Bug bug = await db.Bugs.FirstOrDefaultAsync(b => b.Id == id);

            if (bug is null)
            {
                return null;
            }

            if (bug.StatusId == 3)
            {
                switch (newStatusId)
                {
                    case 2:
                        bug.Status = await db.Statuses.FirstOrDefaultAsync(s => s.Id == 2);
                        bug.StatusId = 2;
                        break;
                    case 4:
                        bug.Status = await db.Statuses.FirstOrDefaultAsync(s => s.Id == 4);
                        bug.StatusId = 4;
                        break;
                }
            }
            else
            {
                switch (bug.StatusId)
                {
                    case 1:
                        bug.Status = await db.Statuses.FirstOrDefaultAsync(s => s.Id == 2);
                        bug.StatusId = 2;
                        break;
                    case 2:
                        bug.Status = await db.Statuses.FirstOrDefaultAsync(s => s.Id == 3);
                        bug.StatusId = 3;
                        break;
                }
            }
            db.Bugs.Update(bug);

            BugChangelog bugChangelog = new BugChangelog()
            {
                BugId = bug.Id,
                Bug = bug,
                Date = DateTime.Now,
                StatusId = newStatusId,
                Comment = comment,
                UserId = userId
            };

            await db.BugChangelogs.AddAsync(bugChangelog);

            await db.SaveChangesAsync();
            return bug;
        }

        public async Task<List<BugChangelogViewModel>> GetBugChangelogs(int bugId)
        {
            List<BugChangelog> bugChangelogs = await db.BugChangelogs.Where(bug => bug.BugId == bugId).ToListAsync();

            List<BugChangelogViewModel> result = new List<BugChangelogViewModel>();

            foreach (BugChangelog b in bugChangelogs)
            {
                BugChangelogViewModel item = new BugChangelogViewModel()
                {
                    BugId = b.BugId,
                    Date = b.Date.ToString("dd.MM.yyy hh:mm"),
                    UserName = db.Users.FirstOrDefault(u => u.Id == b.UserId).UserName,
                    Comment = b.Comment,
                    NewStatus = db.Statuses.FirstOrDefault(s => s.Id == b.StatusId).Name
                };
                result.Add(item);
            }

            return result;
        }
    }
}
