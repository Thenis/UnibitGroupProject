using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace UnibitGroupProject
{
    using MetroFramework.Forms;
    using SolarSystemContext;
    using Repository;

    public partial class ExportForm : MetroForm
    {
        public ExportForm()
        {
            InitializeComponent();
        }

        private void ExportForm_Load(object sender, EventArgs e)
        {

        }

        private void metroButton7_Click(object sender, EventArgs e)
        {
            SolarSystemContext context = new SolarSystemContext();

            var topAnomaly = context.Anomalies.OrderByDescending(anomaly => anomaly.OriginPlanet.Persons.Count)
               .Select(anomaly => new
               {
                   id = anomaly.Id,
                   originPlanet = new
                   {
                       name = anomaly.OriginPlanet.Name
                   },
                   teleportPlanet = new
                   {
                       name = anomaly.TeleportPlanet.Name
                   },
                   victimsCount = anomaly.OriginPlanet.Persons.Count
               }).Take(1);
            var anomalyAsJson = JsonConvert.SerializeObject(topAnomaly, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText("../../anomaly.json", anomalyAsJson);
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            SolarSystemContext context = new SolarSystemContext();

            var exportedPlanetsAndPeople = context.Persons
                .Where(person => !person.Anomalies.Any())
                .Select(person => new
                {
                    name = person.Name,
                    homePlanet = new
                    {
                        name = person.HomePlanet.Name
                    }


                });
            var peopleAsJson = JsonConvert.SerializeObject(exportedPlanetsAndPeople, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText("../../people.json", peopleAsJson);

        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            SolarSystemContext context = new SolarSystemContext();

            var exportedPlanets = context.Planets
                .Where(planet => !planet.OriginAnomalies.Any())
                .Select(planet => new
                {
                    name = planet.Name
                });
            var planetsAsJson = JsonConvert.SerializeObject(exportedPlanets, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText("../../plantes.json", planetsAsJson);
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            var context = new SolarSystemContext();
            var exporetdAnomalies = context.Anomalies
                .OrderBy(anomaly => anomaly.Id)
                .Select(anomaly => new
                {
                    anomalyId = anomaly.Id,
                    originPlanetName = anomaly.OriginPlanet.Name,
                    teleportPlanetName = anomaly.TeleportPlanet.Name,
                    victims = anomaly.Persons.Select(p => new
                    {
                        name = p.Name
                    })
                });
            var xmlDocument = new XElement("anomalies");
            foreach (var exportedAnomaly in exporetdAnomalies)
            {
                var anomalyNode = new XElement("anomaly");
                anomalyNode.Add(new XAttribute("id", exportedAnomaly.anomalyId));
                anomalyNode.Add(new XAttribute("origin-planet", exportedAnomaly.originPlanetName));
                anomalyNode.Add(new XAttribute("teleport-planet", exportedAnomaly.teleportPlanetName));

                var victimsNode = new XElement("victims");
                foreach (var victim in exportedAnomaly.victims)
                {
                    var victimNode = new XElement("victim");
                    victimNode.Add(new XAttribute("name", victim.name));
                    victimsNode.Add(victimNode);
                }
                anomalyNode.Add(victimsNode);
                xmlDocument.Add(anomalyNode);
            }
            xmlDocument.Save("../../anomalies.xml");
        }
    }
}
