//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Data;
//using System.Data.OleDb;
//using System.IO;
//using System.Linq;
//using CsvHelper;
//using CustomerTracker.Web.Infrastructure.Repository;
//using CustomerTracker.Web.Models.Entities;
//using CustomerTracker.Web.Models.Enums;
//using CustomerTracker.Web.Utilities;
//using Ploeh.AutoFixture;

//namespace CustomerTracker.Web.App_Start
//{
//    public class Person
//    {
//        public string Gender { get; set; }

//        public string TelephoneNumber { get; set; }

//        public string GivenName { get; set; }

//        public string Surname { get; set; }

//        public string EmailAddress { get; set; }

//        public string Company { get; set; }

//    }
//    public static class DummyDataGenerate
//    {
//        public static void Generate()
//        {
//            //Fixture fixture = ConfigureFixture();
//            if (ConfigurationHelper.UnitOfWorkInstance == null)
//            {
//                ConfigurationHelper.UnitOfWorkInstance = new UnitOfWork();
//            }
 
//            var remoteMachineConnectionType = ConfigurationHelper.UnitOfWorkInstance.GetRepository<RemoteMachineConnectionType>();
//            remoteMachineConnectionType.Create(new RemoteMachineConnectionType() { IsActive = true, IsDeleted = false, Name = "Teamviewer" });
//            remoteMachineConnectionType.Create(new RemoteMachineConnectionType() { IsActive = true, IsDeleted = false, Name = "Remote Desktop" });
//            remoteMachineConnectionType.Create(new RemoteMachineConnectionType() { IsActive = true, IsDeleted = false, Name = "Vpn" });
//            remoteMachineConnectionType.Create(new RemoteMachineConnectionType() { IsActive = true, IsDeleted = false, Name = "Ammyy" });

           
//            //var repositoryProduct = ConfigurationHelper.UnitOfWorkInstance.GetRepository<Product>();
//            //var productLibrid = new Product() { IsActive = true, IsDeleted = false, Name = "Librid" };
//            //productLibrid.SubProducts = new Collection<Product>();
//            //productLibrid.SubProducts.Add(new Product() { IsActive = true, IsDeleted = false, Name = "Sirkülasyon Masaüstü" });
//            //productLibrid.SubProducts.Add(new Product() { IsActive = true, IsDeleted = false, Name = "Selfcheck Kiosk" });
//            //productLibrid.SubProducts.Add(new Product() { IsActive = true, IsDeleted = false, Name = "Otomasyon Web" });
//            //productLibrid.SubProducts.Add(new Product() { IsActive = true, IsDeleted = false, Name = "Opac Web" });
//            //repositoryProduct.Create(productLibrid);

//            //var repositoryCity = ConfigurationHelper.UnitOfWorkInstance.GetRepository<City>();
//            //repositoryCity.Create(new City() { IsActive = true, IsDeleted = false, Name = "Adana", Code = "01" });

//            ConfigurationHelper.UnitOfWorkInstance.Save();


//            //var repositoryCustomer = ConfigurationHelper.UnitOfWorkInstance.GetRepository<Customer>();
//            //for (int i = 0; i < 400; i++)
//            //{
//            //    var customer = fixture.Create<Customer>();
//            //    repositoryCustomer.Create(customer);
//            //}



//        }

//        private static Fixture ConfigureFixture()
//        {
//            var path = @AppDomain.CurrentDomain.BaseDirectory + "persons.csv";
//            var csv = new CsvReader(File.OpenText(path));
//            var persons = csv.GetRecords<Person>().ToList();


//            var fixture = new Fixture();
//            fixture.Register<RemoteMachine>(() =>
//            {
//                var random = new Random();

//                var remoteMachine = new RemoteMachine()
//                {
//                    Name = Guid.NewGuid().ToString().Substring(0, 10),
//                    Password = "123",
//                    RemoteAddress = "192.180.10.1",
//                    RemoteMachineConnectionTypeId = random.Next(1, 3),
//                    Explanation = Guid.NewGuid().ToString().Substring(0, 20) + " " + Guid.NewGuid().ToString().Substring(0, 20),
//                    Username = Guid.NewGuid().ToString().Substring(0, 5),
//                    IsActive = true,
//                    IsDeleted = false,
//                };
//                return remoteMachine;
//            });

//            int communicationCount = 0;
//            fixture.Register<Communication>(() =>
//            {
//                var random = new Random();
//                var communication = new Communication()
//                {
//                    DepartmentId = random.Next(1, 2),
//                    FirstName = persons[communicationCount].GivenName,
//                    LastName = persons[communicationCount].Surname,
//                    Email = persons[communicationCount].EmailAddress,
//                    MobilePhoneNumber = persons[communicationCount].TelephoneNumber,
//                    HomePhoneNumber = persons[communicationCount].TelephoneNumber,
//                    GenderId = persons[communicationCount].Gender == "male" ? EnumGender.Male.GetHashCode() : EnumGender.Female.GetHashCode(),
//                    IsActive = true,
//                    IsDeleted = false,
//                };

//                communicationCount++;

//                return communication;
//            });

//            int companyCount = 0;
//            fixture.Register<Customer>(() =>
//            {
//                var random = new Random();
//                var customer = new Customer()
//                {
//                    CityId = 1,
//                    Name = persons[companyCount].Company,
//                    Abbreviation = persons[companyCount].Company.Substring(0, 3),
//                    Explanation = Guid.NewGuid().ToString().Substring(0, 20) + " " + Guid.NewGuid().ToString().Substring(0, 20),
//                    IsActive = true,
//                    IsDeleted = false,
//                };

//                customer.Communications = new List<Communication>();
//                for (int i = 0; i < random.Next(1, 4); i++)
//                {
//                    customer.Communications.Add(fixture.Create<Communication>());
//                }

//                customer.RemoteMachines = new List<RemoteMachine>();
//                for (int i = 0; i < random.Next(1, 3); i++)
//                {
//                    customer.RemoteMachines.Add(fixture.Create<RemoteMachine>());
//                }

//                companyCount++;

//                return customer;
//            });

//            return fixture;
//        }

//        public static void GenerateBilgiIslem()
//        {
//            var loadDataSetFromExcel = LoadDataSetFromExcel("c:\\bilgiislem.xlsx", "sheet1");

//            foreach (var source in loadDataSetFromExcel.Tables[0].Rows.Cast<DataRow>())
//            {
//                var musteri = source["MÜŞTERİ"].ToString().Trim();
//                var adsoyad = source["BİLGİ İŞLEM"].ToString().Trim();
//                var sabittelefon = source["TELEFON"].ToString().Replace("/", "").Trim();
//                var ceptelefon = source["CEP TELEFONU"].ToString().Replace("/", "").Trim();
//                var mail = source["MAİL"].ToString().Replace("/", "").Trim();

//                var customer = ConfigurationHelper.UnitOfWorkInstance.GetRepository<Customer>().Filter(q => q.Name == musteri).SingleOrDefault();

//                if (customer == null)
//                    throw new ArgumentNullException();

//                var customerId = customer.Id;

//                var department = ConfigurationHelper.UnitOfWorkInstance.GetRepository<Department>().Filter(q => q.Name == "Bilgi İşlem").SingleOrDefault();

//                if (department == null)
//                    throw new ArgumentNullException();

//                var communication = new Communication
//                {
//                    CustomerId = customerId,
//                    HomePhoneNumber = sabittelefon,
//                    MobilePhoneNumber = ceptelefon,
//                    Email = mail,
//                    DepartmentId = department.Id,
//                    FirstName = adsoyad,
//                    GenderId = EnumGender.Male.GetHashCode(),
//                    IsActive = true
//                };

//                ConfigurationHelper.UnitOfWorkInstance.GetRepository<Communication>().Create(communication);

//            } 

//            ConfigurationHelper.UnitOfWorkInstance.Save();
//        }

//        public static void GenerateBilgiKontak()
//        {
//            var loadDataSetFromExcel = LoadDataSetFromExcel("c:\\kontak.xlsx", "sheet1");

//            foreach (var source in loadDataSetFromExcel.Tables[0].Rows.Cast<DataRow>())
//            {
//                var musteri = source["MÜŞTERİ"].ToString().Trim();
//                var adsoyad = source["KONTAK"].ToString().Trim();
//                var sabittelefon = source["TELEFON"].ToString().Replace("/", "").Trim();
//                var ceptelefon = source["CEP TELEFONU"].ToString().Replace("/", "").Trim();
//                var mail = source["MAİL"].ToString().Replace("/", "").Trim();

//                var customer = ConfigurationHelper.UnitOfWorkInstance.GetRepository<Customer>().Filter(q => q.Name == musteri).SingleOrDefault();

//                if (customer == null)
//                    throw new ArgumentNullException();

//                var customerId = customer.Id;

//                var department = ConfigurationHelper.UnitOfWorkInstance.GetRepository<Department>().Filter(q => q.Name == "Kontak").SingleOrDefault();

//                if (department == null)
//                    throw new ArgumentNullException();

//                var communication = new Communication
//                {
//                    CustomerId = customerId,
//                    HomePhoneNumber = sabittelefon,
//                    MobilePhoneNumber = ceptelefon,
//                    Email = mail,
//                    DepartmentId = department.Id,
//                    FirstName = adsoyad,
//                    GenderId = EnumGender.Male.GetHashCode(),
//                    IsActive = true
//                };

//                ConfigurationHelper.UnitOfWorkInstance.GetRepository<Communication>().Create(communication);

//            }

//            ConfigurationHelper.UnitOfWorkInstance.Save();

//        }

//        private static DataSet LoadDataSetFromExcel(string fileName, string sheetName)
//        {
//            String sConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + fileName + ";" + "Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1ReadOnly=False\"";

//            var objConn = new OleDbConnection(sConnectionString);

//            objConn.Open();

//            var objCmdSelect = new OleDbCommand("SELECT * FROM [" + sheetName + "$]", objConn);

//            var objAdapter1 = new OleDbDataAdapter();

//            objAdapter1.SelectCommand = objCmdSelect;

//            var dsExcelContent = new DataSet();

//            objAdapter1.Fill(dsExcelContent);

//            objConn.Close();

//            return dsExcelContent;
//        }


//    }
//}