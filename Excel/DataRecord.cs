using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestUtilities.Excel
{
    class DataRecord
    {
        //Should have properties which correspond to the Column Names in the file i.e.
        //CommonName,FormalName,TelephoneCode,CountryCode
        public String CommonName { get; set; }
        public String FormalName { get; set; }
        public String TelephoneCode { get; set; }
        public String CountryCode { get; set; }
    }

    public class PerfDataRecord
    {
        //Should have properties which correspond to the Column Names in the file i.e.
        //CommonName,FormalName,TelephoneCode,CountryCode
        public String Email { get; set; }
    }

    public class PerfDataRecord2
    {
        //Should have properties which correspond to the Column Names in the file i.e.
        //CommonName,FormalName,TelephoneCode,CountryCode
        public String Email { get; set; }
        public String LottoTicketNumber { get; set; }
        public String OpenLottoTicketNumber { get; set; }
        public String CompleteLottoTicketNumber { get; set; }
        public String LottoFavNumber { get; set; }
    }
}
