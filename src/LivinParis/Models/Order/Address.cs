namespace LivinParisRoussilleTeynier.Models.Order
{
    public class Address
    {
        public int AddressId { get; set; }
        public int Number { get; set; }
        public string Street { get; set; }
        public string NearestMetro { get; set; }

        public Address(int addressId, int number, string street, string nearestMetro)
        {
            AddressId = addressId;
            Number = number;
            Street = street;
            NearestMetro = nearestMetro;
        }
    }
}
