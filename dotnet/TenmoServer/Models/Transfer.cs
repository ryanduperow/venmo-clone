namespace TenmoServer.Models
{
    public class Transfer
    {
       public int TransferID { get; set; }
        public int TransferTypeID { get; set; }
        public int TransferStatusID { get; set; }
        //do we want a transfer_status_desc property?
        public int AccountFrom { get; set; }
        public int AccountTo { get; set; }
        public decimal Amount { get; set; }
    }
}
