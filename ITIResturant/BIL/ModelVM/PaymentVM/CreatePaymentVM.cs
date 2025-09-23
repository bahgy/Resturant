using System.ComponentModel.DataAnnotations;
using Restaurant.DAL.Enum;

namespace Resturant.BLL.ModelVM.PaymentVM
{
    public class CreatePaymentVM : IValidatableObject
    {
        [Required]
        public int OrderId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }

        [Required]
        public PaymentMethod PaymentMethod { get; set; }

        // Billing info (required)
        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        [Required, EmailAddress] public string Email { get; set; }
        [Required] public string Phone { get; set; }
        [Required] public string Address { get; set; }
        [Required] public string City { get; set; }
        [Required] public string Country { get; set; }

        // Credit card info (conditionally required)
        public string? CardHolderName { get; set; }
        public string? CardNumber { get; set; }
        public string? ExpiryDate { get; set; }
        public string? CVV { get; set; }

        // PayPal info (conditionally required)
        public string? PaypalEmail { get; set; }

        // Custom validation
        public IEnumerable<ValidationResult> Validate(ValidationContext context)
        {
            if (PaymentMethod == PaymentMethod.CreditCard)
            {
                if (string.IsNullOrWhiteSpace(CardHolderName))
                    yield return new ValidationResult("Cardholder Name is required", new[] { nameof(CardHolderName) });
                if (string.IsNullOrWhiteSpace(CardNumber))
                    yield return new ValidationResult("Card Number is required", new[] { nameof(CardNumber) });
                if (string.IsNullOrWhiteSpace(ExpiryDate))
                    yield return new ValidationResult("Expiry Date is required", new[] { nameof(ExpiryDate) });
                if (string.IsNullOrWhiteSpace(CVV))
                    yield return new ValidationResult("CVV is required", new[] { nameof(CVV) });
            }
            else if (PaymentMethod == PaymentMethod.Paypal)
            {
                if (string.IsNullOrWhiteSpace(PaypalEmail))
                    yield return new ValidationResult("PayPal Email is required", new[] { nameof(PaypalEmail) });
            }
            // CashOnDelivery = no extra checks
        }
    }

}
