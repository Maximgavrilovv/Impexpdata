using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using Impexpdata.DatabaseContext;
using Impexpdata.Logging;
using Impexpdata.Models;
using Microsoft.EntityFrameworkCore;

namespace Impexpdata
{
    public class CustomerManager
    {
        public CustomerLogger Logger { get; }

        public CustomerManager()
        {
            Logger = new CustomerLogger();
        }

        public List<Customer> ImportData(string path)
        {
            var customerList = new List<Customer>();
            using (var reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';').ToList<string>();

                    if (values.Count < 2 || values[1].Length < 1)
                    {
                        Logger.Log($"Not enough data in a customer row: {line}");
                        continue;
                    }

                    //insert empty values to avoid IndexOutOfBoundsException (easier than to handle it)
                    while(values.Count < 4)
                    {
                        values.Add(string.Empty);
                    }

                    var customer = new Customer()
                    {
                        CustomerName = values[1],
                        ImportDate = DateTime.Now
                    };

                    var correctId = int.TryParse(values[0], out int id);

                    if (correctId)
                    {
                        customer.CustomerId = id;
                    } else
                    {
                        Logger.Log($"Wrong customer id - {values[0]} in {line}");
                        continue;
                    }

                    customer.Notes = values[2];
                    var correctCodeId = int.TryParse(values[3], out int codeId);
                    customer.CodeId = correctCodeId ? codeId : null;

                    using (var customerContext = new CustomerContext())
                    {
                        //check for duplicate id; comment this for easier testing
                        var customers = customerContext.Customers.Where(x => x.CustomerId == id).ToList();
                        if (customers.Count > 0)
                        {
                            Logger.Log($"Wrong customer id - {values[0]} already exists in DB!");
                            continue;
                        }

                        customerContext.Customers.Add(customer);
                        customerContext.SaveChanges();
                        customerList.Add(customer);
                    }
                }
            }

            //this method could be void as well, return value for unit testing
            return customerList;
        }

        public List<CustomerExportModel> ExportData(string path)
        {
            using (var customerContext = new CustomerContext())
            {
                //if export date is null, then customer wasn't yet exported
                var customers = customerContext.Customers.Where(x => x.ExportDate == null).ToList();
                var codeNames = customerContext.CodeLookup.ToList();
                var customersListToExport = new List<CustomerExportModel>();

                customers.ForEach(x => x.ExportDate = DateTime.Now);
                customerContext.SaveChanges();

                foreach (var customer in customers)
                {
                    var codeLookup = codeNames.FirstOrDefault(x => x.CodeId == customer.CodeId, null);
                    var codeName = codeLookup == null ? string.Empty : codeLookup.CodeName;

                    customersListToExport.Add(new CustomerExportModel()
                    {
                        CustomerId = customer.CustomerId,
                        CustomerName = customer.CustomerName,
                        Notes = customer.Notes,
                        ImportDate = customer.ImportDate,
                        ExportDate = customer.ExportDate,
                        CodeName = codeName
                    });
                }

                if (!File.Exists(path))
                {
                    var file = File.Create(path);
                    file.Close();
                }

                foreach(var customer in customersListToExport)
                {
                    File.AppendAllText(path, customer.GetCsvString());
                }

                return customersListToExport;
            }
        }
    }
}
