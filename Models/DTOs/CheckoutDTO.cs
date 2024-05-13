namespace Library.Models.DTO
{
    public class CheckoutDTO
    {
        public int Id { get; set; }
        public int MaterialId { get; set; }
        public MaterialTypeDTO MaterialType { get; set; }
        public int PatronId { get; set; }
        public PatronDTO Patron { get; set; }
        public DateTime CheckoutDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}