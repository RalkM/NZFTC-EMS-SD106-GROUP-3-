using NZFTC_EMS.Data.Entities;

namespace NZFTC_EMS.Services.Payroll
{
    public class TaxResult
    {
        public decimal PAYE { get; set; }
        public decimal OtherDeductions { get; set; }
        public decimal NetPay { get; set; }
    }

    public interface ITaxService
    {
        TaxResult CalculatePayPeriodTax(decimal grossEarnings, PayFrequency frequency);
    }

    // ⚠️ Simplified NZ PAYE – NOT real IRD figures, just for your project.
    public class TaxService : ITaxService
    {
        public TaxResult CalculatePayPeriodTax(decimal grossEarnings, PayFrequency frequency)
        {
            // Very rough bands per period (simple percentages)
            // You can tweak this if your tutor wants more realism.
            decimal rate = frequency switch
            {
                PayFrequency.Weekly      => 0.20m, // 20%
                _ => 0.20m
            };

            var paye = Math.Round(grossEarnings * rate, 2);
            var other = Math.Round(grossEarnings * 0.03m, 2); // KiwiSaver etc.

            return new TaxResult
            {
                PAYE = paye,
                OtherDeductions = other,
                NetPay = grossEarnings - paye - other
            };
        }
    }
}
