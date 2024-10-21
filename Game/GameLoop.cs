using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EscapeRoomGame.Helpers;

namespace EscapeRoomGame.Game
{
    public class GameLoop
    {
        public static void MainGameLoop()
        {
            int moolahToWin = 1000;
            Variables var = new Variables();
            PrintHelper.PrintCentered("════════════════════════════════════════════════════════════════════════════");
            PrintHelper.PrintCentered("Velkommen til det tekstbaserede escape-room!");
            PrintHelper.PrintCentered("Balancer din indtægt (Moolah) og stress for at undgå mavesår og fiasko!");
            PrintHelper.PrintCentered("════════════════════════════════════════════════════════════════════════════\n");

            while (var.Stress < 100)
            {
                // Tjek for sejr
                if (var.Moolah >= moolahToWin)
                {
                    PrintHelper.PrintCentered("\nTillykke! Du har tjent " + var.Moolah + " Moolah og vundet spillet! Du er nu en Top-G!");
                    PrintHelper.PrintCentered("════════════════════════════════════════════════════════════════════════════");
                    return; // Afslut spillet, når spilleren vinder
                }

                // Dag og status oversigt
                PrintHelper.PrintCentered("════════════════════════════════════════════════════════════════════════════");
                PrintHelper.PrintCentered("Dag " + (var.DaysWorked + 1));
                PrintHelper.PrintCentered("Moolah: " + var.Moolah + " | Stress: " + var.Stress + "% | Status: " + var.Status);
                PrintHelper.PrintCentered("════════════════════════════════════════════════════════════════════════════");

                // Menu
                PrintHelper.PrintCentered("Hvad vil du gøre?");
                PrintHelper.PrintCentered("1. Arbejd for at tjene penge");
                PrintHelper.PrintCentered("2. Invester for at opgradere din status");
                PrintHelper.PrintCentered("3. Se din inventory og brug items");
                PrintHelper.PrintCentered("4. Tag en pause (Reducer stress)");
                PrintHelper.PrintCentered("5. Afslut spillet");

                Console.Write("\nVælg handling: ");
                string valg = Console.ReadLine();

                switch (valg)
                {
                    case "1":
                        Console.Clear();
                        Arbejd(var);
                        break;
                    case "2":
                        Console.Clear();

                        Invester(var);
                        break;
                    case "3":
                        Console.Clear();

                        SeInventoryOgBrugItem(var);
                        break;
                    case "4":
                        Console.Clear();

                        ReducerStress(var);
                        break;
                    case "5":
                        Console.Clear();

                        Console.WriteLine("Du har valgt at afslutte spillet.");
                        return;
                    default:
                        Console.Clear();

                        PrintHelper.PrintCentered("Ugyldigt valg. Prøv igen.");
                        break;
                }

                TilfaeldigBegivenhed(var); // Kan udløse tilfældige events efter hver dag.
            }

            // Game over, hvis stress når 100
            PrintHelper.PrintCentered("GAME OVER! Du fik mavesår af stress og måtte stoppe.");
            PrintHelper.PrintCentered("════════════════════════════════════════════════════════════════════════════");
        }

        // Arbejdsfunktion (tjen penge, men øger stress)
        public static void Arbejd(Variables var)
        {
            int pengeTjent = 10 * var.Status; // Højere status = flere penge
            if (var.Inventory.Contains("Lucky Charm"))
            {
                pengeTjent += 20; // Lucky Charm bonus
            }

            var.Moolah += pengeTjent;
            var.Stress += 15; // Arbejde øger stress
            var.DaysWorked++;

            if (var.Inventory.Contains("Guldur"))
            {
                var.Moolah += pengeTjent / 2; // Guldur giver ekstra 50% for én arbejdsdag
                var.Inventory.Remove("Guldur"); // Fjernes efter brug
                PrintHelper.PrintCentered("\nDu brugte dit Guldur og tjente ekstra " + pengeTjent / 2 + " Moolah!");
            }

            PrintHelper.PrintCentered("\nDu arbejdede og tjente " + pengeTjent + " Moolah! Stress er nu " + var.Stress + "%");
        }

        // Investering (brug penge for at forbedre status)
        public static void Invester(Variables var)
        {
            if (var.Moolah >= 50)
            {
                var.Moolah -= 50;
                var.Status++;
                PrintHelper.PrintCentered("\nDu investerede 50 Moolah og øgede din status til " + var.Status + "!");
            }
            else
            {
                PrintHelper.PrintCentered("\nDu har ikke nok Moolah til at investere.");
            }
        }

        // Se inventory og brug items
        public static void SeInventoryOgBrugItem(Variables var)
        {
            if (var.Inventory.Count > 0)
            {
                PrintHelper.PrintCentered("\nDin inventory indeholder:");
                for (int i = 0; i < var.Inventory.Count; i++)
                {
                    PrintHelper.PrintCentered(i + 1 + ". " + var.Inventory[i]);
                }
                Console.Write("\nIndtast nummeret på et item for at bruge det, eller tryk Enter for at gå tilbage: ");
                string input = Console.ReadLine();

                if (int.TryParse(input, out int itemIndex) && itemIndex > 0 && itemIndex <= var.Inventory.Count)
                {
                    string valgtItem = var.Inventory[itemIndex - 1];
                    var.ItemBuffs[valgtItem](); // Udfør buffen for det valgte item
                    var.Inventory.Remove(valgtItem); // Fjern item efter brug
                    PrintHelper.PrintCentered("\nDu brugte " + valgtItem + ".");
                }
            }
            else
            {
                PrintHelper.PrintCentered("\nDin inventory er tom.");
            }
        }

        // Reducer stress (tag en pause)
        public static void ReducerStress(Variables var)
        {
            if (var.Stress >= 20)
            {
                var.Stress -= 20;
                PrintHelper.PrintCentered("\nDu tog en pause og reducerede din stress til " + var.Stress + "%");
            }
            else
            {
                var.Stress = 0;
                PrintHelper.PrintCentered("\nDin stress er nu helt væk! Tid til at arbejde igen!");
            }
        }

        // Tilfældige begivenheder (som i Monopoly)
        public static void TilfaeldigBegivenhed(Variables var)
        {
            Random rnd = new Random();
            int begivenhed = rnd.Next(1, 7);

            switch (begivenhed)
            {
                case 1:
                    PrintHelper.PrintCentered("\nEn kunde gav dig drikkepenge! Du tjente 20 ekstra Moolah!");
                    var.Moolah += 20;
                    break;
                case 2:
                    PrintHelper.PrintCentered("\nÅh nej, din computer gik i stykker! Du skal betale 30 Moolah for reparation.");
                    if (var.Moolah >= 30) var.Moolah -= 30; else var.Moolah = 0;
                    break;
                case 3:
                    PrintHelper.PrintCentered("\nDu fik en fantastisk idé og øgede din status gratis!");
                    var.Status++;
                    break;
                case 4:
                    PrintHelper.PrintCentered("\nDu blev inviteret til et stressende møde, og din stress steg med 10%.");
                    var.Stress += 10;
                    break;
                case 5:
                    PrintHelper.PrintCentered("\nDu fandt en Energidrik! Den er tilføjet til din inventory.");
                    var.Inventory.Add("Energidrik");
                    break;
                case 6:
                    PrintHelper.PrintCentered("\nDu vandt en Business Taktik Bog! Den er tilføjet til din inventory.");
                    var.Inventory.Add("Business Taktik Bog");
                    break;
            }
        }

        // Opret items og deres buffs
        public static void InitItems(Variables var)
        {
            var.ItemBuffs.Add("Energidrik", () =>
            {
                var.Stress -= 10;
                if (var.Stress < 0) var.Stress = 0;
                PrintHelper.PrintCentered("Din stress er reduceret med 10%");
            });

            var.ItemBuffs.Add("Business Taktik Bog", () =>
            {
                var.Status++;
                PrintHelper.PrintCentered("Din status er øget med 1");
            });

            var.ItemBuffs.Add("Lucky Charm", () =>
            {
                PrintHelper.PrintCentered("Lucky Charm aktiveret! Du får +20 Moolah pr. arbejdsdag.");
            });

            var.ItemBuffs.Add("Stressbold", () =>
            {
                var.Stress -= 20;
                if (var.Stress < 0) var.Stress = 0;
                PrintHelper.PrintCentered("Din stress er reduceret med 20%");
            });

            var.ItemBuffs.Add("Guldur", () =>
            {
                PrintHelper.PrintCentered("Guldur aktiveret! Næste arbejdsdag tjener du 50% ekstra Moolah.");
            });
        }
    }
}
