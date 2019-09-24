﻿using System;
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

        public List<List<object>> GetParameters()
        {
            List<List<object>> parameters = new List<List<object>>();
            List<object> importance = new List<object>();
            foreach (Importance i in db.Importances)
            {
                importance.Add(i);
            }
            parameters.Add(importance);

            List<object> priority = new List<object>();
            foreach (Priority p in db.Priorities)
            {
                priority.Add(p);
            }
            parameters.Add(priority);

            return parameters;
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

        public async Task<IndexViewModel> GetBugsListAsync(string sortOrder, int page, int pageSize)
        {
            List<BugPreviewViewModel> bugs = await db.Bugs.Select(b => new BugPreviewViewModel
            {
                Id = b.Id,
                CreationDate = b.CreationDate.ToString("dd.MM.yyy hh:mm"),
                CreationDateTime = b.CreationDate,
                ShortDescription = b.ShortDescription,
                Importance = b.Importance.Name,
                ImportanceId = b.ImportanceId,
                Priority = b.Priority.Name,
                PriorityId = b.PriorityId,
                Status = b.Status.Name,
                StatusId = b.StatusId,
                UserName = b.User.UserName
            }).ToListAsync();

            IEnumerable<BugPreviewViewModel> result = bugs.OrderBy(b => b.Id);

            switch (sortOrder)
            {
                case "IdAsc":
                    break;
                case "IdDesc":
                    result = bugs.OrderByDescending(b => b.Id);
                    break;
                case "CreationDateAsc":
                    result = bugs.OrderBy(b => b.CreationDateTime);
                    break;
                case "CreationDateDesc":
                    result = bugs.OrderByDescending(b => b.CreationDateTime);
                    break;
                case "ShortDescriptionAsc":
                    result = bugs.OrderBy(b => b.ShortDescription);
                    break;
                case "ShortDescriptionDesc":
                    result = bugs.OrderByDescending(b => b.ShortDescription);
                    break;
                case "UserNameAsc":
                    result = bugs.OrderBy(b => b.UserName);
                    break;
                case "UserNameDesc":
                    result = bugs.OrderByDescending(b => b.UserName);
                    break;
                case "StatusAsc":
                    result = bugs.OrderBy(b => b.StatusId);
                    break;
                case "StatusDesc":
                    result = bugs.OrderByDescending(b => b.StatusId);
                    break;
                case "PriorityAsc":
                    result = bugs.OrderByDescending(b => b.PriorityId);
                    break;
                case "PriorityDesc":
                    result = bugs.OrderBy(b => b.PriorityId);
                    break;
                case "ImportanceAsc":
                    result = bugs.OrderByDescending(b => b.ImportanceId);
                    break;
                case "ImportanceDesc":
                    result = bugs.OrderBy(b => b.ImportanceId);
                    break;
            }

            var count = result.Count();
            var resultBugs = result.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            IndexViewModel viewModel = new IndexViewModel
            {
                Pages = new PageViewModel(count, page, pageSize),
                Bugs = resultBugs
            };

            return viewModel;
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
