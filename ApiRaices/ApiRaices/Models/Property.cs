namespace ApiRaices.Models
{
    public class Property
    {
        public int IdProperty { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public float Price { get; set; }
        public int CodeInternal { get; set; }
        public string Year { get; set; }
        public int IdOwner { get; set; }
    }
}
