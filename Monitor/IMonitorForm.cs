using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Monitor
{
    public class MonitorForm : Form
    {
        public MonitorForm(object owner, params object[] args)
            : base()
        {
            this.owner = owner;
            this.args = args;
        }

        public virtual void CloseThisSubForm()
        {
        }

        object owner;

        public object TheOwner
        {
            get { return owner; }
            set { owner = value; }
        }
        object[] args;

        public object[] TheArgs
        {
            get { return args; }
            set { args = value; }
        }
    }
}
