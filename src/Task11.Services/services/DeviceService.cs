using System.Text.Json;
using Task11.API;
using Task11.Repository.context;
using Task11.Repository.interfaces;
using Task11.Services.dtos;
using Task11.Services.interfaces;
using Microsoft.EntityFrameworkCore;

namespace Task11.Services.services;

public class DeviceService : IDeviceService
{
    private readonly IDeviceRepository _deviceRepository;
    private readonly EmployeeDatabaseContext _context;

    public DeviceService(IDeviceRepository deviceRepository, EmployeeDatabaseContext context)
    {
        _deviceRepository = deviceRepository;
        _context = context;
    }

    public async Task<List<GetAllDeviceDto>> GetAllDevices(CancellationToken cancellationToken)
    {
        var devices = await _deviceRepository.GetAllDevices(cancellationToken);
        return devices.Select(d => new GetAllDeviceDto(d.Id, d.Name)).ToList();
    }

    public async Task<GetDeviceByIdDto?> GetDeviceById(int id, CancellationToken cancellationToken)
    {
        var device = await _deviceRepository.GetDeviceById(id, cancellationToken);
        if (device == null) return null;

        var dto = new GetDeviceByIdDto
        {
            Name = device.Name,
            DeviceType = device.DeviceType?.Name ?? "Unknown",
            IsEnabled = device.IsEnabled,
            AdditionalProperties = JsonDocument.Parse(device.AdditionalProperties).RootElement,
            CurrentEmployee = null
        };

        var assigned = device.DeviceEmployees.FirstOrDefault(de => de.ReturnDate == null);
        if (assigned != null)
        {
            var person = assigned.Employee.Person;
            dto.CurrentEmployee = new GetAllEmployeeDto
            {
                Id = assigned.Employee.Id,
                FullName = $"{person.FirstName} {person.MiddleName} {person.LastName}".Replace("  ", " ").Trim()
            };
        }

        return dto;
    }

    public async Task<bool> CreateDevice(CreateDeviceDto createDto, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(createDto.DeviceType))
            throw new ArgumentException("Device type is required");

        var type = await _deviceRepository.GetDeviceTypeByName(createDto.DeviceType, cancellationToken);
        if (type == null)
            throw new ArgumentException("Invalid device type");

        var device = new Device
        {
            Name = createDto.Name,
            DeviceType = type,
            IsEnabled = createDto.IsEnabled,
            AdditionalProperties = createDto.AdditionalProperties ?? ""
        };

        return await _deviceRepository.CreateDevice(device, cancellationToken);
    }

    public async Task<bool> UpdateDevice(int id, UpdateDeviceDto dto, CancellationToken cancellationToken)
    {
        var device = await _deviceRepository.GetDeviceById(id, cancellationToken);
        if (device == null)
            throw new KeyNotFoundException("Device not found");

        var type = await _deviceRepository.GetDeviceTypeByName(dto.DeviceType!, cancellationToken);
        if (type == null)
            throw new ArgumentException("Invalid device type");

        var update = new Device
        {
            Name = dto.Name,
            IsEnabled = dto.IsEnabled,
            DeviceType = type,
            AdditionalProperties = dto.AdditionalProperties ?? ""
        };

        return await _deviceRepository.UpdateDevice(id, update, cancellationToken);
    }

    public async Task<bool> DeleteDevice(int id, CancellationToken cancellationToken)
    {
        var device = await _deviceRepository.GetDeviceById(id, cancellationToken);
        if (device == null)
            throw new KeyNotFoundException("Device not found");

        return await _deviceRepository.DeleteDevice(id, cancellationToken);
    }

    public async Task<int> GetEmployeeIdForAccount(int accountId, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts
            .Include(a => a.Employee)
            .FirstOrDefaultAsync(a => a.Id == accountId, cancellationToken);

        return account?.EmployeeId ?? 0;
    }

    public async Task<bool> IsDeviceAssignedToEmployee(int deviceId, int employeeId, CancellationToken cancellationToken)
    {
        return await _context.DeviceEmployees
            .AnyAsync(de => de.DeviceId == deviceId && de.EmployeeId == employeeId && de.ReturnDate == null, cancellationToken);
    }
}
