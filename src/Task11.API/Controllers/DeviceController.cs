using Task11.Services.dtos;
using Task11.Services.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Task11.API.Controllers;

[ApiController]
[Route("api/devices")]
[Authorize]
public class DeviceController : ControllerBase
{
    private readonly IDeviceService _deviceService;

    public DeviceController(IDeviceService deviceService)
    {
        _deviceService = deviceService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var devices = await _deviceService.GetAllDevices(cancellationToken);
        return devices.Any() ? Ok(devices) : NotFound("No devices found.");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var device = await _deviceService.GetDeviceById(id, cancellationToken);
        if (device == null) return NotFound("Device not found");

        // Only Admins or Assigned Users can view device
        var role = User.FindFirstValue(ClaimTypes.Role);
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        if (role != "Admin")
        {
            var employeeId = await _deviceService.GetEmployeeIdForAccount(userId, cancellationToken);
            if (!await _deviceService.IsDeviceAssignedToEmployee(id, employeeId, cancellationToken))
                return Forbid();
        }

        return Ok(device);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreateDeviceDto dto, CancellationToken cancellationToken)
    {
        await _deviceService.CreateDevice(dto, cancellationToken);
        return Created();
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, UpdateDeviceDto dto, CancellationToken cancellationToken)
    {
        await _deviceService.UpdateDevice(id, dto, cancellationToken);
        return Ok();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _deviceService.DeleteDevice(id, cancellationToken);
        return Ok();
    }
}
