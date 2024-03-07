using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EigenbelegToolAlpha.ConstsAndEnums
{
    public class OnboardingInstructionsConsts
    {
        public static string lockedDeviceMessage = "Den Prozess bis Etikettdruck zuende machen und dann das Gerät mit aufgeklebtem Etikett in " +
                                                    "die Kiste 'Locked Devices' legen.\r\nWeitere Informationen Company Playbook/Logistics: Arrival Of Goods # not defined";
        public static string counterOfferMessage = "Für das Gerät muss ein Beweis festgehalten werden. Danach kommt es in die Kiste 'Gegenangebote'";
        public static string dfuResetNecessary = "Das Gerät an einen PC anschließen und über Tastenkombination zurücksetzen.";
        public static string imeiNotIdentifiable = "Das Gerät kommt in die Kiste 'High Prio'.";
        public static string receivedSimCard = "Die Sim Karte auf das ausgedruckte Etikett kleben und an die Simkarten Sammelstelle hängen.";

    }
}
