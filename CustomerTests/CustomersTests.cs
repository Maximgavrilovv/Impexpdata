using Impexpdata;
using Impexpdata.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerTests
{
    [TestClass]
    public class CustomersTests
    {
        /// <summary>
        /// 1. Create fake customer data, save it to file and run import
        /// 2. Check if more than 0 rows were imported
        /// </summary>
        [TestMethod]
        public void TestImportData()
        {
            var customerManager = new CustomerManager();
            string path = "importedData.csv";
            var customersListToExport = new List<Customer>();
            var rand = new Random();

            //create fake data manually
            for (int i = 0; i < 5; i++)
            {
                customersListToExport.Add(new Customer()
                {
                    CustomerId = rand.Next(100000),
                    CustomerName = $"Customer{i}",
                    Notes = $"Notes{i}",
                    CodeId = i
                });
            }

            if (!File.Exists(path))
            {
                var file = File.Create(path);
                file.Close();
            }

            //write fake data to file
            foreach (var customer in customersListToExport)
            {
                File.AppendAllText(path, $"{customer.CustomerId};{customer.CustomerName};{customer.Notes};{customer.CodeId}\n");
            }

            var rows = customerManager.ImportData(path);

            Assert.IsTrue(rows.Count > 0);
        }

        /// <summary>
        /// Tests if customers with same id will not be saved after import
        /// </summary>
        [TestMethod]
        public void TestImportSameId()
        {
            var customerManager = new CustomerManager();
            string path = "importedData.csv";
            var customersListToExport = new List<Customer>();

            var fakeCustomer = new Customer()
            {
                CustomerId = 150,
                CustomerName = $"Customer{150}",
                Notes = $"Notes{150}",
                CodeId = 150
            };

            customersListToExport.Add(fakeCustomer);
            customersListToExport.Add(fakeCustomer);

            if (!File.Exists(path))
            {
                var file = File.Create(path);
                file.Close();
            }

            //write fake data to file
            foreach (var customer in customersListToExport)
            {
                File.AppendAllText(path, $"{customer.CustomerId};{customer.CustomerName};{customer.Notes};{customer.CodeId}\n");
            }

            var rows = customerManager.ImportData(path);

            //Check if only one or less records were saved
            Assert.IsTrue(rows.Count < 2);
        }
    }
}