using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CommunityCertForT;
using CommunityCertForT.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Winter_Classes_App.EntityFramework;
using Winter_Classes_App.Models;

namespace Winter_Classes_App.Controllers
{

    public class BaseApiController : ControllerBase

    {
        //protected IConfiguration _configuration;
        protected AppSettings AppSettings { get; set; }
        protected readonly DataContext _context;


        public BaseApiController(DataContext context)

        {
           // _configuration = Configuration;
            _context = context;
           // AppSettings = _configuration.GetSection("AppSettings").Get<AppSettings>();
        }

        /*public enum PrivilegesLevel { NO_LOGGEDIN = 0, LOGGEDIN = 1, HR = 2, ADMIN = 3 };

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<PrivilegesLevel> CheckGroup()

        {
            if (!User.Identity.IsAuthenticated)
            {
                return PrivilegesLevel.NO_LOGGEDIN;
            }
            Session session = null;
            AADGraph graph = new AADGraph(AppSettings);
            if (Request.Cookies["session"] == null)
            {
                string[] groups = { "Admins", "HRs" };
                string groupName = null;
                foreach (string group in groups)
                {
                    string groupId = AppSettings.AADGroups.FirstOrDefault(g => System.String.Compare(g.Name, group) == 0).Id;
                    if (await graph.IsUserInGroup(User.Claims, groupId))
                    {
                        groupName = group;
                        continue;
                    }
                }
                if (groupName == null)
                {
                    groupName = "";
                }
                CookieOptions option = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true
                };
                string userEmail = ((ClaimsIdentity)User.Identity).FindFirst("Emails").Value;
                Session oldSession = _context.Session.Where(s => s.Email.Contains(userEmail)).FirstOrDefault();
                if (oldSession != null)
                {
                    _context.Remove(oldSession);
                }
                session = new Session
                {
                    Group = groupName,
                    Email = userEmail
                };
                _context.Session.Add(session);
                await _context.SaveChangesAsync();
                Response.Cookies.Append("session", session.Id.ToString(), option);
            } else
            {
                session = await _context.Session.FindAsync(Guid.Parse(Request.Cookies["session"]));
                if (session.Group == null)
                {
                    return PrivilegesLevel.NO_LOGGEDIN;
                }
                else if (session.Group == "" && User.Identity.IsAuthenticated)
                {
                    return PrivilegesLevel.LOGGEDIN;
                }

                string groupId = AppSettings.AADGroups.FirstOrDefault(g => System.String.Compare(g.Name, session.Group) == 0).Id;
                if(!await graph.IsUserInGroup(User.Claims, groupId))
                {
                    return PrivilegesLevel.NO_LOGGEDIN;
                }
            }

            if (User.Identity.IsAuthenticated)
            {
                switch (session.Group)
                {
                    case "Admins":
                        return PrivilegesLevel.ADMIN;
                    case "HRs":
                        return PrivilegesLevel.HR;
                    default:
                        return PrivilegesLevel.NO_LOGGEDIN;
                }
            } else
            {
                return PrivilegesLevel.NO_LOGGEDIN;
            }
        }*/
    }
}