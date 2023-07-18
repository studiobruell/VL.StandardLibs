﻿namespace VL.ImGui.Widgets
{
    /// <summary>
    /// Submit one header cell manually (rarely used)
    /// </summary>
    [GenerateNode(Category = "ImGui.Commands", GenerateRetained = false, IsStylable = false)]
    internal partial class TableHeader : Widget
    {

        public string? Label { private get; set; }

        internal override void UpdateCore(Context context)
        {
            if (context.IsInBeginTables)
                ImGuiNET.ImGui.TableHeader(widgetLabel.Update(Label));
        }
    }
}