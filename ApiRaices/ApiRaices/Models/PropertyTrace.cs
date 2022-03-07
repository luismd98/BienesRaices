namespace ApiRaices.Models
{
    public class PropertyTrace
    {
        public int IdPropertyTrace { get; set; }
        public string DateSale { get; set; }
        public string Name { get; set; }
        public float Value { get; set; }
        public byte Tax { get; set; }
        public int IdProperty { get; set; }
        public int IdOwner { get; set; }

    }
}
