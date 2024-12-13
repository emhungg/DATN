using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.Models.Request
{
    public class VNPayIPNRequest
    {
        public string Vnp_Amount { get; set; }
        public string Vnp_BankCode { get; set; }
        public string Vnp_BankTranNo { get; set; }
        public string Vnp_CardType { get; set; }
        public string Vnp_OrderInfo { get; set; }
        public string Vnp_PayDate { get; set; }
        public string Vnp_ResponseCode { get; set; }
        public string Vnp_TmnCode { get; set; }
        public string Vnp_TransactionNo { get; set; }
        public string Vnp_TransactionStatus { get; set; }
        public string Vnp_TxnRef { get; set; }
        public string Vnp_SecureHash { get; set; }
    }
}
