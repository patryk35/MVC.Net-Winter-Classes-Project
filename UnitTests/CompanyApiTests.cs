using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;

using Winter_Classes_App.Models;
using Winter_Classes_App.EntityFramework;
using Winter_Classes_App.Controllers.Api;
using System;

namespace UnitTestProject3
{
    [TestClass]
    public class CompanyApiTests
    {
        [TestMethod]
        public void GetCompaniesTest()
        {
            // Arrange - We're mocking our dbSet & dbContext
            // in-memory data
            IQueryable<Company> companies = new List<Company>
            {
                new Company
                {
                    Id = 1,
                    Name = "Google"
                },
                new Company
                {
                    Id = 2,
                    Name = "Microsoft"
                }

            }.AsQueryable();

            // To query our database we need to implement IQueryable 
            var mockSet = new Mock<DbSet<Company>>();
            mockSet.As<IQueryable<Company>>().Setup(m => m.Provider).Returns(companies.Provider);
            mockSet.As<IQueryable<Company>>().Setup(m => m.Expression).Returns(companies.Expression);
            mockSet.As<IQueryable<Company>>().Setup(m => m.ElementType).Returns(companies.ElementType);
            mockSet.As<IQueryable<Company>>().Setup(m => m.GetEnumerator()).Returns(companies.GetEnumerator());

            var mockContext = new Mock<DataContext>();
            mockContext.Setup(c => c.Companies).Returns(mockSet.Object);

            CompanyApiController controller = new CompanyApiController(mockContext.Object);
            System.Threading.Tasks.Task<Winter_Classes_App.Models.Paging.Models.PagingView> actual = controller.GetCompaniesAsync("",0);

            var companies_list = actual.Result.PagingModel.OrderBy(o => o.Id).ToList();
            Assert.AreEqual(2, companies.Count());
            Company tmp = (Company)companies_list[1];
            Assert.AreEqual("Microsoft",  tmp.Name);
            tmp = (Company)companies_list[0];
            Assert.AreEqual("Google", tmp.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async System.Threading.Tasks.Task AddNullCompanyTestAsync()
        {
            // Arrange - We're mocking our dbSet & dbContext
            // in-memory data
            IQueryable<Company> companies = new List<Company>
            {
                new Company
                {
                    Id = 1,
                    Name = "Google"
                },
                new Company
                {
                    Id = 2,
                    Name = "Microsoft"
                }

            }.AsQueryable();

            // To query our database we need to implement IQueryable 
            var mockSet = new Mock<DbSet<Company>>();
            mockSet.As<IQueryable<Company>>().Setup(m => m.Provider).Returns(companies.Provider);
            mockSet.As<IQueryable<Company>>().Setup(m => m.Expression).Returns(companies.Expression);
            mockSet.As<IQueryable<Company>>().Setup(m => m.ElementType).Returns(companies.ElementType);
            mockSet.As<IQueryable<Company>>().Setup(m => m.GetEnumerator()).Returns(companies.GetEnumerator());

            var mockContext = new Mock<DataContext>();
            mockContext.Setup(c => c.Companies).Returns(mockSet.Object);

            CompanyApiController controller = new CompanyApiController(mockContext.Object);
            await controller.PostCompany(null);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task EditCompanyTest()
        {
            // Arrange - We're mocking our dbSet & dbContext
            // in-memory data
            IQueryable<Company> companies = new List<Company>
            {
                new Company
                {
                    Id = 1,
                    Name = "Google"
                },
                new Company
                {
                    Id = 2,
                    Name = "Microsoft"
                }

            }.AsQueryable();

            // To query our database we need to implement IQueryable 
            var mockSet = new Mock<DbSet<Company>>();
            mockSet.As<IQueryable<Company>>().Setup(m => m.Provider).Returns(companies.Provider);
            mockSet.As<IQueryable<Company>>().Setup(m => m.Expression).Returns(companies.Expression);
            mockSet.As<IQueryable<Company>>().Setup(m => m.ElementType).Returns(companies.ElementType);
            mockSet.As<IQueryable<Company>>().Setup(m => m.GetEnumerator()).Returns(companies.GetEnumerator());

            var mockContext = new Mock<DataContext>();
            mockContext.Setup(c => c.Companies).Returns(mockSet.Object);

            CompanyApiController controller = new CompanyApiController(mockContext.Object);
            Company cc = await controller.GetCompanysById(1);
            cc.Name = "NewCorpIndustry";
            var res = controller.Edit(cc.Id, cc);


            System.Threading.Tasks.Task<Winter_Classes_App.Models.Paging.Models.PagingView> actual = controller.GetCompaniesAsync("", 0);

            var companies_list = actual.Result.PagingModel.OrderBy(o => o.Id).ToList();
            Assert.AreEqual(2, companies.Count());
            Company tmp = (Company)companies_list[1];
            Assert.AreEqual("Microsoft", tmp.Name);
            tmp = (Company)companies_list[0];
            Assert.AreEqual("NewCorpIndustry", tmp.Name);
        }
    }
}
