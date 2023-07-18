﻿using VL.Core.EditorAttributes;

namespace VL.ImGui.Widgets
{
    [GenerateNode(Category = "ImGui.Widgets", Tags = "toggle")]
    [WidgetType(WidgetType.Default)]
    internal partial class Checkbox : ChannelWidget<bool>, IHasLabel
    {

        public string? Label { get; set; }

        internal override void UpdateCore(Context context)
        {
            var value = Update();
            if (ImGuiNET.ImGui.Checkbox(widgetLabel.Update(Label), ref value))
                Value = value;
        }
    }
}
