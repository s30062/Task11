using Task11.Services.dtos;

namespace Task11.Services.interfaces;

public interface IDeviceService
{
    Task<List<GetAllDeviceDto>> GetAllDevices(CancellationToken cancellationToken);
    Task<GetDeviceByIdDto?> GetDeviceById(int id, CancellationToken cancellationToken);
    Task<bool> CreateDevice(CreateDeviceDto dto, CancellationToken cancellationToken);
    Task<bool> UpdateDevice(int id, UpdateDeviceDto dto, CancellationToken cancellationToken);
    Task<bool> DeleteDevice(int id, CancellationToken cancellationToken);

 // security checks
    Task<int> GetEmployeeIdForAccount(int accountId, CancellationToken cancellationToken);
    Task<bool> IsDeviceAssignedToEmployee(int deviceId, int employeeId, CancellationToken cancellationToken);
}