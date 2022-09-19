using System;
using System.Drawing;
using System.Windows.Forms;

namespace NichiBox
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            box = Image.FromFile("res/box.png");
            this.customPictureBox1.Image = box;
            this.tbl = new TBL();
            this.webBrowser1.Navigate(this.UrlBox.Text);
            this.customPictureBox1.MouseWheel += CustomPictureBox1_MouseWheel;


        }
       
        //https://stackoverflow.com/questions/9616617/c-sharp-copy-paste-an-image-region-into-another-image
        private void CopyRegionIntoImage(Bitmap srcBitmap, Rectangle srcRegion, ref Bitmap destBitmap, Rectangle destRegion)
        {
            using (Graphics grD = Graphics.FromImage(destBitmap))
            {
                grD.DrawImage(srcBitmap, destRegion, srcRegion, GraphicsUnit.Pixel);

            }

        }
        private void GenerateBox()
        {
            Bitmap bitmap = new Bitmap(this.box);
            Rectangle srcRect = new Rectangle(0, 0, 20, 20);
            Rectangle destRect = new Rectangle(10, 2, 20, 20);
            int lineMax = 0;
           
            if (this.sourceText != null)
            {
                int lenString = this.sourceText.Length + 13;
                this.sourceText = "SPEAKER-NAME\n" + this.sourceText +"----";
                char[] array = this.sourceText.ToCharArray();
                for (int i = 0; i < lenString; i++)
                {
                    if (array[i] == '\n')
                    {
                        destRect.X = 10;
                        destRect.Y += 16;
                        if (lineMax > 40)
                        {
                            destRect.X = 10;
                            destRect.Y += 16;
                        }
                        lineMax = 0;
                    }
                    else if (array[i] == '.' && array[i+1] == '.' && array[i+2] == '.')// 3 period to elipsis
                    {
                        string fntName = string.Format("res/font/{0,0:d4}.png", this.tbl.chr['…']);
                        int index = this.tbl.chr['…'];
                        CopyRegionIntoImage(new Bitmap(Image.FromFile(fntName)), srcRect, ref bitmap, destRect);
                        destRect.X += this.tbl.width[index] * 2;
                        i += 2;
                    }
                    else
                    {
                        try
                        {
                            string fntName = string.Format("res/font/{0,0:d4}.png", this.tbl.chr[array[i]]);
                            int index = this.tbl.chr[array[i]];
                            CopyRegionIntoImage(new Bitmap(Image.FromFile(fntName)), srcRect, ref bitmap, destRect);
                            destRect.X += this.tbl.width[index] * 2;
                        }
                        catch (Exception)
                        {
                            CopyRegionIntoImage(new Bitmap(Image.FromFile("res/cross.png")), srcRect, ref bitmap, destRect);
                            destRect.X += 20;
                        }
                    }
                    lineMax ++;
                }
            }
            this.customPictureBox1.Image = bitmap;


        }
        private void CustomPictureBox1_MouseWheel(object sender,MouseEventArgs e)
        {
            int nHeight;
            int nWidth;
            if (e.Delta > 0)
            {
                nHeight = this.customPictureBox1.Height * 2;
                nWidth = this.customPictureBox1.Width * 2;
            }
            else
            {
                nHeight = this.customPictureBox1.Height / 2;
                nWidth = this.customPictureBox1.Width / 2;
            }
            if (nHeight >= 74)
            {
                this.customPictureBox1.Height = nHeight;
                this.customPictureBox1.Width = nWidth;
            }
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            this.UrlBox.Text = this.webBrowser1.Url.ToString();
            FindText();
        }
        
       private void FindText()
        {
          
            try   
            {
                HtmlElementCollection elements1 = this.webBrowser1.Document.GetElementsByTagName("div");
                HtmlElementCollection elements2 = elements1[5].GetElementsByTagName("div");
                HtmlElementCollection elements3 = elements2[12].GetElementsByTagName("div");
                HtmlElementCollection elements4 = elements3[0].GetElementsByTagName("div");
                HtmlElementCollection elements = elements4[10].All;
                foreach (HtmlElement element in elements)
                {
                    if (element.GetAttribute("className") == "translation-editor form-control highlight-editor")
                    {
                        if (element.InnerText != null)
                        {
                            this.sourceText = element.InnerText.Replace("\r\n", "\n");
                            GenerateBox();
                        }
                        
                    }
                }
            }
            catch (Exception)
            {

            }
          
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.webBrowser1.Navigate(this.UrlBox.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.webBrowser1.GoBack();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.webBrowser1.GoForward();

        }
        private void webBrowser1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey)
            {
                FindText();
            }
        }
    }
}
