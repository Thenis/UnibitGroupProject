


namespace Repository
{
    using SolarSystemContext;
    using System.Linq;
    using Models;

    public class Repository
    {

        public Person GetPersonByName(string victimName, SolarSystemContext context)
        {
            var personEntity = context.Persons.FirstOrDefault(p => p.Name == victimName);
            return personEntity;
        }

        public Anomaly GetAnomalyById(int anomalyId, SolarSystemContext context)
        {
            var anomalyEntity = context.Anomalies.FirstOrDefault(a => a.Id == anomalyId);
            return anomalyEntity;
        }

        public Planet GetPlanetByName(string planetName, SolarSystemContext context)
        {
            var planetEntity = context.Planets.FirstOrDefault(p => p.Name == planetName);
            return planetEntity;
        }

        public Star GetSunByName(string sunName, SolarSystemContext context)
        {
            var sunEntity = context.Stars.FirstOrDefault(s => s.Name == sunName);
            return sunEntity;
        }

        public SolarSystem GetSolarSystemByName(string solarSystem, SolarSystemContext context)
        {
            var solarSystemEntity = context.SolarSystems.FirstOrDefault(s => s.Name == solarSystem);
            return solarSystemEntity;
        }
    }
}
