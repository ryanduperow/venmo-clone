namespace TenmoClient.Models
{
    public class Transfer
    {
        public int TransferID { get; set; }
        public int TransferTypeID { get; set; }
        public int TransferStatusID { get; set; }
        public int AccountFrom { get; set; }
        public int AccountTo { get; set; }
        public decimal Amount { get; set; }

        public string FormattedTransfer
        {
            get 
            { 
                return $"[{TransferID}]: Transfer ${Amount} from {AccountFrom} to {AccountTo}. TypeID:{TransferTypeID} Status:{TransferStatusID}"; 
            }
            
        }

    }
}
