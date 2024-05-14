namespace Library.Models.DTO
{
    public class PatronDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }

        public List<CheckoutWithLateFeeDTO>? Checkouts { get; set; }

        // Calculated property to calculate the total balance (unpaid fines) of the patron
        public decimal Balance
        {
            get
            {
                decimal totalBalance = 0;

                // Iterate through the checkouts and sum up the late fees
                if (Checkouts != null)
                {
                    foreach (var checkout in Checkouts)
                    {
                        // Add the late fee to the total balance if it exists
                        if (checkout.LateFee.HasValue)
                        {
                            totalBalance += checkout.LateFee.Value;
                        }
                    }
                }

                return totalBalance;
            }
        }
    }
}