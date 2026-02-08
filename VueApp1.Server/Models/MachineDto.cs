namespace VueApp1.Server.Models
{
    public class MachineDto
    {
        public long id { get; set; }
        public int position { get; set; }
        public float lat{ get; set; }
        public float lon { get; set; }
        public double distance { get; set; }
        public string? house { get; set; }
        public string? street { get; set; }
        public string? city { get; set; }
        public string? postcode { get; set; }
        public string? full_address { get; set; }
        public MachineParamsDto? params_info { get; set; }

    }
}
