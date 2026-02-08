namespace VueApp1.Server.Common.Errors
{
    public class ServiceError
    {
        public string Code { get; set; }        // e.g. "INVALID_TAG", "MACHINE_NOT_FOUND"
        public string Message { get; set; }     // Human readable
        public string Field { get; set; }       // e.g. "CHECK_DATE"
        public string MachineId { get; set; }   // Which machine failed (optional)
    }
}
