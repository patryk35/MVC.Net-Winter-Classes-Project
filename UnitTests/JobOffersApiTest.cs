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
    public class JobOffersApiTest
    {
        [TestMethod]
        public void GetJobOffersTest()
        {

            IQueryable<Company> companies = new List<Company>
            {
                new Company
                {
                    Id = 0,
                    Name = "Google"
                }

            }.AsQueryable();

            // To query our database we need to implement IQueryable 
            var mockSet = new Mock<DbSet<Company>>();
            mockSet.As<IQueryable<Company>>().Setup(m => m.Provider).Returns(companies.Provider);
            mockSet.As<IQueryable<Company>>().Setup(m => m.Expression).Returns(companies.Expression);
            mockSet.As<IQueryable<Company>>().Setup(m => m.ElementType).Returns(companies.ElementType);
            mockSet.As<IQueryable<Company>>().Setup(m => m.GetEnumerator()).Returns(companies.GetEnumerator());

            // Arrange - We're mocking our dbSet & dbContext
            // in-memory data
            IQueryable<JobOffer> offers = new List<JobOffer>
            {
                new JobOffer
                {
                    Id = 1,
                    JobTitle = "Offer 1",
                    Description = "Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand " +
                    "Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand " +
                    "Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand ",
                    Location = "Wawa",
                    CompanyId = 0,
                    ValidUntil = new DateTime()
                },
                new JobOffer
                {
                    Id = 2,
                    JobTitle = "Offer 2",
                    Description = "Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand " +
                    "Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand " +
                    "Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand ",
                    Location = "Wawa",
                    CompanyId = 0,
                    ValidUntil = new DateTime()
                }

            }.AsQueryable();

            // To query our database we need to implement IQueryable 
            var mockSetOffer = new Mock<DbSet<JobOffer>>();
            mockSetOffer.As<IQueryable<JobOffer>>().Setup(m => m.Provider).Returns(offers.Provider);
            mockSetOffer.As<IQueryable<JobOffer>>().Setup(m => m.Expression).Returns(offers.Expression);
            mockSetOffer.As<IQueryable<JobOffer>>().Setup(m => m.ElementType).Returns(offers.ElementType);
            mockSetOffer.As<IQueryable<JobOffer>>().Setup(m => m.GetEnumerator()).Returns(offers.GetEnumerator());

            var mockContext = new Mock<DataContext>();
            mockContext.Setup(c => c.Companies).Returns(mockSet.Object);
            mockContext.Setup(c => c.JobOffers).Returns(mockSetOffer.Object);

            JobOffersApiController controller = new JobOffersApiController(mockContext.Object);
            Winter_Classes_App.Models.Paging.Models.PagingView actual = controller.GetJobOffers(null,0);

            var offers_list = actual.PagingModel.OrderBy(o => o.Id).ToList();
            Assert.AreEqual(2, offers.Count());
            JobOffer tmp = (JobOffer)offers_list[0];
            Assert.AreEqual("Offer 1",  tmp.JobTitle);
            tmp = (JobOffer)offers_list[1];
            Assert.AreEqual("Offer 2", tmp.JobTitle);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async System.Threading.Tasks.Task AddNullJobOfferTestAsync()
        {
            IQueryable<Company> companies = new List<Company>
            {
                new Company
                {
                    Id = 0,
                    Name = "Google"
                }

            }.AsQueryable();

            // To query our database we need to implement IQueryable 
            var mockSet = new Mock<DbSet<Company>>();
            mockSet.As<IQueryable<Company>>().Setup(m => m.Provider).Returns(companies.Provider);
            mockSet.As<IQueryable<Company>>().Setup(m => m.Expression).Returns(companies.Expression);
            mockSet.As<IQueryable<Company>>().Setup(m => m.ElementType).Returns(companies.ElementType);
            mockSet.As<IQueryable<Company>>().Setup(m => m.GetEnumerator()).Returns(companies.GetEnumerator());

            // Arrange - We're mocking our dbSet & dbContext
            // in-memory data
            IQueryable<JobOffer> offers = new List<JobOffer>
            {
                new JobOffer
                {
                    Id = 1,
                    JobTitle = "Offer 1",
                    Description = "Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand " +
                    "Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand " +
                    "Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand ",
                    Location = "Wawa",
                    CompanyId = 0,
                    ValidUntil = new DateTime()
                },
                new JobOffer
                {
                    Id = 2,
                    JobTitle = "Offer 2",
                    Description = "Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand " +
                    "Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand " +
                    "Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand ",
                    Location = "Wawa",
                    CompanyId = 0,
                    ValidUntil = new DateTime()
                }

            }.AsQueryable();

            // To query our database we need to implement IQueryable 
            var mockSetOffer = new Mock<DbSet<JobOffer>>();
            mockSetOffer.As<IQueryable<JobOffer>>().Setup(m => m.Provider).Returns(offers.Provider);
            mockSetOffer.As<IQueryable<JobOffer>>().Setup(m => m.Expression).Returns(offers.Expression);
            mockSetOffer.As<IQueryable<JobOffer>>().Setup(m => m.ElementType).Returns(offers.ElementType);
            mockSetOffer.As<IQueryable<JobOffer>>().Setup(m => m.GetEnumerator()).Returns(offers.GetEnumerator());

            var mockContext = new Mock<DataContext>();
            mockContext.Setup(c => c.Companies).Returns(mockSet.Object);
            mockContext.Setup(c => c.JobOffers).Returns(mockSetOffer.Object);

            JobOffersApiController controller = new JobOffersApiController(mockContext.Object);
            await controller.PostOffer(null);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task EditJobOfferTest()
        {
            IQueryable<Company> companies = new List<Company>
            {
                new Company
                {
                    Id = 0,
                    Name = "Google"
                }

            }.AsQueryable();

            // To query our database we need to implement IQueryable 
            var mockSet = new Mock<DbSet<Company>>();
            mockSet.As<IQueryable<Company>>().Setup(m => m.Provider).Returns(companies.Provider);
            mockSet.As<IQueryable<Company>>().Setup(m => m.Expression).Returns(companies.Expression);
            mockSet.As<IQueryable<Company>>().Setup(m => m.ElementType).Returns(companies.ElementType);
            mockSet.As<IQueryable<Company>>().Setup(m => m.GetEnumerator()).Returns(companies.GetEnumerator());

            // Arrange - We're mocking our dbSet & dbContext
            // in-memory data
            IQueryable<JobOffer> offers = new List<JobOffer>
            {
                new JobOffer
                {
                    Id = 1,
                    JobTitle = "Offer 1",
                    Description = "Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand " +
                    "Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand " +
                    "Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand ",
                    Location = "Wawa",
                    CompanyId = 0,
                    ValidUntil = new DateTime()
                },
                new JobOffer
                {
                    Id = 2,
                    JobTitle = "Offer 2",
                    Description = "Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand " +
                    "Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand " +
                    "Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand Rand ",
                    Location = "Wawa",
                    CompanyId = 0,
                    ValidUntil = new DateTime()
                }

            }.AsQueryable();

            // To query our database we need to implement IQueryable 
            var mockSetOffer = new Mock<DbSet<JobOffer>>();
            mockSetOffer.As<IQueryable<JobOffer>>().Setup(m => m.Provider).Returns(offers.Provider);
            mockSetOffer.As<IQueryable<JobOffer>>().Setup(m => m.Expression).Returns(offers.Expression);
            mockSetOffer.As<IQueryable<JobOffer>>().Setup(m => m.ElementType).Returns(offers.ElementType);
            mockSetOffer.As<IQueryable<JobOffer>>().Setup(m => m.GetEnumerator()).Returns(offers.GetEnumerator());

            var mockContext = new Mock<DataContext>();
            mockContext.Setup(c => c.Companies).Returns(mockSet.Object);
            mockContext.Setup(c => c.JobOffers).Returns(mockSetOffer.Object);

            JobOffersApiController controller = new JobOffersApiController(mockContext.Object);
            JobOffer cc = await controller.GetJobOfferById(1);
            cc.JobTitle = "New Offer";
            var res = controller.Edit(cc);


            Winter_Classes_App.Models.Paging.Models.PagingView actual = controller.GetJobOffers("", 0);

            var offers_list = actual.PagingModel.OrderBy(o => o.Id).ToList();
            Assert.AreEqual(2, offers.Count());
            JobOffer tmp = (JobOffer)offers_list[1];
            Assert.AreEqual("Offer 2", tmp.JobTitle);
            tmp = (JobOffer)offers_list[0];
            Assert.AreEqual("New Offer", tmp.JobTitle);
        }
    }
}
