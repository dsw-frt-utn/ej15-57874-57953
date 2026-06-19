using Dsw2026Ej15.Api.Models;
using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Dsw2026Ej15.Domain.Exceptions;

namespace Dsw2026Ej15.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctorsController : ControllerBase
{
    private readonly IPersistence _persistence;

    public DoctorsController(IPersistence persistence)
    {
        _persistence = persistence;
    }
    [HttpPost]
    public IActionResult Creatre(CreateDoctorRequest request)
    {
        if(string.IsNullOrWhiteSpace(request.Name))
        {
           throw new ValidationException("Nombre es requerido");
        }
        if(string.IsNullOrEmpty(request.LicenseNumber))
        {
            throw new ValidationException("Numero de Licencia es requerido");
        }
        var speciality = _persistence.Specialities
            .FirstOrDefault(s => s.Id == request.SpecialityId);
        if (speciality == null)
        {
            throw new ValidationException("La especialidad que busca no existe");
        }
        var doctor = new Doctor
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            LicenseNumber = request.LicenseNumber,
            IsActive = true,
            Speciality = speciality
        };
        _persistence.Doctors.Add(doctor);
        return Created($"/api/doctors/{doctor.Id}", doctor);
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var activeDoctors = _persistence.Doctors
            .Where(d => d.IsActive)
            .ToList();
        return Ok(activeDoctors);
    }
    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        var doctor = _persistence.Doctors
            .FirstOrDefault(d => d.Id == id && d.IsActive);
        if (doctor == null)
        {
            return NotFound(); 
        }

        var response = new DoctorResponse
        {
            Name = doctor.Name,
            LicenseNumber = doctor.LicenseNumber,
            SpecialityName = doctor.Speciality?.Name ?? string.Empty,
        };
        return Ok(response);
    }

    [HttpDelete("{id}")]

    public IActionResult Delete(Guid id)
    {
        var doctor = _persistence.Doctors.FirstOrDefault(d => d.Id == id && d.IsActive);
        if( doctor== null)
        {
          return NotFound(); 
        }

        doctor.IsActive = false;
        return NoContent();  //204 
    }
}

