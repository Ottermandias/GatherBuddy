using ImGuiNET;
using OtterGui;
using OtterGui.Raii;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatherBuddy.Gui;

public partial class Interface
{ 

	private void DrawAutoTab()
	{
		using var id = ImRaii.PushId("Auto");
		using var tab = ImRaii.TabItem("Auto");
		ImGuiUtil.HoverTooltip("Automatically gather items based on your current location.");

		if (!tab)
			return;

		using var child = ImRaii.Child("AutoTab");
		if (!child)
			return;

		var auto = GatherBuddy.Config.AutoGather;
		if (ImGui.Checkbox("Enable Auto-Gathering", ref auto))
			GatherBuddy.Config.AutoGather = auto;


		if (ImGui.CollapsingHeader("Debug Info"))
		{

		}
	}
}

