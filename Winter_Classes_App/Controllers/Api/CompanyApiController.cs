using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Winter_Classes_App.EntityFramework;
using Winter_Classes_App.Models;
using Winter_Classes_App.Models.Paging.Models;

namespace Winter_Classes_App.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyApiController : ControllerBase
    {
        private readonly DataContext _context;

        public CompanyApiController(DataContext context)
        {
            _context = context;

        }

        [HttpGet]
        public async Task<PagingView> GetCompaniesAsync([FromQuery(Name = "searchString")]string searchString, [FromQuery(Name = "pageNo")]int pageNo = 1)
        {
            /*PrivilegesLevel privilegesLevel = await CheckGroup();
            if (privilegesLevel < PrivilegesLevel.HR)
            {
                HttpContext.Response.StatusCode = 401; //unauthorized
                return null;
            }*/

            int totalPage, totalRecord, pageSize;
            pageSize = 8;


            List<Company> record = null;
            if (searchString == null)
            {
                totalRecord = _context.Companies.Count();
                totalPage = (totalRecord / pageSize) + ((totalRecord % pageSize) > 0 ? 1 : 0);
                record = (from j in _context.Companies select j).OrderBy(j => j.Name).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                totalRecord = _context.Companies.Where(s => s.Name.Contains(searchString)).Count();

                totalPage = (totalRecord / pageSize) + ((totalRecord % pageSize) > 0 ? 1 : 0);
                record = _context.Companies
                                .Where(s => s.Name.Contains(searchString))
                                .OrderByDescending(j => j.Name)
                                .Skip((pageNo - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();
            }

            PagingView empData = new PagingView
            {
                PagingModel = record,
                TotalPage = totalPage
            };

            return empData;
        }

        [HttpGet("{id}")]
        public Task<Company> GetCompanysById(int id)
        {
            /*PrivilegesLevel privilegesLevel = await CheckGroup();
            ViewBag.PrivilegesLevel = (int)privilegesLevel;
            if (privilegesLevel == PrivilegesLevel.NO_LOGGEDIN)
            {
                return NotFound();
            }*/

            var company = Task.FromResult(_context.Companies
                .FirstOrDefault(m => m.Id == id));
            if (company == null)
            {
                HttpContext.Response.StatusCode = 400; //not found
                return null;
            }

            return company;
        }


        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> PostCompany([FromForm]Company model)
        {
            /*PrivilegesLevel privilegesLevel = await CheckGroup();
            if (privilegesLevel < PrivilegesLevel.HR)
            {
                return Unauthorized();
            }*/
            if (model == null)
            {
                throw new ArgumentNullException("Company can not be null");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Add(model);
            await _context.SaveChangesAsync();
            return CreatedAtAction("Index", "CompaniesController", null, null);
        }

        [HttpPost("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Company company)
        {
            /*PrivilegesLevel privilegesLevel = await CheckGroup();
            if (privilegesLevel < PrivilegesLevel.ADMIN)
            {
                return Unauthorized();
            }*/

            if( company == null)
            {
                throw new NullReferenceException("Company can not be null");
            }

            if (id != company.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(company);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyExists(company.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw new Exception("Company not exists");
                    }
                }
                return CreatedAtAction("Index", "CompaniesController", null, null);
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            /*PrivilegesLevel privilegesLevel = await CheckGroup();
            if (privilegesLevel < PrivilegesLevel.ADMIN)
            {
                return NotFound();
            }*/
            var company = await _context.Companies.FindAsync(id);
            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
            return CreatedAtAction("Index", "CompaniesController", null, null);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private bool CompanyExists(int id)
        {
            return _context.Companies.Any(e => e.Id == id);
        }
    }
}
