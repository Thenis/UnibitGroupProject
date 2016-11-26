using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.XPath;
using DTOModels;
using MetroFramework.Forms;
using Models;
using Newtonsoft.Json;

namespace UnibitGroupProject
{
    using SolarSystemContext;
    using Repository;

    public partial class AddDataForm : MetroForm
    {

        private string fileLocation = null;

        public AddDataForm()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
        OpenFileDialog result = new OpenFileDialog();

        private void metroButton2_Click(object sender, EventArgs e)
        {
            //result.Filter = "GPD|*.json";
            if (result.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    fileLocation = result.FileName;
                }
                catch (IOException)
                {

                }
            }
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            SolarSystemContext context = new SolarSystemContext();
            Repository repository = new Repository();
            StringBuilder result = new StringBuilder();

            var json = File.ReadAllText(fileLocation);
            var solarSystems = JsonConvert.DeserializeObject<IEnumerable<SolarSystemDTO>>(json);
            foreach (var solarSystem in solarSystems)
            {
                string solarSystemName = solarSystem.Name;

                if (solarSystemName == null)
                {
                    result.AppendLine("Solar system name is required.");
                }
                else
                {
                    var solarSystemEntity = new SolarSystem()
                    {
                        Name = solarSystemName
                    };

                    if (repository.GetSolarSystemByName(solarSystemName, context) == null)
                    {
                        context.SolarSystems.Add(solarSystemEntity);
                        result.AppendLine($"Successfully imported Solar System {solarSystemName}.");
                    }
                    else
                    {
                        result.AppendLine("Attempt for duplicate record insertion.");
                    }
                }
            }
            Logger.Text=result.ToString();
            context.SaveChanges();

        }

        private void metroButton1_Click_1(object sender, EventArgs e)
        {
            SolarSystemContext context = new SolarSystemContext();
            Repository repository = new Repository();
            StringBuilder result = new StringBuilder();

            var json = File.ReadAllText(fileLocation);
            var stars = JsonConvert.DeserializeObject<IEnumerable<StarDTO>>(json);
            foreach (var star in stars)
            {
                string starName = star.Name;
                string solarSystemName = star.SolarSystem;
                if (starName == null || solarSystemName == null)
                {
                    result.AppendLine("Error: Invalid data.");
                }
                else
                {
                    var starEntity = new Star()
                    {
                        Name = starName,
                        SolarSystem = repository.GetSolarSystemByName(solarSystemName, context)
                    };
                    if (repository.GetSunByName(starName, context) == null)
                    {
                        context.Stars.Add(starEntity);
                        result.AppendLine($"Successfully imported star {starName}.");
                    }
                    else
                    {
                        result.AppendLine("Attempt for duplicate record insertion.");
                    }
                }
            }
            Logger.Text = result.ToString();
            context.SaveChanges();

        }

        private void metroButton3_Click(object sender, EventArgs e)
        {

            SolarSystemContext context = new SolarSystemContext();
            Repository repository = new Repository();
            StringBuilder result = new StringBuilder();

            var json = File.ReadAllText(fileLocation);
            var planets = JsonConvert.DeserializeObject<IEnumerable<PlanetDTO>>(json);
            foreach (var planet in planets)
            {
                string planetName = planet.Name;
                string sunName = planet.Sun;
                string solarSystemName = planet.SolarSystem;

                if (planetName == null || sunName == null || solarSystemName == null)
                {
                    result.AppendLine("Error: Invalid data.");
                }
                else
                {
                    var planetEntity = new Planet()
                    {
                        Name = planetName,
                        Sun = repository.GetSunByName(sunName, context),
                        SolarSystem = repository.GetSolarSystemByName(solarSystemName, context)
                    };
                    if (repository.GetPlanetByName(planetName, context) == null)
                    {
                        context.Planets.Add(planetEntity);
                        result.AppendLine($"Successfully imported planet {planetName}.");
                    }
                    else
                    {
                        result.AppendLine("Attempt for duplicate record insertion.");
                    }
                }
            }
            Logger.Text = result.ToString();
            context.SaveChanges();
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            SolarSystemContext context = new SolarSystemContext();
            Repository repository = new Repository();
            StringBuilder result = new StringBuilder();

            var json = File.ReadAllText(fileLocation);
            var anomalies = JsonConvert.DeserializeObject<IEnumerable<AnomaliesDTO>>(json);
            foreach (var anomaly in anomalies)
            {
                string originPlanetName = anomaly.OriginPlanet;
                string teleportPlanetName = anomaly.TeleportPlanet;
                if (originPlanetName == null || teleportPlanetName == null)
                {
                    result.AppendLine("Error: Invalid Data.");
                }
                else
                {
                    var originPlanet = repository.GetPlanetByName(originPlanetName, context);
                    var teleportPlanet = repository.GetPlanetByName(teleportPlanetName, context);
                    if (originPlanet == null || teleportPlanet == null)
                    {
                        result.AppendLine("Error: Invalid Data.");
                    }
                    else
                    {
                        var anomalyEntity = new Anomaly()
                        {
                            OriginPlanet = originPlanet,
                            TeleportPlanet = teleportPlanet
                        };
                        context.Anomalies.Add(anomalyEntity);
                        result.AppendLine("Successfully imported Anomaly.");
                    }
                }
            }
            Logger.Text = result.ToString();
            context.SaveChanges();

        }

        private void metroButton5_Click(object sender, EventArgs e)
        {
            SolarSystemContext context = new SolarSystemContext();
            Repository repository = new Repository();
            StringBuilder result = new StringBuilder();

            var json = File.ReadAllText(fileLocation);
            var persons = JsonConvert.DeserializeObject<IEnumerable<PersonDTO>>(json);
            foreach (var person in persons)
            {
                string personName = person.Name;
                string planetName = person.HomePlanet;

                if (personName == null || planetName == null)
                {
                    result.AppendLine("Error: Invalid Data.");
                }
                else
                {
                    var personEntity = new Person();
                    var personPlanet = repository.GetPlanetByName(planetName, context);
                    if (personPlanet == null)
                    {
                        result.AppendLine("Error: Invalid Data.");
                    }
                    else
                    {
                        if (repository.GetPersonByName(personName, context) == null)
                        {
                            personEntity.Name = personName;
                            personEntity.HomePlanet = personPlanet;
                            context.Persons.Add(personEntity);
                            result.AppendLine($"Successfully imported Person {personName}");
                        }
                        else
                        {
                            result.AppendLine("Attempt for duplicate record insertion.");
                        }
                    }
                }
                Logger.Text = result.ToString();
                context.SaveChanges();
            }
        }

        private void metroButton6_Click(object sender, EventArgs e)
        {
            SolarSystemContext context = new SolarSystemContext();
            Repository repository = new Repository();
            StringBuilder result = new StringBuilder();

            var json = File.ReadAllText(fileLocation);
            var anomalyVictims = JsonConvert.DeserializeObject<IEnumerable<AnomalyVictimsDTO>>(json);
            foreach (var anomalyVictim in anomalyVictims)
            {
                int anomalyId = 0;
                anomalyId = anomalyVictim.Id;
                string victimName = anomalyVictim.Person;
                if (anomalyId == 0 || victimName == null)
                {
                    result.AppendLine("Error: Invalid Data.");
                }
                else
                {
                    var anomalyEntity = repository.GetAnomalyById(anomalyId, context);
                    var personEntity = repository.GetPersonByName(victimName, context);

                    if (anomalyEntity == null || personEntity == null)
                    {
                        result.AppendLine("Error: Invalid Data.");
                    }
                    else
                    {
                        if (anomalyEntity.Persons.Contains(personEntity))
                        {
                            result.AppendLine("Duplicate data insertion attempt.");
                        }
                        else
                        {
                            anomalyEntity.Persons.Add(personEntity);
                            context.SaveChanges();
                        }
                    }
                }
            }
            Logger.Text = result.ToString();

        }

        private void metroButton7_Click(object sender, EventArgs e)
        {
            SolarSystemContext context = new SolarSystemContext();
            Repository repository = new Repository();
            StringBuilder result = new StringBuilder();

            var xml = XDocument.Load(fileLocation);
            var anomalies = xml.XPathSelectElements("anomalies/anomaly");

            context = new SolarSystemContext();
            foreach (var anomaly in anomalies)
            {
                var originPlanetName = anomaly.Attribute("origin-planet");
                var teleportPlanetName = anomaly.Attribute("teleport-planet");
                if (originPlanetName == null || teleportPlanetName == null)
                {
                    result.AppendLine("Error: Invalid Data.");
                }
                else
                {
                    var originPlanet = repository.GetPlanetByName(originPlanetName.Value, context);
                    var teleportPlanet = repository.GetPlanetByName(teleportPlanetName.Value, context);
                    if (originPlanet == null || teleportPlanet == null)
                    {
                        result.AppendLine("Error: Invalid Data.");
                    }
                    else
                    {
                        var anomalyEntity = new Anomaly()
                        {
                            OriginPlanet = originPlanet,
                            TeleportPlanet = teleportPlanet
                        };
                        context.Anomalies.Add(anomalyEntity);

                        var victims = anomaly.XPathSelectElements("victims/victim");
                        foreach (var victim in victims)
                        {
                            var name = victim.Attribute("name");
                            if (name == null)
                            {
                                result.AppendLine("Error: Invalid Data.");
                            }
                            else
                            {
                                string personName = name.Value;
                                var personEntity = repository.GetPersonByName(personName, context);
                                if (personEntity == null)
                                {
                                    result.AppendLine("Error: Invalid Data.");
                                }
                                else
                                {
                                    anomalyEntity.Persons.Add(personEntity);
                                }
                            }
                            context.SaveChanges();
                            result.AppendLine("Successfully imported anomaly.");
                        }
                    }
                }
            }
            Logger.Text = result.ToString();
        }
    }
}
