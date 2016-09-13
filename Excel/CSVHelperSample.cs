using CsvHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TestUtilities.db;
namespace TestUtilities.Excel
{
    public class CSVHelperSample
    {
        public static void WriteTest()
        {
            using (var sr = new StreamReader(@"countrylist.csv"))
            {
                using (var sw = new StreamWriter(@"countrylistoutput.csv"))
                {
                    var reader = new CsvReader(sr);
                    var writer = new CsvWriter(sw);

                    //CSVReader will now read the whole file into an enumerable
                    IEnumerable records = reader.GetRecords<DataRecord>().ToList();

                    //Write the entire contents of the CSV file into another
                    writer.WriteRecords(records);

                    //Now we will write the data into the same output file but will do it 
                    //Using two methods.  The first is writing the entire record.  The second
                    //method writes individual fields.  Note you must call NextRecord method after 
                    //using Writefield to terminate the record.

                    //Note that WriteRecords will write a header record for you automatically.  If you 
                    //are not using the WriteRecords method and you want to a header, you must call the 
                    //Writeheader method like the following:
                    //
                    //writer.WriteHeader<DataRecord>();
                    //
                    //Do not use WriteHeader as WriteRecords will have done that already.

                    foreach (DataRecord record in records)
                    {
                        //Write entire current record
                        writer.WriteRecord(record);

                        //write record field by field
                        writer.WriteField(record.CommonName);
                        writer.WriteField(record.FormalName);
                        writer.WriteField(record.TelephoneCode);
                        writer.WriteField(record.CountryCode);
                        //ensure you write end of record when you are using WriteField method
                        writer.NextRecord();
                    }
                }
            }
        }

        public static void ReadTest()
        {
            using (var sr = new StreamReader(@"countrylist.csv"))
            {
                var reader = new CsvReader(sr);

                //CSVReader will now read the whole file into an enumerable
                IEnumerable<DataRecord> records = reader.GetRecords<DataRecord>();

                //First 5 records in CSV file will be printed to the Output Window
                foreach (DataRecord record in records.Take(5))
                {
                    Debug.Print("{0} {1}, {2}, {3}", record.CommonName, record.CountryCode, record.FormalName,
                        record.TelephoneCode);
                }
            }


        }
    }
}
