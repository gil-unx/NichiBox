using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace NichiBox
{
    //https://foxlearn.com/windows-forms/how-to-make-a-picturebox-move-in-csharp-455.html
    internal class CustomPictureBox : PictureBox
    {
        public CustomPictureBox(IContainer container)
        {
            container.Add(this);
        }

        Point point;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            point = e.Location;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - point.X;
                this.Top += e.Y - point.Y;
            }
        }

    }
}
