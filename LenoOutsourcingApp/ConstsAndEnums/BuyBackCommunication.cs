using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EigenbelegToolAlpha.ConstsAndEnums
{
    public static class BuyBackCommunication
    {
        public static string receivedSimCardMessage = "Guten Tag,\r\n\r\nwir haben in Ihrem Gerät eine Sim Karte gefunden. " +
            "Wenn Sie diese benötigen ,melden Sie sich gerne! Für den Rückversand erheben wir 2,50€ per Paypal an: dange.businessebay@gmail.com" +
            "\r\n\r\nWenn wir nach einem Monat keine Rückmeldung erhalten, wird die Karte entsorgt.\r\n\r\n" +
            "Vielen Dank für Ihr Verständnis.\r\n\r\nMit freundlichen Grüßen\r\nIhr Leno Repair Team";

        public static string imeiNotIdentifiableMessage = "Guten Tag,\r\n\r\nder Auftrag wurde vorübergehend pausiert, da es nicht möglich ist" +
            "die IMEI mit herkömmlichen Methoden wie Anschalten des Geräts, über die Originalverpackung oder den Simschlitten zu ermitteln." +
            "\r\nDaher werden unsere technischen Mitarbeiter zeitnah, eine andere Möglichkeit finden, diese herauszufinden. Dies ist relevant, um" +
            "sicher zu gehen, dass keine Find My iPhone Sperre vorhanden ist.\r\n" +
            "Vielen Dank für Ihr Verständnis.\r\n\r\nMit freundlichen Grüßen\r\nIhr Leno Repair Team";
    }
}
