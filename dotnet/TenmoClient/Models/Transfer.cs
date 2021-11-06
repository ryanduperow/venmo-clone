using System;
using System.ComponentModel.DataAnnotations;

namespace TenmoClient.Models
{
    public class Transfer
    {
        [Required(ErrorMessage = "The field TransferID should not be blank.")]
        public int TransferID { get; set; }

        [Required(ErrorMessage = "The field TransferTypeID should not be blank.")]
        public int TransferTypeID { get; set; }

        [Required(ErrorMessage = "The field TransferStatusID should not be blank.")]
        public int TransferStatusID { get; set; }

        [Required(ErrorMessage = "The field AccountFrom should not be blank.")]
        public int AccountFrom { get; set; }

        [Required(ErrorMessage = "The field AccountTo should not be blank.")]
        public int AccountTo { get; set; }

        [Range(0.01, Double.PositiveInfinity, ErrorMessage = "The field Amount should be greater than 0.")]
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
