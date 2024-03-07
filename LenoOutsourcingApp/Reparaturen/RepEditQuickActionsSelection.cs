using EigenbelegToolAlpha.ConstsAndEnums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EigenbelegToolAlpha
{
    public partial class RepEditQuickActionsSelection : Form
    {
        private static string repNumber = Reparaturen.internalNumber;
        public string contentForCommentBox = "";
        private string buybackOrderId = RepEditQuickActions.GetBuyBackOrderIdViaREPNumber(repNumber);
        public RepEditQuickActionsSelection()
        {
            InitializeComponent();
        }
        public static void ShowMessageBoxOnboarding(string message)
        {
            if (UserFileManagement.CheckIfUserOnboarding())
            {
                MessageBox.Show(message);
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            repNumber = Reparaturen.internalNumber;
            buybackOrderId = RepEditQuickActions.GetBuyBackOrderIdViaREPNumber(repNumber);
            //counter offer stuff
            using (var form = new RepEditCounterOfferInputReason())
            {
                var result = form.ShowDialog();
                this.DialogResult = DialogResult.OK;
                this.Close();
                if (result == DialogResult.OK)
                {
                    string reason = form.counterOfferReason;
                    contentForCommentBox = reason;
                    RepEditQuickActions.PrintCounterOffer(reason, repNumber);
                    ShowMessageBoxOnboarding(OnboardingInstructionsConsts.counterOfferMessage);
                }
            }
        }

        private void btn_DfuResetNecessary_Click(object sender, EventArgs e)
        {
            repNumber = Reparaturen.internalNumber;
            buybackOrderId = RepEditQuickActions.GetBuyBackOrderIdViaREPNumber(repNumber);
            RepEditQuickActions.PrintDFUResetNecessary(repNumber);
            ShowMessageBoxOnboarding(OnboardingInstructionsConsts.dfuResetNecessary);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void RepEditQuickActionsSelection_Load(object sender, EventArgs e)
        {

        }

        private void btn_lockedDevice_Click(object sender, EventArgs e)
        {
            repNumber = Reparaturen.internalNumber;
            buybackOrderId = RepEditQuickActions.GetBuyBackOrderIdViaREPNumber(repNumber);
            RepEditQuickActions.PrintLockedDevice(repNumber);
            string[] reasons = new string[] { "customer_account_present" };
            BackMarketAPIHandler.SuspendBuyBackOrder(reasons, buybackOrderId);
            ShowMessageBoxOnboarding(OnboardingInstructionsConsts.lockedDeviceMessage);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btn_ReceivedSimCard_Click(object sender, EventArgs e)
        {
            repNumber = Reparaturen.internalNumber;
            buybackOrderId = RepEditQuickActions.GetBuyBackOrderIdViaREPNumber(repNumber);
            RepEditQuickActions.PrintReceivedSimCard(repNumber);
            string[] files = { };
            BackMarketAPIHandler.SendMessageToBuyBackCustomer(BuyBackCommunication.receivedSimCardMessage, files, buybackOrderId);
            ShowMessageBoxOnboarding(OnboardingInstructionsConsts.receivedSimCard);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btn_IMEINotIdentifiable_Click(object sender, EventArgs e)
        {
            repNumber = Reparaturen.internalNumber;
            buybackOrderId = RepEditQuickActions.GetBuyBackOrderIdViaREPNumber(repNumber);
            RepEditQuickActions.PrintIMEINotRecognizable(repNumber);
            contentForCommentBox = "IMEI nicht identifizierbar";

            string[] reasons = new string[] { "customer_account_present" };
            BackMarketAPIHandler.SuspendBuyBackOrder(reasons, buybackOrderId);
            string[] files = { };
            BackMarketAPIHandler.SendMessageToBuyBackCustomer(BuyBackCommunication.imeiNotIdentifiableMessage, files, buybackOrderId);
            ShowMessageBoxOnboarding(OnboardingInstructionsConsts.imeiNotIdentifiable);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
