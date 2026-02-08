using System.Text.Json.Serialization;

namespace VueApp1.Server.Models
{
    public class MachineSuggestionDto
    {
        public long MachineID { get; set; }
        public string TagKey { get; set; }
        public string TagValue { get; set; }
        public string SuggestTagValue { get; set; }

    }
}
