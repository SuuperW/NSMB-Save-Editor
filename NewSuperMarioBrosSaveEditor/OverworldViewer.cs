using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NewSuperMarioBrosSaveEditor
{
	public partial class OverworldViewer : UserControl
	{
		ToolTip ttip;
		List<Panel> overworldControls = new List<Panel>();

		public OverworldViewer()
		{
			InitializeComponent();
			ttip = new ToolTip();
		}

		public void LoadOverworld(JObject jObject)
		{
			foreach (Panel p in overworldControls)
			{
				p.Dispose();
				Controls.Remove(p);
			}
			// Scroll to 0, 0 so that locations that are set later on are correct.
			AutoScrollPosition = new Point(AutoScrollPosition.X, -AutoScrollPosition.Y);

			string worldPrefix = ((int)jObject["id"] + 1).ToString() + "-";
			JToken nodes = jObject["nodes"];
			JToken paths = jObject["paths"];
			// Create nodes
			const int nodeSeparation = 20;
			const int nodeSize = 10;
			int minX = int.MaxValue;
			int minY = int.MaxValue;
			int maxX = int.MinValue;
			int maxY = int.MinValue;
			foreach (JToken node in nodes)
			{
				Panel p = new Panel();
				p.Location = new Point((int)node["location"][0] * nodeSeparation, (int)node["location"][2] * nodeSeparation);
				p.Size = new Size(nodeSize, nodeSize);
				p.BackColor = Color.Blue;
				p.Tag = (int)node["idInWorld"];
				ttip.SetToolTip(p, worldPrefix + (string)node["name"]);
				p.Parent = this;
				overworldControls.Add(p);

				// The control's AutoScroll property does not support scrolling to negative locations.
				// So, we must adjust all locations so that they are positive.
				minX = Math.Min(minX, p.Location.X);
				minY = Math.Min(minY, p.Location.Y);
				maxX = Math.Max(maxX, p.Location.X);
				maxY = Math.Max(maxY, p.Location.Y);
			}

			const int padding = 20;
			foreach (Panel p in overworldControls)
				p.Location = new Point(p.Location.X - minX + padding, p.Location.Y - minY + padding);
			paddingLbl.Location = new Point(maxX - minX + 2 * padding, maxY - minY + 2 * padding);
		}
	}
}
