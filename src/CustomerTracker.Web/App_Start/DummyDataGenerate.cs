using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using CsvHelper;
using CustomerTracker.Web.Models.Entities;
using CustomerTracker.Web.Models.Enums;
using CustomerTracker.Web.Utilities;
using Ploeh.AutoFixture;

namespace CustomerTracker.Web.App_Start
{
    public class Person
    {
        public string Gender { get; set; }

        public string TelephoneNumber { get; set; }

        public string GivenName { get; set; }

        public string Surname { get; set; }

        public string EmailAddress { get; set; }

        public string Company { get; set; }

    }
    public static class DummyDataGenerate
    { 
        public static void Generate()
        {
            Fixture fixture = ConfigureFixture();

            var repositoryDepartment = ConfigurationHelper.UnitOfWorkInstance.GetRepository<Department>();
            repositoryDepartment.Create(new Department() { IsActive = true, IsDeleted = false, Name = "Muhasebeci" });
            repositoryDepartment.Create(new Department() { IsActive = true, IsDeleted = false, Name = "Güvenlik Görevlisi" });
            

            var repositoryProduct = ConfigurationHelper.UnitOfWorkInstance.GetRepository<Product>();
            var productLibrid = new Product() { IsActive = true, IsDeleted = false, Name = "Librid" };
            productLibrid.SubProducts = new Collection<Product>();
            productLibrid.SubProducts.Add(new Product() { IsActive = true, IsDeleted = false, Name = "Sirkülasyon Masaüstü" });
            productLibrid.SubProducts.Add(new Product() { IsActive = true, IsDeleted = false, Name = "Selfcheck Kiosk" });
            productLibrid.SubProducts.Add(new Product() { IsActive = true, IsDeleted = false, Name = "Otomasyon Web" });
            productLibrid.SubProducts.Add(new Product() { IsActive = true, IsDeleted = false, Name = "Opac Web" });
            repositoryProduct.Create(productLibrid);
          
            var repositoryCity = ConfigurationHelper.UnitOfWorkInstance.GetRepository<City>();
            repositoryCity.Create(new City() { IsActive = true, IsDeleted = false, Name = "Adana", Code = "01" });

            ConfigurationHelper.UnitOfWorkInstance.Save();


            var repositoryCustomer = ConfigurationHelper.UnitOfWorkInstance.GetRepository<Customer>();
            for (int i = 0; i < 100; i++)
            {
                var customer = fixture.Create<Customer>();
                repositoryCustomer.Create(customer);
            }

            var repositoryRole = ConfigurationHelper.UnitOfWorkInstance.GetRepository<Role>();
            repositoryRole.Create(new Role() { IsActive = true, IsDeleted = false, RoleName = ConfigurationHelper.RoleAdmin });
            repositoryRole.Create(new Role() { IsActive = true, IsDeleted = false, RoleName = ConfigurationHelper.RolePersonel });
            repositoryRole.Create(new Role() { IsActive = true, IsDeleted = false, RoleName = ConfigurationHelper.RoleCustomer });
             
        }
         
        private static Fixture ConfigureFixture()
        {
            var path = @AppDomain.CurrentDomain.BaseDirectory + "persons.csv";
            var csv = new CsvReader(File.OpenText(path));
            var persons = csv.GetRecords<Person>().ToList();

           
            var fixture = new Fixture();
            fixture.Register<RemoteMachine>(() =>
            {
                var random = new Random();
                var remoteMachine = new RemoteMachine()
                {
                    Name = "TeamViewer",
                    Password = "123",
                    RemoteAddress = "192.180.10.1",
                    RemoteConnectionTypeId = ((RemoteConnectionType)random.Next(1, 5)).GetHashCode(),
                    Explanation = Guid.NewGuid().ToString().Substring(0, 20) + " " + Guid.NewGuid().ToString().Substring(0, 20),
                    Username = Guid.NewGuid().ToString().Substring(0, 5),
                    IsActive = true,
                    IsDeleted = false,
                };
                return remoteMachine;
            });

            int communicationCount = 0;
            fixture.Register<Communication>(() =>
            {
                var random = new Random();
                var communication = new Communication()
                {
                    DepartmentId = random.Next(1, 2),
                    FirstName = persons[communicationCount].GivenName,
                    LastName = persons[communicationCount].Surname,
                    Email = persons[communicationCount].EmailAddress,
                    PhoneNumber = persons[communicationCount].TelephoneNumber,
                    GenderId = persons[communicationCount].Gender == "male" ? EnumGender.Male.GetHashCode() : EnumGender.Female.GetHashCode(),
                    IsActive = true,
                    IsDeleted = false,
                };

                communicationCount++;

                return communication;
            });

            int companyCount = 0;
            fixture.Register<Customer>(() =>
            {
                var random = new Random();
                var customer = new Customer()
                {
                    CityId = 1,
                    Title = persons[companyCount].Company,
                    Explanation = Guid.NewGuid().ToString().Substring(0, 20) + " " + Guid.NewGuid().ToString().Substring(0, 20),
                    IsActive = true,
                    IsDeleted = false,
                };

                customer.Communications = new List<Communication>();
                for (int i = 0; i < random.Next(1, 4); i++)
                {
                    customer.Communications.Add(fixture.Create<Communication>());
                }

                customer.RemoteMachines = new List<RemoteMachine>();
                for (int i = 0; i < random.Next(1, 3); i++)
                {
                    customer.RemoteMachines.Add(fixture.Create<RemoteMachine>());
                }

                companyCount++;

                return customer;
            });

            return fixture;
        }
     
    }
}