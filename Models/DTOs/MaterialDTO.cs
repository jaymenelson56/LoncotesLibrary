namespace Library.Models.DTO
{
    public class MaterialDTO
    {
        public int Id { get; set; }
        public string MaterialName { get; set; }
        public int MaterialTypeId { get; set; }
        public MaterialTypeDTO MaterialType { get; set; }
        public int GenreId { get; set; }
        public GenreDTO Genre { get; set; }
        public List<CheckoutDTO>? Checkouts { get; set;}
        public DateTime? OutOfCirculationSince { get; set; }
    }
}