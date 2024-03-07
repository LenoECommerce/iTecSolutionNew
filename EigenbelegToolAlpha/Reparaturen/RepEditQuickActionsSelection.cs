using EigenbelegToolAlpha.ConstsAndEnums;
using System;
using System.Windows.Forms;

namespace EigenbelegToolAlpha
{
    public partial class RepEditQuickActionsSelection : Form
    {
        private static string repNumber = Reparaturen.internalNumber;
        public string contentForCommentBox = "";
        public string[] files = { };

        private readonly RepEditQuickActions _repEditQuickActions;

        public RepEditQuickActionsSelection()
        {
            _repEditQuickActions = new RepEditQuickActions();
            InitializeComponent();
        }
        public static void ShowMessageBoxOnboarding(string message)
        {
            if (UserFileManagement.CheckIfUserOnboarding())
            {
                MessageBox.Show(message);
            }
        }

        private void SetOrderId()
        {
            repNumber = Reparaturen.internalNumber;
        }

        private void CloseWindow()
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SetOrderId();
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
                    _repEditQuickActions.PrintCounterOffer(reason, repNumber);
                    ShowMessageBoxOnboarding(OnboardingInstructionsConsts.counterOfferMessage);
                }
            }
        }

        private void btn_DfuResetNecessary_Click(object sender, EventArgs e)
        {
            SetOrderId();
            _repEditQuickActions.PrintDFUResetNecessary(repNumber);
            ShowMessageBoxOnboarding(OnboardingInstructionsConsts.dfuResetNecessary);
            CloseWindow();
        }

        private void RepEditQuickActionsSelection_Load(object sender, EventArgs e)
        {

        }

        private void btn_lockedDevice_Click(object sender, EventArgs e)
        {
            SetOrderId();
            contentForCommentBox = "FMI On";
            _repEditQuickActions.PrintLockedDevice(repNumber);
            string[] reasons = new string[] { "customer_account_present" };
            var buybackOrderId = _repEditQuickActions.GetBuyBackOrderIdViaREPNumber(repNumber);
            BackMarketAPIHandler.SuspendBuyBackOrder(reasons, buybackOrderId);
            ShowMessageBoxOnboarding(OnboardingInstructionsConsts.lockedDeviceMessage);
            CloseWindow();
        }

        private void btn_ReceivedSimCard_Click(object sender, EventArgs e)
        {
            SetOrderId();
            _repEditQuickActions.PrintReceivedSimCard(repNumber);
            var buybackOrderId = _repEditQuickActions.GetBuyBackOrderIdViaREPNumber(repNumber);
            BackMarketAPIHandler.SendMessageToBuyBackCustomer(
                BuyBackCommunication.receivedSimCardMessage, 
                files, 
                buybackOrderId);
            ShowMessageBoxOnboarding(OnboardingInstructionsConsts.receivedSimCard);
            CloseWindow();
        }

        private void btn_IMEINotIdentifiable_Click(object sender, EventArgs e)
        {
            SetOrderId();
            _repEditQuickActions.PrintIMEINotRecognizable(repNumber);
            contentForCommentBox = "IMEI nicht identifizierbar";

            string[] reasons = new string[] { "customer_account_present" };
            var buybackOrderId = _repEditQuickActions.GetBuyBackOrderIdViaREPNumber(repNumber);
            BackMarketAPIHandler.SuspendBuyBackOrder(reasons, buybackOrderId);
            BackMarketAPIHandler.SendMessageToBuyBackCustomer(
                BuyBackCommunication.imeiNotIdentifiableMessage, 
                files, 
                buybackOrderId);
            ShowMessageBoxOnboarding(OnboardingInstructionsConsts.imeiNotIdentifiable);
            CloseWindow();
        }

        private void btn_nonFunctionalGradeButNoIssueFound_Click(object sender, EventArgs e)
        {
            SetOrderId();
            contentForCommentBox = "Keine technischen Fehler gefunden\r\nKundenantwort ausstehend";
            var buybackOrderId = _repEditQuickActions.GetBuyBackOrderIdViaREPNumber(repNumber);
            BackMarketAPIHandler.SendMessageToBuyBackCustomer(
                BuyBackCommunication.nonFunctionalGradeErrorFoundMessage,
                files, 
                buybackOrderId);
            CloseWindow();
        }
    }
}
