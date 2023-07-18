﻿namespace VL.ImGui.Widgets
{
    [GenerateNode(Category = "ImGui.Widgets")]
    internal partial class RadioButton : ChannelWidget<int>, IHasLabel
    {
        public string? Label { get; set; }

        public int Index { get; set; }

        internal override void UpdateCore(Context context)
        {
            var value = Update();
            if (ImGuiNET.ImGui.RadioButton(widgetLabel.Update(Label), ref value, Index))
                Value = value;
        }
    }
}
