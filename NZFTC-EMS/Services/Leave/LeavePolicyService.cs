using Microsoft.EntityFrameworkCore;
using NZFTC_EMS.Data;
using NZFTC_EMS.Data.Entities;
using NZFTC_EMS.Models.ViewModels.Leave;
using System.Text.Json;

namespace NZFTC_EMS.Services.Leave
{
    public class LeavePolicyService
    {
        private readonly AppDbContext _db;

        public LeavePolicyService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<LeavePolicyVM> GetPolicyAsync()
        {
            var policy = await _db.LeavePolicies.FirstOrDefaultAsync(x => x.LeavePolicyId == 1);
            if (policy == null) throw new Exception("Leave policy not found.");

            return new LeavePolicyVM
            {
                AnnualDefault = policy.AnnualDefault,
                AnnualAccrualRate = policy.AnnualAccrualRate,
                AnnualCarryOverLimit = policy.AnnualCarryOverLimit,
                AllowNegativeAnnual = policy.AllowNegativeAnnual,

                SickDefault = policy.SickDefault,
                SickAccrualRate = policy.SickAccrualRate,
                AllowUnpaidSick = policy.AllowUnpaidSick,

                CustomLeaveTypes = JsonSerializer.Deserialize<List<string>>(policy.CustomLeaveTypesJson) ?? new()
            };
        }

        public async Task UpdatePolicyAsync(LeavePolicyVM vm)
        {
            var policy = await _db.LeavePolicies.FirstAsync(x => x.LeavePolicyId == 1);

            policy.AnnualDefault = vm.AnnualDefault;
            policy.AnnualAccrualRate = vm.AnnualAccrualRate;
            policy.AnnualCarryOverLimit = vm.AnnualCarryOverLimit;
            policy.AllowNegativeAnnual = vm.AllowNegativeAnnual;

            policy.SickDefault = vm.SickDefault;
            policy.SickAccrualRate = vm.SickAccrualRate;
            policy.AllowUnpaidSick = vm.AllowUnpaidSick;

            policy.CustomLeaveTypesJson = JsonSerializer.Serialize(vm.CustomLeaveTypes);
            policy.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
        }
    }
}
