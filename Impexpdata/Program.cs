using Impexpdata;

var customer = new CustomerManager();
var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
var importPath = "importedData.csv";
var exportPath = "exportedData.csv";
//change both to false to check CLI options
bool import = true;
bool export = true;

for (int i = 0; i < args.Length; i++)
{
    if (args[i].StartsWith('-'))
    {
        if (args[i].Contains("import"))
        {
            import = true;
            importPath = args[i + 1];
        }
        if (args[i].Contains("export"))
        {
            export = true;
            exportPath = args[i + 1];
        }
    }
}

if (import) customer.ImportData(importPath);
if (export) customer.ExportData(exportPath);