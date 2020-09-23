using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace send_mail
{
    public partial class waitForm : Form
    {
        public Action Worker { get; set; }

        public waitForm(Action pWorker, string pProceso)
        {
            InitializeComponent();

            lblProceso.Text = pProceso;

            if (pWorker == null)
            {
                throw new ArgumentNullException();
            }

            Worker = pWorker;

            CheckForIllegalCrossThreadCalls = false;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Task.Factory.StartNew(Worker)
                .ContinueWith(t => { this.Close(); }, TaskScheduler.FromCurrentSynchronizationContext());
        }

    }
}
