using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Controls;

namespace systemLab5.models
{
    public delegate bool ValidateBoxList(string Text);

    public class NumericBox : TextBox
    {
        private ValidateBoxList Validator;

        public void RegisterValidatorDelegate(ValidateBoxList _validator)
        {
            Validator += _validator;
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            if (!int.TryParse(Text, out _))
            {
                if (Text.Length > 1)
                    Text = Text[..^1];
                else
                    Text = "";

            }
            else
            {
                if (Validator != null)
                {
                    if (!Validator.Invoke(Text))
                    {
                        if (Text.Length > 1)
                            Text = Text[..^1];
                        else
                            Text = "";
                    }
                }
            }



            base.OnTextChanged(e);
        }

        public bool Valid { get => !string.IsNullOrEmpty(Text); }

        public override string ToString()
        {
            return Text;
        }
    }
}
