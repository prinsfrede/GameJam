using System;
using System.Collections.Generic;

class EscapeRoomGame
{
    // Spillervariabler
    public static int moolah = 50; // Start penge
    public static int stress = 0;
    public static int status = 1;
    public static int daysWorked = 0;
    public static int moolahToWin = 1000; // Mængden af penge for at vinde

    // Inventory og investeringer
    public static List<string> inventory = new List<string>();
    public static Dictionary<string, Action> itemBuffs = new Dictionary<string, Action>(); // Items med buffs

    // Hovedspil loop
    static void Main(string[] args)
    {
        // Opret buff items
        InitItems();

        // Indledning
        PrintCentered("════════════════════════════════════════════════════════════════════════════");
        PrintCentered("Velkommen til det tekstbaserede escape-room!");
        PrintCentered("Balancer din indtægt (Moolah) og stress for at undgå mavesår og fiasko!");
        PrintCentered("════════════════════════════════════════════════════════════════════════════\n");

        while (stress < 100)
        {
            // Tjek for sejr
            if (moolah >= moolahToWin)
            {
                PrintCentered("\nTillykke! Du har tjent " + moolah + " Moolah og vundet spillet! Du er nu en Top-G!");
                PrintCentered("════════════════════════════════════════════════════════════════════════════");
                return; // Afslut spillet, når spilleren vinder
            }

            // Dag og status oversigt
            PrintCentered("════════════════════════════════════════════════════════════════════════════");
            PrintCentered("Dag " + (daysWorked + 1));
            PrintCentered("Moolah: " + moolah + " | Stress: " + stress + "% | Status: " + status);
            PrintCentered("════════════════════════════════════════════════════════════════════════════");

            // Menu
            PrintCentered("Hvad vil du gøre?");
            PrintCentered("1. Arbejd for at tjene penge");
            PrintCentered("2. Invester for at opgradere din status");
            PrintCentered("3. Se din inventory og brug items");
            PrintCentered("4. Tag en pause (Reducer stress)");
            PrintCentered("5. Afslut spillet");

            Console.Write("\nVælg handling: ");
            string valg = Console.ReadLine();

            switch (valg)
            {
                case "1":
                    Console.Clear();
                    Arbejd();
                    break;
                case "2":
                    Console.Clear();

                    Invester();
                    break;
                case "3":
                    Console.Clear();

                    SeInventoryOgBrugItem();
                    break;
                case "4":
                    Console.Clear();

                    ReducerStress();
                    break;
                case "5":
                    Console.Clear();

                    Console.WriteLine("Du har valgt at afslutte spillet.");
                    return;
                default:
                    Console.Clear();

                    PrintCentered("Ugyldigt valg. Prøv igen.");
                    break;
            }

            TilfaeldigBegivenhed(); // Kan udløse tilfældige events efter hver dag.
        }

        // Game over, hvis stress når 100
        PrintCentered("GAME OVER! Du fik mavesår af stress og måtte stoppe.");
        PrintCentered("════════════════════════════════════════════════════════════════════════════");
    }

    // Arbejdsfunktion (tjen penge, men øger stress)
    public static void Arbejd()
    {
        int pengeTjent = 10 * status; // Højere status = flere penge
        if (inventory.Contains("Lucky Charm"))
        {
            pengeTjent += 20; // Lucky Charm bonus
        }

        moolah += pengeTjent;
        stress += 15; // Arbejde øger stress
        daysWorked++;

        if (inventory.Contains("Guldur"))
        {
            moolah += pengeTjent / 2; // Guldur giver ekstra 50% for én arbejdsdag
            inventory.Remove("Guldur"); // Fjernes efter brug
            PrintCentered("\nDu brugte dit Guldur og tjente ekstra " + pengeTjent / 2 + " Moolah!");
        }

        PrintCentered("\nDu arbejdede og tjente " + pengeTjent + " Moolah! Stress er nu " + stress + "%");
    }

    // Investering (brug penge for at forbedre status)
    public static void Invester()
    {
        if (moolah >= 50)
        {
            moolah -= 50;
            status++;
            PrintCentered("\nDu investerede 50 Moolah og øgede din status til " + status + "!");
        }
        else
        {
            PrintCentered("\nDu har ikke nok Moolah til at investere.");
        }
    }

    // Se inventory og brug items
    public static void SeInventoryOgBrugItem()
    {
        if (inventory.Count > 0)
        {
            PrintCentered("\nDin inventory indeholder:");
            for (int i = 0; i < inventory.Count; i++)
            {
                PrintCentered((i + 1) + ". " + inventory[i]);
            }
            Console.Write("\nIndtast nummeret på et item for at bruge det, eller tryk Enter for at gå tilbage: ");
            string input = Console.ReadLine();

            if (int.TryParse(input, out int itemIndex) && itemIndex > 0 && itemIndex <= inventory.Count)
            {
                string valgtItem = inventory[itemIndex - 1];
                itemBuffs[valgtItem](); // Udfør buffen for det valgte item
                inventory.Remove(valgtItem); // Fjern item efter brug
                PrintCentered("\nDu brugte " + valgtItem + ".");
            }
        }
        else
        {
            PrintCentered("\nDin inventory er tom.");
        }
    }

    // Reducer stress (tag en pause)
    public static void ReducerStress()
    {
        if (stress >= 20)
        {
            stress -= 20;
            PrintCentered("\nDu tog en pause og reducerede din stress til " + stress + "%");
        }
        else
        {
            stress = 0;
            PrintCentered("\nDin stress er nu helt væk! Tid til at arbejde igen!");
        }
    }

    // Tilfældige begivenheder (som i Monopoly)
    public static void TilfaeldigBegivenhed()
    {
        Random rnd = new Random();
        int begivenhed = rnd.Next(1, 7);

        switch (begivenhed)
        {
            case 1:
                PrintCentered("\nEn kunde gav dig drikkepenge! Du tjente 20 ekstra Moolah!");
                moolah += 20;
                break;
            case 2:
                PrintCentered("\nÅh nej, din computer gik i stykker! Du skal betale 30 Moolah for reparation.");
                if (moolah >= 30) moolah -= 30; else moolah = 0;
                break;
            case 3:
                PrintCentered("\nDu fik en fantastisk idé og øgede din status gratis!");
                status++;
                break;
            case 4:
                PrintCentered("\nDu blev inviteret til et stressende møde, og din stress steg med 10%.");
                stress += 10;
                break;
            case 5:
                PrintCentered("\nDu fandt en Energidrik! Den er tilføjet til din inventory.");
                inventory.Add("Energidrik");
                break;
            case 6:
                PrintCentered("\nDu vandt en Business Taktik Bog! Den er tilføjet til din inventory.");
                inventory.Add("Business Taktik Bog");
                break;
        }
    }

    // Opret items og deres buffs
    public static void InitItems()
    {
        itemBuffs.Add("Energidrik", () => {
            stress -= 10;
            if (stress < 0) stress = 0;
            PrintCentered("Din stress er reduceret med 10%");
        });

        itemBuffs.Add("Business Taktik Bog", () => {
            status++;
            PrintCentered("Din status er øget med 1");
        });

        itemBuffs.Add("Lucky Charm", () => {
            PrintCentered("Lucky Charm aktiveret! Du får +20 Moolah pr. arbejdsdag.");
        });

        itemBuffs.Add("Stressbold", () => {
            stress -= 20;
            if (stress < 0) stress = 0;
            PrintCentered("Din stress er reduceret med 20%");
        });

        itemBuffs.Add("Guldur", () => {
            PrintCentered("Guldur aktiveret! Næste arbejdsdag tjener du 50% ekstra Moolah.");
        });
    }

    // Metode til at centrere tekst i konsollen
    public static void PrintCentered(string text)
    {
        int windowWidth = Console.WindowWidth;
        int textLength = text.Length;
        int spaces = (windowWidth - textLength) / 2;
        Console.WriteLine(new string(' ', spaces) + text);
    }
}



