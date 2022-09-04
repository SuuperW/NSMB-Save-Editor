using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;

using Newtonsoft.Json.Linq;

namespace NewSuperMarioBrosSaveEditor
{
	public partial class OverworldViewer : UserControl
	{
		ToolTip ttip;
		List<Panel> nodeControls = new List<Panel>();
		List<Panel> pathControls = new List<Panel>();
		int worldId;

		SaveFile saveFile;
		JToken nodes, paths;
		Color unlockedPathColor = Color.Black;
		Color lockedPathColor = Color.DarkGray;

		public OverworldViewer()
		{
			InitializeComponent();
			ttip = new ToolTip();
		}

		public void LoadOverworld(JObject jObject)
		{
			SuspendLayout();

			// Remove previous nodes
			foreach (Panel p in nodeControls)
			{
				p.Dispose();
				Controls.Remove(p);
			}
			nodeControls.Clear();
			foreach (Panel p in pathControls)
			{
				p.Dispose();
				Controls.Remove(p);
			}
			pathControls.Clear();
			// Scroll to 0, 0 so that locations that are set later on are correct.
			AutoScrollPosition = new Point(AutoScrollPosition.X, -AutoScrollPosition.Y);

			nodes = jObject["nodes"];
			paths = jObject["paths"];
			worldId = (int)jObject["id"];
			string worldPrefix = (worldId + 1).ToString() + "-";
			// Create nodes
			const int nodeSeparation = 24;
			const int nodeSize = 16;
			int minX = int.MaxValue;
			int minY = int.MaxValue;
			int maxX = int.MinValue;
			int maxY = int.MinValue;
			foreach (JToken node in nodes)
			{
				Panel p = new Panel();
				p.Location = new Point((int)node["location"][0] * nodeSeparation, (int)node["location"][2] * nodeSeparation);
				p.Size = new Size(nodeSize, nodeSize);
				p.BackColor = Color.Transparent;
				p.BackgroundImage = (string)node["name"] == "Start" ? Properties.Resources.Node_Start :Properties.Resources.Node_Locked;
				p.BackgroundImageLayout = ImageLayout.Zoom;
				p.Visible = (bool)node["isVisible"];
				p.Tag = (int)node["idInWorld"];
				ttip.SetToolTip(p, worldPrefix + (string)node["name"]);
				p.Parent = this;
				p.Click += NodeClicked;
				nodeControls.Add(p);
				// Pipes are a bit special, at least to us
				if ((string)node["name"] == "Pipe")
				{
					p.BackgroundImage = Properties.Resources.Pipe2d;
					p.Visible = true;
				}

				// The control's AutoScroll property does not support scrolling to negative locations.
				// So, we must adjust all locations so that they are positive.
				minX = Math.Min(minX, p.Location.X);
				minY = Math.Min(minY, p.Location.Y);
				maxX = Math.Max(maxX, p.Location.X);
				maxY = Math.Max(maxY, p.Location.Y);
			}

			const int padding = 18;
			foreach (Panel p in nodeControls)
				p.Location = new Point(p.Location.X - minX + padding, p.Location.Y - minY + padding);
			paddingLbl.Location = new Point(maxX - minX + 2 * padding + nodeSize, maxY - minY + 2 * padding + nodeSize);

			// Create paths
			const int lineHalfWidth = 3;
			foreach (JToken node in nodes)
			{
				int nodeId = (int)node["idInWorld"];
				foreach (JToken connection in node["connections"].Where((c) => !(bool)c["isBackwards"]))
				{
					string direction = (string)connection["direction"];
					int otherNode = (int)connection["destinationNodeId"];

					if (direction == "Pipe") continue;
					else if (direction == "Up" || direction == "Down")
					{
						// Temporarily swap X/Y so we can apply the same logic as right/left
						Point l = nodeControls[nodeId].Location;
						nodeControls[nodeId].Location = new Point(l.Y, l.X);
						l = nodeControls[otherNode].Location;
						nodeControls[otherNode].Location = new Point(l.Y, l.X);
					}
					bool isRight = direction == "Right" || direction == "Down";

					// We may need more than 1, if the path curves.
					List<Panel> pathsForConnection = new List<Panel>();
					Func<int, int, int, int, Panel> makeNewPanel = (x, y, w, h) =>
					{
						Panel p = new Panel();
						p.BackColor = Color.Black;
						p.Tag = (int)connection["pathIdInWorld"];
						p.Parent = this;
						ttip.SetToolTip(p, "Path " + p.Tag);
						pathsForConnection.Add(p);

						if (w < 0) { x += w; w = -w; }
						if (h < 0) { y += h; h = -h; }
						p.Location = new Point(x, y);
						p.Size = new Size(w, h);

						return p;
					};

					// First (and maybe only) segment
					int startX = nodeControls[nodeId].Location.X + nodeSize / 2;
					int startY = nodeControls[nodeId].Location.Y + nodeSize / 2;
					int length = nodeControls[otherNode].Location.X - nodeControls[nodeId].Location.X;
					if (length == 0) // 8-Bowser does this!
						length = nodeSeparation * (isRight ? 2 : -2);
					makeNewPanel(startX, startY - lineHalfWidth, length, lineHalfWidth * 2);
					// Second segment?
					if (nodeControls[nodeId].Location.Y != nodeControls[otherNode].Location.Y)
					{
						int height = nodeControls[otherNode].Location.Y - nodeControls[nodeId].Location.Y;
						makeNewPanel( startX + length - lineHalfWidth, startY, lineHalfWidth * 2, height);
						// And finally, potentially a third to go to horizontal again
						// This will be necessary on the path to 8-Bowser.
						if (nodeControls[nodeId].Location.X + length != nodeControls[otherNode].Location.X)
						{
							makeNewPanel(
								startX + length,
								startY + height - lineHalfWidth,
								nodeControls[otherNode].Location.X - (nodeControls[nodeId].Location.X + length),
								lineHalfWidth * 2
							);
						}
					}

					// Swap X/Y back
					if (direction == "Up" || direction == "Down")
					{
						Point l = nodeControls[nodeId].Location;
						nodeControls[nodeId].Location = new Point(l.Y, l.X);
						l = nodeControls[otherNode].Location;
						nodeControls[otherNode].Location = new Point(l.Y, l.X);
						// new controls
						foreach (Panel p in pathsForConnection)
						{
							l = p.Location;
							p.Location = new Point(l.Y, l.X);
							p.Size = new Size(p.Height, p.Width);
						}
					}

					foreach (Panel p in pathsForConnection)
					{
						pathControls.Add(p);
						p.Click += PathClicked;
					}
				}
			}

			if (saveFile != null)
				UpdateDisplay();

			ResumeLayout();
		}

		private void PathClicked(object sender, EventArgs e)
		{
			Panel clicked = sender as Panel;
			int id = (int)clicked.Tag;
			bool newUnlockStatus = clicked.BackColor != unlockedPathColor;
			SetPathLock(id, newUnlockStatus);
		}
		private void SetPathLock(int id, bool unlockStatus)
		{ 
			// Display
			foreach (Panel p in pathControls)
				if ((int)p.Tag == id) p.BackColor = unlockStatus ? unlockedPathColor : lockedPathColor;

			// Save file
			byte flags = saveFile.GetPathFlags(worldId, id);
			if (unlockStatus)
				flags |= (byte)SaveFile.PathFlags.Unlocked;
			else
				flags &= (byte)~SaveFile.PathFlags.Unlocked;
			saveFile.SetPathFlags(worldId, id, flags);
		}

		private void NodeClicked(object sender, EventArgs e)
		{
			// Get node
			Panel clicked = sender as Panel;
			int id = (int)clicked.Tag;
			byte flags = saveFile.GetNodeFlags(worldId, id);
			// Set completion for node
			bool newCompleteStatus = (flags & SaveFile.NodeFlags.Completed) == 0;
			SetNodeCompletion(id, newCompleteStatus);
			// Paths from the node too
			UnlockPathsFrom(id, newCompleteStatus, true, true);

			// Display
			UpdateDisplay();
		}
		private void SetNodeCompletion(int id, bool completed)
		{
			byte flags = saveFile.GetNodeFlags(worldId, id);
			if (completed)
				flags |= (byte)SaveFile.NodeFlags.Completed;
			else
				flags &= (byte)~SaveFile.NodeFlags.Completed;
			saveFile.SetNodeFlags(worldId, id, flags);
		}
		private void UnlockPathsFrom(int id, bool unlocked, bool normalExit, bool secretExit)
		{ 
			JToken connections = nodes[id]["connections"];
			foreach (JToken c in connections.Where((c) => !(bool)c["isBackwards"]))
			{
				int pathId = (int)c["pathIdInWorld"];
				JToken path = paths[pathId];
				// If unlocking, don't clear signs.
				// If locking, do put signs back.
				if (!(bool)path["isUnlockedBySign"] || !unlocked)
				{
					// Un/lock this path.
					SetPathLock(pathId, unlocked);
					// Where does it lead?
					int destinationId = (int)c["destinationNodeId"];
					JToken destination = nodes[destinationId];
					// If it leads to a main level, we're done. (has star coins should work)
					// If it leads to a non-playable level, un/lock paths from that one.
					if (!(bool)destination["hasStarCoins"])
					{
						// We also need to make sure we're not locking a path that a different level unlocked.
						// E.g., un-completing 1-3 while 1-2 secret goal is cleared should not lock 1-Tower.
						bool unlockNext = true;
						if (!unlocked)
						{
							JToken destConnections = destination["connections"];
							unlockNext = !destConnections
								.Where((dc) => (bool)dc["isBackwards"])
								.Any((dc) => (saveFile.GetPathFlags((int)dc["pathIdInWorld"]) & SaveFile.PathFlags.Unlocked) != 0);
						}

						if (unlockNext)
						{
							UnlockPathsFrom(destinationId, unlocked, normalExit, secretExit);
							// We also want to mark this node complete if it's an empty node.
							// (but not if it's a mushroom house)
							if ((string)destination["name"] == "dot")
								SetNodeCompletion(destinationId, unlocked);
						}
					}
				}
			}
		}

		public void ApplySave(SaveFile saveFile)
		{
			this.saveFile = saveFile;
		}

		private void UpdateDisplay()
		{ 
			SuspendLayout();

			// paths
			bool[] unlocked = new bool[0x1E];
			foreach (Panel p in pathControls)
			{
				int id = (int)p.Tag;
				int flags = saveFile.GetPathFlags(worldId, id);
				unlocked[id] = (flags & SaveFile.PathFlags.Unlocked) != 0;
				if (unlocked[id])
					p.BackColor = unlockedPathColor;
				else
					p.BackColor = lockedPathColor;
			}
		
			// nodes
			for (int i = 1; i < nodeControls.Count; i++)
			{
				if (!(bool)nodes[i]["isVisible"]) continue;

				Panel p = nodeControls[i];
				// Is it completed? The game checks that first.
				int flags = saveFile.GetNodeFlags(worldId, i);
				if ((flags & SaveFile.NodeFlags.Completed) != 0)
					p.BackgroundImage = Properties.Resources.Node_Complete;
				else
				{
					// Is it unlocked?
					JToken connections = nodes[i]["connections"];
					bool levelUnlocked = connections.Any((c) => (bool)c["isBackwards"] == true && unlocked[(int)c["pathIdInWorld"]]);
					if (levelUnlocked)
						p.BackgroundImage = Properties.Resources.Node_Unlocked;
					else
						p.BackgroundImage = Properties.Resources.Node_Locked;
				}
			}

			ResumeLayout();
		}
	}
}
