using TestFinal.Dtos;
using TestFinal.Models;

namespace TestFinal.Services.Interfaces;

public interface ICheckInService
{
    Task<(CheckIn checkIn, List<Violation> violations)> CreateCheckInAsync(int proctorUserId, CheckInCreateDto dto);
}
