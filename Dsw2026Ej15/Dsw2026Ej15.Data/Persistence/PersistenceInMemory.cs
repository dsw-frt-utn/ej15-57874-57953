using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Interfaces;
using System.Text.Json;

namespace Dsw2026Ej15.Data.Persistence
{
    public class PersistenceInMemory : IPersistence
    {
        public List<Doctor> Doctors { get; private set; }

        public List<Speciality> Specialities { get; private set; }

        public PersistenceInMemory()
        {
            Doctors = new List<Doctor>();
            Specialities = LoadSpecialities();
        }

        private List<Speciality> LoadSpecialities()
        {
            var json = File.ReadAllText("specialities.json");

            return JsonSerializer.Deserialize<List<Speciality>>
            (
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            ) ?? new List<Speciality>();
        }
    }
}
