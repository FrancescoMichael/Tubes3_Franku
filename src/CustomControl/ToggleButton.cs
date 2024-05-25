using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

/* Source:
 * https://www.youtube.com/watch?v=m7Iv6xfjnuw
 * https://rjcodeadvance.com/toggle-button-custom-controls-winform-c/
 */

namespace src.CustomControl
{
    internal class ToggleButton : CheckBox
    {
        // Fields
        private Color onBackColor = Color.MediumSlateBlue;
        private Color onToggleColor = Color.WhiteSmoke;
        private Color offBackColor = Color.Gray;
        private Color offToggleColor = Color.Gainsboro;
        private bool solidStyle = true;
        private int animationInterval = 1; // Interval between animation frames in ms
        private int animationStep = 5; // Number of pixels to move the toggle each frame
        private int togglePosition; // Current position of the toggle
        private Timer animationTimer;

        //Constructor
        public ToggleButton()
        {
            this.MinimumSize = new Size(150, 50);
            this.togglePosition = this.Checked ? this.Width - this.Height : 0;
            this.animationTimer = new Timer();
            this.animationTimer.Interval = animationInterval;
            this.animationTimer.Tick += AnimationTimer_Tick;
        }

        //Methods
        private GraphicsPath GetFigurePath()
        {
            int arcSize = this.Height - 1;
            Rectangle leftArc = new Rectangle(0, 0, arcSize, arcSize);
            Rectangle rightArc = new Rectangle(this.Width - arcSize - 2, 0, arcSize, arcSize);
            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(leftArc, 90, 180);
            path.AddArc(rightArc, 270, 180);
            path.CloseFigure();
            return path;
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            int toggleSize = this.Height - 5;
            pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            pevent.Graphics.Clear(this.Parent.BackColor);

            //Draw the control surface
            if (this.Checked) //ON
            {
                if (solidStyle)
                    pevent.Graphics.FillPath(new SolidBrush(onBackColor), GetFigurePath());
                else
                    pevent.Graphics.DrawPath(new Pen(onBackColor, 2), GetFigurePath());
            }
            else //OFF
            {
                if (solidStyle)
                    pevent.Graphics.FillPath(new SolidBrush(offBackColor), GetFigurePath());
                else
                    pevent.Graphics.DrawPath(new Pen(offBackColor, 2), GetFigurePath());
            }

            // Draw the toggle
            pevent.Graphics.FillEllipse(new SolidBrush(this.Checked ? onToggleColor : offToggleColor),
                new Rectangle(togglePosition, 2, toggleSize, toggleSize));
        }

        protected override void OnCheckedChanged(EventArgs e)
        {
            base.OnCheckedChanged(e);
            this.animationTimer.Start();
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            if (this.Checked) // Animate to the right
            {
                if (togglePosition < this.Width - this.Height)
                {
                    togglePosition += animationStep;
                    if (togglePosition > this.Width - this.Height)
                    {
                        togglePosition = this.Width - this.Height;
                        this.animationTimer.Stop();
                    }
                }
                else
                {
                    this.animationTimer.Stop();
                }
            }
            else // Animate to the left
            {
                if (togglePosition > 0)
                {
                    togglePosition -= animationStep;
                    if (togglePosition < 0)
                    {
                        togglePosition = 0;
                        this.animationTimer.Stop();
                    }
                }
                else
                {
                    this.animationTimer.Stop();
                }
            }

            this.Invalidate();
        }
    }
}
