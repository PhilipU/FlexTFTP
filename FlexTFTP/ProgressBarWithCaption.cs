using System;
using System.Drawing;
using System.Windows.Forms;

namespace FlexTFTP
{
    public enum ProgressBarDisplayText
    {
        Percentage,
        CustomText
    }

    class ProgressBarWithCaption : ProgressBar
    {
        //Property to set to decide whether to print a % or Text
        public ProgressBarDisplayText DisplayStyle { get; set; }

        //Property to hold the custom text
        private string _mCustomText;
        public string CustomText
        {
            get => _mCustomText;
            set
            {
                _mCustomText = value;
                Invalidate();
            }
        }

        private const int WmPaint = 0x000F;
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            switch (m.Msg)
            {
                case WmPaint:
                    int mPercent = Convert.ToInt32((Convert.ToDouble(Value) / Convert.ToDouble(Maximum)) * 100);
                    dynamic flags = TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter | TextFormatFlags.SingleLine | TextFormatFlags.WordEllipsis;

                    using (Graphics g = Graphics.FromHwnd(Handle))
                    {
                        using (new SolidBrush(ForeColor))
                        {

                            switch (DisplayStyle)
                            {
                                case ProgressBarDisplayText.CustomText:
                                    TextRenderer.DrawText(g, CustomText, new Font("Arial", Convert.ToSingle(8.25), FontStyle.Regular), new Rectangle(0, 0, Width, Height), Color.Black, flags);
                                    break;
                                case ProgressBarDisplayText.Percentage:
                                    TextRenderer.DrawText(g, $"{mPercent}%", new Font("Arial", Convert.ToSingle(8.25), FontStyle.Regular), new Rectangle(0, 0, Width, Height), Color.Black, flags);
                                    break;
                            }

                        }
                    }

                    break;
            }

        }

    }
}
