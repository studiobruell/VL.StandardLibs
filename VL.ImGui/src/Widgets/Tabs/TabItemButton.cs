﻿using System.Reactive;

namespace VL.ImGui.Widgets
{

    [GenerateNode(Category = "ImGui.Widgets", Button = true)]
    internal partial class TabItemButton : ChannelWidget<Unit>, IHasLabel
    {
        public string? Label { get; set; }

        public ImGuiNET.ImGuiTabItemFlags Flags { private get; set; }

        internal override void UpdateCore(Context context)
        {
            Update();
            if (ImGuiNET.ImGui.TabItemButton(widgetLabel.Update(Label), Flags))
                Value = Unit.Default;
        }
    }
}
