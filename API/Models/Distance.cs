namespace API.Models
{
    public class Distance
    {
        public Address address1 { get; set; }
        public Address address2 { get; set; }

        // Distance is measured in km
        public double distance { get; set; }


    }
}
