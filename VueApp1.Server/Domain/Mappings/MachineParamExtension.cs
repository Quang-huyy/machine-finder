using VueApp1.Server.Domain.Enums;
namespace VueApp1.Server.Domain.Mappings
{
    public static class MachineParamExtension
    {
        public static string toString(this MachineParam param)
        {
            return param switch
            {
                MachineParam.BRAND => "brand",
                MachineParam.CHARGE => "charge",
                MachineParam.CHECK_DATE => "check_date",
                MachineParam.DISUSED => "disused",
                MachineParam.FEE => "fee",
                MachineParam.INDOOR => "indoor",
                MachineParam.OPENING_HOURS => "opening_hours",
                MachineParam.MACHINE_OPERATOR => "machine_operator",
                MachineParam.CASH => "cash",
                MachineParam.COINS => "coins",
                MachineParam.CREDIT_CARDS => "credit_cards",
                MachineParam.DEBIT_CARDS => "debit_cards",
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
