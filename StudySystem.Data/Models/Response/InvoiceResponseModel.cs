using System;

namespace StudySystem.Data.Models.Response
{
    public class InvoiceResponseModel
    {
        public string InvoiceId { get; set; }
        public string Date { get; set; }
        public string Buyer { get; set; }
        public string PaymentMethod { get; set; }
        public string Address { get; set; }
        public string AmountInWords { get; set; }

        public double ToTalAmountNotVAT { get; set; }
        public double ToTalAmountVAT { get; set; }
        public double ToTalAmount { get; set; }
        public List<ItemInvoiceResponseModel> Items { get; set; }
    }

    public class ItemInvoiceResponseModel
    {
        public string ItemName { get; set; }
        public string Unit { get; set; }

        public double Quantity { get; set; }
        public double UnitPrice { get; set; }
        public double AmountNotVAT { get; set; }
        public double VATRate { get; set; }
        public double VAT { get; set; }
        public double Amount { get; set; }


    }
}
