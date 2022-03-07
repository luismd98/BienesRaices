namespace ApiRaices.Models
{
    public class PropertyImage
    {
        public int IdPropertyImage { get; set; }
        public int IdProperty { get; set; }
        public string Photo { get; set; }
        public bool Enabled { get; set; }   
    }
}
