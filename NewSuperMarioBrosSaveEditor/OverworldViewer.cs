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

		SaveFile saveFile;
		World world;
		List<OverworldNode> nodes;
		List<OverworldPath> paths;
		Color unlockedPathColor = Color.Black;
		Color lockedPathColor = Color.DarkGray;

		public event Action LocksChanged;

		public enum NodeAction { Normal = 0, Secret, All }
		public NodeAction NodeClickAction;
		public bool NodeActionOnDoubleClickOnly = false;

		private int selectedNode = 0;

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
			mainPanel.AutoScrollPosition = new Point(mainPanel.AutoScrollPosition.X, -mainPanel.AutoScrollPosition.Y);

			world = (World)jObject;
			nodes = new List<OverworldNode>();
			foreach (JToken node in jObject["nodes"])
				nodes.Add((OverworldNode)node);
			paths = new List<OverworldPath>();
			foreach (JToken path in jObject["paths"])
				paths.Add((OverworldPath)path);
			string worldPrefix = (world.id + 1).ToString() + "-";
			// Create nodes
			const int nodeSeparation = 24;
			const int nodeSize = 16;
			int minX = int.MaxValue;
			int minY = int.MaxValue;
			int maxX = int.MinValue;
			int maxY = int.MinValue;
			foreach (OverworldNode node in nodes)
			{
				Panel p = new Panel
				{
					Location = new Point(node.location[0] * nodeSeparation, node.location[2] * nodeSeparation),
					Size = new Size(nodeSize, nodeSize),
					BackColor = Color.Transparent,
					BackgroundImage = node.name == "Start" ? Properties.Resources.Node_Start : Properties.Resources.Node_Locked,
					BackgroundImageLayout = ImageLayout.Zoom,
					Visible = node.isVisible,
					Tag = node.idInWorld,
					Parent = mainPanel
				};
				ttip.SetToolTip(p, worldPrefix + node.name);
				p.Click += NodeClicked;
				p.DoubleClick += NodeDoubleClicked;
				nodeControls.Add(p);
				// Pipes are a bit special, at least to us
				if (node.name == "Pipe")
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
			foreach (OverworldNode node in nodes)
			{
				int nodeId = node.idInWorld;
				foreach (OverworldNode.Connection connection in node.connections.Where((c) => !c.isBackwards))
				{
					string direction = connection.direction;
					int otherNode = connection.destinationNodeId;

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
						Panel p = new Panel
						{
							BackColor = Color.Black,
							Tag = connection.pathIdInWorld,
							Parent = mainPanel
						};
						ttip.SetToolTip(p, "Path " + p.Tag);
						pathsForConnection.Add(p);
						pathControls.Add(p);

						if (w < 0) { x += w; w = -w; }
						if (h < 0) { y += h; h = -h; }
						p.Location = new Point(x, y);
						p.Size = new Size(w, h);

						p.Click += PathClicked;
						p.DoubleClick += PathClicked;

						return p;
					};

					// First (and maybe only) segment
					int startX = nodeControls[nodeId].Location.X + nodeSize / 2;
					int startY = nodeControls[nodeId].Location.Y + nodeSize / 2;
					int length = nodeControls[otherNode].Location.X - nodeControls[nodeId].Location.X;
					if (length == 0) // 8-Bowser does this!
						length = nodeSeparation * (isRight ? 2 : -2);
					makeNewPanel(startX, startY - lineHalfWidth, length, lineHalfWidth * 2);
					
					// Sign?
					if (paths[connection.pathIdInWorld].isUnlockedBySign)
					{
						Panel p = makeNewPanel(startX - nodeSize / 2 + length / 2, startY - nodeSize / 2, nodeSize, nodeSize);
						p.BackgroundImage = Properties.Resources.Starcoin_Sign;
						p.BackgroundImageLayout = ImageLayout.Zoom;
						p.BackColor = Color.Transparent;
						p.BringToFront();
					}

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

			if (LocksChanged != null)
				LocksChanged.Invoke();
		}
		private void SetPathLock(int id, bool unlockStatus)
		{
			// Display
			foreach (Panel p in pathControls)
			{
				if ((int)p.Tag == id)
				{
					if (p.BackgroundImage == null)
						p.BackColor = unlockStatus ? unlockedPathColor : lockedPathColor;
					else
						p.Visible = !unlockStatus;
				}
			}

			// Save file
			byte flags = saveFile.GetPathFlags(world.id, id);
			if (unlockStatus)
				flags |= SaveFile.PathFlags.Unlocked;
			else
				flags = 0;
			saveFile.SetPathFlags(world.id, id, flags);

			// Tell parent
			if (LocksChanged != null)
				LocksChanged.Invoke();
		}

		private void NodeClicked(object sender, EventArgs e)
		{
			if (!NodeActionOnDoubleClickOnly)
				NodeDoubleClicked(sender, e);
			else
			{
				// We still want to select it
				selectedNode = (int)(sender as Panel).Tag;
				UpdateDisplay(false);
			}
		}
		private void NodeDoubleClicked(object sender, EventArgs e)
		{
			// Get node
			Panel clicked = sender as Panel;
			selectedNode = (int)clicked.Tag;
			byte flags = saveFile.GetNodeFlags(world.id, selectedNode);
			// Are we clearing or unclearing?
			bool levelWasCompleted = (flags & SaveFile.NodeFlags.Completed) != 0;
			bool newCompleteStatus;
			if (!levelWasCompleted)
				newCompleteStatus = true;
			else if (NodeClickAction == NodeAction.All)
			{
				newCompleteStatus = (flags & SaveFile.NodeFlags.AllStarCoins) != SaveFile.NodeFlags.AllStarCoins ||
					nodes[selectedNode].connections
					.Where((c) => !c.isBackwards)
					.Where((c) => !paths[c.pathIdInWorld].isUnlockedBySign)
					.Any((c) => !saveFile.IsPathUnlocked(world.id, c.pathIdInWorld));
			}
			else
			{
				newCompleteStatus = nodes[selectedNode].connections
					.Where((c) => !c.isBackwards)
					.Where((c) => !paths[c.pathIdInWorld].isUnlockedBySign)
					.Where((c) => paths[c.pathIdInWorld].isUnlockedBySecretGoal ^ (NodeClickAction == NodeAction.Normal))
					.Any((c) => !saveFile.IsPathUnlocked(world.id, c.pathIdInWorld));
			}
			// Node
			bool normalExit = NodeClickAction != NodeAction.Secret;
			bool secretExit = NodeClickAction != NodeAction.Normal;
			SetNodeCompletion(selectedNode, newCompleteStatus, normalExit, secretExit);
			// Paths from the node too
			UnlockPathsFrom(selectedNode, newCompleteStatus, normalExit, secretExit);

			// Display
			UpdateDisplay();
		}
		private void SetNodeCompletion(int id, bool completed, bool normalExit, bool secretExit)
		{
			// This node/level
			byte nodeFlags = saveFile.GetNodeFlags(world.id, id);
			if (completed)
			{
				nodeFlags |= SaveFile.NodeFlags.Completed;
				if (NodeClickAction == NodeAction.All)
					nodeFlags |= SaveFile.NodeFlags.AllStarCoins;
			}
			else
				nodeFlags = 0;
			saveFile.SetNodeFlags(world.id, id, nodeFlags);

			// For special levels, set world flags
			OverworldNode node = nodes[id];
			// If secretExit it set, check if the node has a secret goal
			secretExit = secretExit && (!node.pathsByNormalExit.SequenceEqual(node.pathsBySecretExit));

			ushort flags = 0;
			if (node.isFirstTower)
			{
				if (normalExit)
					flags |= SaveFile.WorldFlags.AllForTower;
			}
			else if (node.isCastle)
			{
				if (normalExit)
					flags |= SaveFile.WorldFlags.AllForCastle;
			}
			else if (node.isSecondTower)
			{
				if (normalExit)
					flags |= SaveFile.WorldFlags.AllForTower2;
			}
			// End of the world
			if (node.isLastLevelInWorld)
				flags |= SaveFile.WorldFlags.ExitWorldCutscene;

			// Set flags
			ushort worldFlags = saveFile.GetWorldFlags(world.id);
			if (completed)
				worldFlags |= flags;
			else
				worldFlags &= (ushort)~flags;
			saveFile.SetWorldFlags(world.id, worldFlags);
			// World unlocks are last, because they'll also modify flags for the current world.
			if (node.name == "Cannon")
			{
				if (normalExit)
					//           These are the worlds that each cannon leads to.
					
					UnlockWorld(world.cannonDestination);
				// TODO: Handle re-locking worlds.
			}
			else if (node.isLastLevelInWorld)
			{
				if (!node.isBowserCastle)
				{
					if (normalExit)
						UnlockWorld(world.normalNextWorld);
					if (secretExit)
						UnlockWorld(world.secretNextWorld);
					// TODO: Handle re-locking worlds.
				}
				else
					saveFile.PlayerHasSeenCredits = completed;
			}


		}
		private void UnlockPathsFrom(int id, bool unlocked, bool normalExit, bool secretExit)
		{ 
			foreach (OverworldNode.Connection c in nodes[id].connections.Where((c) => !c.isBackwards))
			{
				int pathId = c.pathIdInWorld;
				OverworldPath path = paths[pathId];

				bool shouldSet;
				if (unlocked)
				{
					// If we're unlocking, only get paths that match the exit we're clearing
					if (normalExit && !path.isUnlockedBySecretGoal ||
						secretExit && path.isUnlockedBySecretGoal)
						// and aren't signs
						shouldSet = !path.isUnlockedBySign;
					else
						shouldSet = false;
				}
				else
					// If we're locking, always lock.
					shouldSet = true;

				if (shouldSet && (!path.isUnlockedBySign || !unlocked))
				{
					// Un/lock this path.
					SetPathLock(pathId, unlocked);
					// Where does it lead?
					int destinationId = c.destinationNodeId;
					OverworldNode destination = nodes[destinationId];
					// If it leads to a main level, we're done. (has star coins should work)
					// If it leads to a non-playable level, un/lock paths from that one.
					if (!destination.hasStarCoins)
					{
						// We also need to make sure we're not locking a path that a different level unlocked.
						// E.g., un-completing 1-3 while 1-2 secret goal is cleared should not lock 1-Tower.
						bool unlockNext = true;
						if (!unlocked)
						{
							List<OverworldNode.Connection> destConnections = destination.connections;
							unlockNext = !destConnections
								.Where((dc) => dc.isBackwards)
								.Any((dc) => (saveFile.GetPathFlags(world.id, dc.pathIdInWorld) & SaveFile.PathFlags.Unlocked) != 0);
						}

						if (unlockNext)
						{
							UnlockPathsFrom(destinationId, unlocked, normalExit, secretExit);
							// We also want to mark this node complete if it's an empty node.
							// (but not if it's a mushroom house)
							if (destination.name == "dot")
								SetNodeCompletion(destinationId, unlocked, normalExit, secretExit);
						}
					}
				}
			}
		}
		private void UnlockWorld(int id)
		{
			ushort worldFlags = saveFile.GetWorldFlags(id);
			worldFlags |= SaveFile.WorldFlags.AllForUnlocked;
			saveFile.SetWorldFlags(id, worldFlags);

			// Unlocking a world also sets cutscene flags for previous worlds.
			int lastWorldToSetCutsceneses = id - 1;
			// Optional worlds do not set cutscene flags for the other optional world.
			if (id == 3 || id == 6)
				lastWorldToSetCutsceneses--;
			for (int i = 0; i <= lastWorldToSetCutsceneses; i++)
			{
				worldFlags = saveFile.GetWorldFlags(i);
				worldFlags |= SaveFile.WorldFlags.AllBowserJuniorCutscenes;
				saveFile.SetWorldFlags(i, worldFlags);
			}
		}

		public void ApplySave(SaveFile saveFile)
		{
			this.saveFile = saveFile;
		}

		private void starCoinPbx_Click(object sender, EventArgs e)
		{
			if (saveFile != null)
			{
				byte flags = saveFile.GetNodeFlags(world.id, selectedNode);
				int starCoinShift = (int)(sender as Control).Tag;
				saveFile.SetNodeFlags(world.id, selectedNode, (byte)(flags ^ (SaveFile.NodeFlags.StarCoin1 << starCoinShift)));
				UpdateDisplay(false);
			}
		}

		private void UpdateDisplay(bool updatePathsAndNodes = true)
		{ 
			SuspendLayout();

			if (updatePathsAndNodes)
			{
				// paths
				bool[] unlocked = new bool[0x1E];
				foreach (Panel p in pathControls)
				{
					int id = (int)p.Tag;
					int flags = saveFile.GetPathFlags(world.id, id);
					unlocked[id] = (flags & SaveFile.PathFlags.Unlocked) != 0;
					if (p.BackgroundImage == null)
					{
						if (unlocked[id])
							p.BackColor = unlockedPathColor;
						else
							p.BackColor = lockedPathColor;
					}
					else
						p.Visible = !unlocked[id];
				}

				// nodes
				for (int i = 1; i < nodeControls.Count; i++)
				{
					if (!nodes[i].isVisible) continue;

					Panel p = nodeControls[i];
					// Is it completed? The game checks that first.
					int flags = saveFile.GetNodeFlags(world.id, i);
					if ((flags & SaveFile.NodeFlags.Completed) != 0)
						p.BackgroundImage = Properties.Resources.Node_Complete;
					else
					{
						// Is it unlocked?
						bool levelUnlocked = nodes[i].connections.Any((c) => c.isBackwards == true && unlocked[c.pathIdInWorld]);
						if (levelUnlocked)
							p.BackgroundImage = Properties.Resources.Node_Unlocked;
						else
							p.BackgroundImage = Properties.Resources.Node_Locked;
					}
				}
			}

			// node info
			nodeNameLbl.Text = "W" + (world.id + 1).ToString() + "-" + nodes[selectedNode].name;
			starCoin1Pbx.Visible = starCoin2Pbx.Visible = starCoin3Pbx.Visible =
				nodes[selectedNode].hasStarCoins;

			byte nodeFlags = saveFile.GetNodeFlags(world.id, selectedNode);
			starCoin1Pbx.BackgroundImage = (nodeFlags & SaveFile.NodeFlags.StarCoin1) != 0 ? Properties.Resources.StarCoin : Properties.Resources.NoStarCoin;
			starCoin2Pbx.BackgroundImage = (nodeFlags & SaveFile.NodeFlags.StarCoin2) != 0 ? Properties.Resources.StarCoin : Properties.Resources.NoStarCoin;
			starCoin3Pbx.BackgroundImage = (nodeFlags & SaveFile.NodeFlags.StarCoin3) != 0 ? Properties.Resources.StarCoin : Properties.Resources.NoStarCoin;

			ResumeLayout();
		}
	}
}
