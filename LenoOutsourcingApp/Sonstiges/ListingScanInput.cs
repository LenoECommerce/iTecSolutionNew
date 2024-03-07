using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Microsoft.FSharp.Core.ByRefKinds;

namespace EigenbelegToolAlpha
{
    public partial class ListingScanInput : Form
    {
        public static string scan = "";
        
        public ListingScanInput()
        {
            InitializeComponent();
        }

        private void ListingScanInput_Load(object sender, EventArgs e)
        {

        }
        public void OnKeyDownHandler(object sender, KeyEventArgs kea)
        {
            if (kea.KeyCode.Equals(Keys.Return))
            {
                scan = AdaptYZConfusion(textBox_scanInput.Text);
                this.DialogResult = DialogResult.OK;
                this.Hide();
            }
        }
        public string AdaptYZConfusion(string input)
        {
            char[] chars = input.ToCharArray();

            for (int i = 0; i < chars.Length; i++)
            {
                if (chars[i] == 'Z')
                {
                    chars[i] = 'Y';
                }
                else if (chars[i] == 'Y')
                {
                    chars[i] = 'Z';
                }
            }

            return new string(chars);
        }
        private void textBox_scanInput_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
