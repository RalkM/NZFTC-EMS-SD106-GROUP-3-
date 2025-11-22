using NZFTC_EMS.Data.Entities;

namespace NZFTC_EMS.Services.Payroll
{
    public class TaxService
    {
        // 2024–2025 New Zealand PAYE Tax Brackets (Weekly Equivalent)
        private readonly (decimal Threshold, decimal Rate)[] _taxBracketsWeekly =
        {
            (0,        0.105m),
            (600,      0.175m),
            (2_100,    0.300m),
            (3_600,    0.330m),
            (7_000,    0.390m),
        };

        private const decimal ACC_LEVY = 0.0153m;              // 1.53%
        private const decimal KIWISAVER_EMPLOYEE = 0.03m;      // 3%
        private const decimal KIWISAVER_EMPLOYER = 0.03m;      // 3%
        private const decimal STUDENT_LOAN_RATE = 0.12m;       // 12%
        private const decimal SL_WEEKLY_THRESHOLD = 439.00m;   // IRD rule

        // --------------------------------------------------------------
        // CALCULATE PAYE TAX (Weekly)
        // --------------------------------------------------------------
        public decimal CalculatePAYE(decimal grossWeekly)
        {
            decimal tax = 0m;

            decimal remaining = grossWeekly;
            decimal previousThreshold = 0;

            foreach (var (threshold, rate) in _taxBracketsWeekly)
            {
                if (remaining <= 0)
                    break;

                decimal taxableAmount = Math.Min(remaining, threshold - previousThreshold);
                if (taxableAmount > 0)
                    tax += taxableAmount * rate;

                remaining -= taxableAmount;
                previousThreshold = threshold;
            }

            // If income is above highest bracket
            if (remaining > 0)
            {
                tax += remaining * 0.39m;
            }

            return Math.Round(tax, 2);
        }

        // --------------------------------------------------------------
        // ACC Earners Levy (1.53%)
        // --------------------------------------------------------------
        public decimal CalculateACC(decimal grossWeekly)
        {
            return Math.Round(grossWeekly * ACC_LEVY, 2);
        }

        // --------------------------------------------------------------
        // KiwiSaver
        // --------------------------------------------------------------
        public decimal CalculateKiwiSaverEmployee(decimal gross)
        {
            return Math.Round(gross * KIWISAVER_EMPLOYEE, 2);
        }

        public decimal CalculateKiwiSaverEmployer(decimal gross)
        {
            return Math.Round(gross * KIWISAVER_EMPLOYER, 2);
        }

        // --------------------------------------------------------------
        // Student Loan (12% above threshold)
        // --------------------------------------------------------------
        public decimal CalculateStudentLoan(decimal grossWeekly)
        {
            if (grossWeekly <= SL_WEEKLY_THRESHOLD)
                return 0m;

            var taxable = grossWeekly - SL_WEEKLY_THRESHOLD;
            return Math.Round(taxable * STUDENT_LOAN_RATE, 2);
        }
    }
}
