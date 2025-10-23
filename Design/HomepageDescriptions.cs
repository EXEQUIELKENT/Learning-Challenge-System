using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace Design
{
    public class ChallengeDescriptions
    {
        // Dictionary for mapping images to unique identifiers
        public Dictionary<Image, string> ImageIdentifiers { get; private set; }

        // Dictionary for mapping identifiers to descriptions
        public Dictionary<string, string> ImageDescriptions { get; private set; }

        public List<Image> Images { get; private set; }

        public ChallengeDescriptions()
        {
            {// Initialize images from .resx with unique identifiers
                ImageIdentifiers = new Dictionary<Image, string>
            {
            //Math
            { DCP.Properties.Resources.Budget_Problem__Easy_M, "Budget_Problem__Easy_M" },
            { DCP.Properties.Resources.Budget_Problem__Medium_M, "Budget_Problem__Medium_M" },
            { DCP.Properties.Resources.Budget_Problem__Hard_M, "Budget_Problem__Hard_M" },
            { DCP.Properties.Resources.Pattern_Recognition__Easy_M, "Pattern_Recognition__Easy_M" },
            { DCP.Properties.Resources.Pattern_Recognition__Medium_M, "Pattern_Recognition__Medium_M" },
            { DCP.Properties.Resources.Pattern_Recognition__Hard_M, "Pattern_Recognition__Hard_M" },
            { DCP.Properties.Resources.Real_Life_Application__Easy_M, "Real_Life_Application__Easy_M" },
            { DCP.Properties.Resources.Real_Life_Application__Medium_M, "Real_Life_Application__Medium_M" },
            { DCP.Properties.Resources.Real_Life_Application__Hard_M, "Real_Life_Application__Hard_M" },
            { DCP.Properties.Resources.Math_Puzzle__Easy_M, "Math_Puzzle__Easy_M" },
            { DCP.Properties.Resources.Math_Puzzle__Medium_M, "Math_Puzzle__Medium_M" },
            { DCP.Properties.Resources.Math_Puzzle__Hard_M, "Math_Puzzle__Hard_M" },
            { DCP.Properties.Resources.Time_Challenge__Easy_M, "Time_Challenge__Easy_M" },
            { DCP.Properties.Resources.Time_Challenge__Medium_M, "Time_Challenge__Medium_M" },
            { DCP.Properties.Resources.Time_Challenge__Hard_M, "Time_Challenge__Hard_M" },
            { DCP.Properties.Resources.Addition_Easy_M, "Addition_Easy_M" },
            { DCP.Properties.Resources.Addition_Medium_M, "Addition_Medium_M" },
            { DCP.Properties.Resources.Addition_Hard_M, "Addition_Hard_M" },
            { DCP.Properties.Resources.Subtraction_Easy_M, "Subtraction_Easy_M" },
            { DCP.Properties.Resources.Subtraction_Medium_M, "Subtraction_Medium_M" },
            { DCP.Properties.Resources.Subtraction_Hard_M, "Subtraction_Hard_M" },
            { DCP.Properties.Resources.Multiplication_Easy_M, "Multiplication_Easy_M" },
            { DCP.Properties.Resources.Multiplication_Medium_M, "Multiplication_Medium_M" },
            { DCP.Properties.Resources.Multiplication_Hard_M, "Multiplication_Hard_M" },
            { DCP.Properties.Resources.Division_Easy_M, "Division_Easy_M" },
            { DCP.Properties.Resources.Division_Medium_M, "Division_Medium_M" },
            { DCP.Properties.Resources.Division_Hard_M, "Division_Hard_M" },

            };
                Images = new List<Image>(ImageIdentifiers.Keys);
            }




            // Initialize descriptions for each identifier
            ImageDescriptions = new Dictionary<string, string>
            {
                //Math
                { "Budget_Problem__Easy_M", GenerateDescription("Budget_Problem__Easy_M") },
                { "Budget_Problem__Medium_M", GenerateDescription("Budget_Problem__Medium_M") },
                { "Budget_Problem__Hard_M", GenerateDescription("Budget_Problem__Hard_M") },
                { "Pattern_Recognition__Easy_M", GenerateDescription("Pattern_Recognition__Easy_M") },
                { "Pattern_Recognition__Medium_M", GenerateDescription("Pattern_Recognition__Medium_M") },
                { "Pattern_Recognition__Hard_M", GenerateDescription("Pattern_Recognition__Hard_M") },
                { "Real_Life_Application__Easy_M", GenerateDescription("Real_Life_Application__Easy_M") },
                { "Real_Life_Application__Medium_M", GenerateDescription("Real_Life_Application__Medium_M") },
                { "Real_Life_Application__Hard_M", GenerateDescription("Real_Life_Application__Hard_M") },
                { "Math_Puzzle__Easy_M", GenerateDescription("Math_Puzzle__Easy_M") },
                { "Math_Puzzle__Medium_M", GenerateDescription("Math_Puzzle__Medium_M") },
                { "Math_Puzzle__Hard_M", GenerateDescription("Math_Puzzle__Hard_M") },
                { "Time_Challenge__Easy_M", GenerateDescription("Time_Challenge__Easy_M") },
                { "Time_Challenge__Medium_M", GenerateDescription("Time_Challenge__Medium_M") },
                { "Time_Challenge__Hard_M", GenerateDescription("Time_Challenge__Hard_M") },
                { "Addition_Easy_M", GenerateDescription("Addition_Easy_M") },
                { "Addition_Medium_M", GenerateDescription("Addition_Medium_M") },
                { "Addition_Hard_M", GenerateDescription("Addition_Hard_M") },
                { "Subtraction_Easy_M", GenerateDescription("Subtraction_Easy_M") },
                { "Subtraction_Medium_M", GenerateDescription("Subtraction_Medium_M") },
                { "Subtraction_Hard_M", GenerateDescription("Subtraction_Hard_M") },
                { "Multiplication_Easy_M", GenerateDescription("Multiplication_Easy_M") },
                { "Multiplication_Medium_M", GenerateDescription("Multiplication_Medium_M") },
                { "Multiplication_Hard_M", GenerateDescription("Multiplication_Hard_M") },
                { "Division_Easy_M", GenerateDescription("Division_Easy_M") },
                { "Division_Medium_M", GenerateDescription("Division_Medium_M") },
                { "Division_Hard_M", GenerateDescription("Division_Hard_M") }
                // Additional descriptions here...
            };

        }
        private string GenerateDescription(string identifier)
        {
            // Sample long description, modify as needed
            switch (identifier)
            {
                //New Challenges
                case "Addition_Easy_M":
                    return "                ADDITION (EASY)\n\n" +
                      "Description:\n" +
                      "Sa antas na ito, ipinapakilala sa mga mag-aaral ang konsepto ng pagdaragdag sa madadaling bilang. Layunin nitong mapalakas ang kanilang kakayahan sa pagbilang at paggamit ng tamang paraan ng pagdaragdag. Nakatuon ito sa pag-unawa sa ugnayan ng mga bilang sa isang simple at masayang paraan.";

                case "Addition_Medium_M":
                    return "                ADDITION (MEDIUM)\n\n" +
                      "Description:\n" +
                      "Sa antas na ito, mas pinapalalim ang kakayahan ng mag-aaral sa pagdaragdag ng mas malalaking bilang. Tinuturuan silang mag-isip nang mas kritikal at gumamit ng mga estratehiya upang makuha ang tamang sagot. Layunin nitong mapaunlad ang bilis at katumpakan sa pag-compute.";

                case "Addition_Hard_M":
                    return "                ADDITION (HARD)\n\n" +
                      "Ang antas na ito ay para sa mga mag-aaral na bihasa na sa pagdaragdag. Nakatuon ito sa mas kumplikadong bilang at sitwasyon na nangangailangan ng masusing pagsusuri. Pinauunlad nito ang kakayahan sa problem solving at lohikal na pag-iisip.";

                case "Subtraction_Easy_M":
                    return "                SUBTRACTION (EASY)\n\n" +
                      "Sa antas na ito, ipinapakilala ang konsepto ng pagbabawas gamit ang simpleng bilang. Tinuturuan ang mga mag-aaral kung paano bawasan ang isang bilang mula sa isa pa upang maunawaan ang ideya ng pagkakaiba. Layunin nitong sanayin ang kanilang pag-unawa sa basic subtraction.";

                case "Subtraction_Medium_M":
                    return "                SUBTRACTION (MEDIUM)\n\n" +
                      "Sa antas na ito, mas pinaiigting ang kakayahan ng mag-aaral sa pagbabawas ng mas malalaking bilang. Pinapalakas nito ang kanilang kasanayan sa pagbilang pabalik at sa paggamit ng wastong pamamaraan upang makuha ang tamang sagot.";

                case "Subtraction_Hard_M":
                    return "                SUBTRACTION (HARD)\n\n" +
                      "Ang antas na ito ay nagpapakita ng mas mahihirap na gawain sa pagbabawas. Kabilang dito ang mga problemang nangangailangan ng pagpapahiram o regrouping. Layunin nitong hasain ang lohikal na pag-iisip at pagiging maingat sa pagkalkula.";

                case "Multiplication_Easy_M":
                    return "                MULTIPLICATION (EASY)\n\n" +
                      "Sa antas na ito, ipinapakilala ang ideya ng pag-uulit na pagdaragdag bilang batayan ng multiplication. Tinuturuan ang mga mag-aaral na maunawaan ang kahulugan ng bawat bilang at kung paano ito inuulit. Layunin nitong palakasin ang kanilang pundasyon sa pag-multiply.";

                case "Multiplication_Medium_M":
                    return "                MULTIPLICATION (MEDIUM)\n\n" +
                      "Sa antas na ito, mas pinaiigting ang kakayahan sa paggamit ng multiplication table. Pinauunlad ang bilis at katumpakan sa pagsagot, pati na rin ang kakayahan sa pag-unawa ng mas mahahabang bilang. Layunin nitong ihanda sila sa mas komplikadong gawain.";

                case "Multiplication_Hard_M":
                    return "                MULTIPLICATION (HARD)\n\n" +
                      "Ang antas na ito ay nakatuon sa mas malalaking bilang at word problems na nangangailangan ng tamang estratehiya. Pinapalakas nito ang kakayahan sa analytical thinking at paggamit ng multiplication sa totoong sitwasyon.";

                case "Division_Easy_M":
                    return "                DIVISION (EASY)\n\n" +
                      "Sa antas na ito, ipinapakilala ang konsepto ng paghahati ng bilang sa pantay-pantay na bahagi. Tinuturuan ang mga mag-aaral na maunawaan ang relasyon ng division sa multiplication. Layunin nitong maging madali at masaya ang pagkatuto ng basic division.";

                case "Division_Medium_M":
                    return "                DIVISION (MEDIUM)\n\n" +
                      "Sa antas na ito, mas pinaiigting ang kakayahan sa paghahati ng mas malalaking bilang. Pinapalakas nito ang pag-unawa sa mga hakbang ng division at sa koneksyon nito sa iba pang operasyon. Tinuturuan din ang mag-aaral na maging mas mabilis at tumpak.";

                case "Division_Hard_M":
                    return "                DIVISION (HARD)\n\n" +
                      "Ang antas na ito ay para sa mga mag-aaral na bihasa na sa paghahati ng bilang. Kabilang dito ang mga problemang may natitirang sagot o remainder. Layunin nitong hasain ang kakayahan sa problem solving at aplikasyon ng division sa iba’t ibang sitwasyon.";

                case "Budget_Problem__Easy_M":
                    return "              PROBLEM SOLVING (EASY)\n\n" +
                      "Ang madali na antas ng Budget Problem ay nagpapakilala ng mga simpleng sitwasyon sa paggastos at pagba-budget. Layunin nito na matutunan ng mag-aaral kung paano magdesisyon sa paggamit ng pera at maunawaan ang mga pangunahing konsepto ng wastong pamamahala ng badyet.\n\n" +
                      "Sa antas na ito, natutulungan ang mga mag-aaral na magkaroon ng tiwala sa pagdedesisyon tungkol sa pera at matutunan ang tamang pagpaplano ng gastusin. Habang lumalahok sila, nagkakaroon din sila ng kamalayan sa kahalagahan ng pagtitipid at responsableng paggamit ng salapi.";

                case "Budget_Problem__Medium_M":
                    return "              PROBLEM SOLVING (MEDIUM)\n\n" +
                      "Sa katamtamang antas ng Budget Problem, haharap ang mag-aaral sa mas masalimuot na sitwasyon na may kasamang higit na desisyon at pagsusuri. Dito, kailangan nilang magplano at mag-analisa kung paano gagamitin ang pera sa tamang paraan.\n\n" +
                      "Layunin ng antas na ito na mapaunlad ang kakayahan sa kritikal na pag-iisip at tamang pagpapasya sa pinansyal na aspeto. Sa bawat pagsubok, natututo ang mag-aaral kung paano gumawa ng matalinong hakbang sa pagba-budget.";

                case "Budget_Problem__Hard_M":
                    return "              PROBLEM SOLVING (HARD)\n\n" +
                      "Ang mahirap na antas ng Budget Problem ay nagbibigay ng mas komplikadong sitwasyon sa pera na nangangailangan ng masusing pagsusuri at estratehikong pag-iisip. Dito, sinusubok ang kakayahan ng mag-aaral na magplano at magdesisyon sa mahihirap na sitwasyon.\n\n" +
                      "Sa pagsali dito, natututo silang maging matalino at maingat sa paggamit ng pera, at mas nauunawaan nila ang kahalagahan ng tamang pagba-budget para sa hinaharap.";

                case "Pattern_Recognition__Easy_M":
                    return "              PATTERN RECOGNITION (EASY)\n\n" +
                      "Ang madali na antas ng Pattern Recognition ay nagpapakilala sa mga mag-aaral ng mga simpleng pattern o pagkakasunod-sunod. Layunin nitong sanayin ang kanilang mata sa pagkilala ng mga paulit-ulit na bagay o ideya.\n\n" +
                      "Sa antas na ito, natutulungan silang magkaroon ng masusing obserbasyon at mas maunawaan kung paano nabubuo ang mga pattern sa araw-araw.";

                case "Pattern_Recognition__Medium_M":
                    return "              PATTERN RECOGNITION (MEDIUM)\n\n" +
                      "Sa katamtamang antas ng Pattern Recognition, mas pinapalalim ang pag-unawa ng mag-aaral sa mas komplikadong mga pattern. Dito, kinakailangan ng masusing pagsusuri at pag-iisip upang matukoy ang mga ugnayan o pagkakaiba sa mga datos o sitwasyon.\n\n" +
                      "Layunin nitong mapaunlad ang lohikal na pag-iisip at kakayahang mag-analisa sa mas malalim na antas.";

                case "Pattern_Recognition__Hard_M":
                    return "              PATTERN RECOGNITION (HARD)\n\n" +
                      "Ang mahirap na antas ng Pattern Recognition ay naglalaman ng masalimuot na mga pattern na nangangailangan ng mataas na antas ng pag-iisip at pagsusuri. Dito sinusubok ang kakayahan ng mag-aaral na mag-isip nang malalim at sistematiko.\n\n" +
                      "Sa pagsasanay na ito, natututo silang mag-analisa at maghanap ng lohikal na koneksyon sa mas komplikadong sitwasyon.";

                case "Real_Life_Application__Easy_M":
                    return "              REAL APPLICATION (EASY)\n\n" +
                      "Ang madali na antas ng Real Life Application ay nagtuturo kung paano gamitin ang mga natutunang konsepto sa totoong buhay. Layunin nitong ipakita sa mag-aaral na ang matematika ay may gamit sa pang-araw-araw na gawain.\n\n" +
                      "Sa antas na ito, natututo silang mag-isip praktikal at magamit ang kanilang kaalaman sa simpleng problema sa buhay.";

                case "Real_Life_Application__Medium_M":
                    return "              REAL APPLICATION (MEDIUM)\n\n" +
                      "Sa katamtamang antas ng Real Life Application, mas mahihirap na sitwasyon sa totoong buhay ang ipapakita. Kailangang gamitin ng mag-aaral ang kanilang pag-unawa at lohikal na pag-iisip upang makahanap ng solusyon.\n\n" +
                      "Layunin nito na mahasa ang kakayahan nilang mag-isip nang malalim at magdesisyon batay sa karanasan at kaalaman.";

                case "Real_Life_Application__Hard_M":
                    return "              REAL APPLICATION (HARD)\n\n" +
                      "Ang mahirap na antas ng Real Life Application ay nagpapakita ng masalimuot na sitwasyon na nangangailangan ng mataas na antas ng pag-aanalisa at estratehikong pag-iisip. Dito sinusubok ang kakayahan ng mag-aaral na ilapat ang natutunan sa tunay na mundo.\n\n" +
                      "Natutulungan silang maging mapanuri, malikhain, at handa sa mga komplikadong problema sa buhay.";

                case "Math_Puzzle__Easy_M":
                    return "              MATH PUZZLE (EASY)\n\n" +
                      "Ang madali na antas ng Math Puzzle ay naglalaman ng mga simpleng palaisipan sa matematika upang sanayin ang pag-iisip ng mag-aaral. Layunin nitong gawing masaya at nakakapanabik ang pag-aaral ng mga pangunahing konsepto sa math.\n\n" +
                      "Sa antas na ito, natututo silang mag-isip nang lohikal habang nalilinang ang kanilang kasanayan sa problem solving.";

                case "Math_Puzzle__Medium_M":
                    return "              MATH PUZZLE (MEDIUM)\n\n" +
                      "Sa katamtamang antas ng Math Puzzle, mas pinapalalim ang mga palaisipan na kailangang pag-isipan at pag-ugnayin. Dito, sinusubok ang kakayahan ng mag-aaral sa lohikal na pag-iisip at pagsusuri ng mas kumplikadong problema.\n\n" +
                      "Layunin nitong mapaunlad ang tiwala sa sarili at kasanayan sa paghahanap ng tamang solusyon.";

                case "Math_Puzzle__Hard_M":
                    return "              MATH PUZZLE (HARD)\n\n" +
                      "Ang mahirap na antas ng Math Puzzle ay naglalaman ng masalimuot at mas hamong palaisipan na nangangailangan ng mataas na antas ng pag-aanalisa. Dito sinusubok ang tiyaga, diskarte, at kritikal na pag-iisip ng mag-aaral.\n\n" +
                      "Sa pagsasanay na ito, natututo silang mag-isip nang malalim at maghanap ng malikhaing solusyon sa komplikadong problema.";

                case "Time_Challenge__Easy_M":
                    return "              TIME CHALLENGE (EASY)\n\n" +
                      "Ang madali na antas ng Time Challenge ay nagtuturo ng tamang paggamit ng oras sa simpleng gawain. Layunin nitong sanayin ang mag-aaral na maging maayos at responsable sa kanilang oras.\n\n" +
                      "Sa antas na ito, natututo silang magplano at maglaan ng oras para sa bawat gawain sa araw-araw.";

                case "Time_Challenge__Medium_M":
                    return "              TIME CHALLENGE (MEDIUM)\n\n" +
                      "Sa katamtamang antas ng Time Challenge, mas hamon ang mga gawain na nangangailangan ng masusing pagpaplano at tamang paglalaan ng oras. Layunin nitong mapaunlad ang disiplina at kakayahan sa organisasyon.\n\n" +
                      "Dito, natututo ang mag-aaral kung paano pagsabayin ang maraming tungkulin nang hindi napapabayaan ang kalidad ng trabaho.";

                case "Time_Challenge__Hard_M":
                    return "              TIME CHALLENGE (HARD)\n\n" +
                      "Ang mahirap na antas ng Time Challenge ay naglalaman ng mga komplikadong sitwasyon kung saan kailangang magdesisyon nang mabilis at tama. Dito sinusubok ang kakayahan ng mag-aaral sa pamamahala ng oras sa ilalim ng pressure.\n\n" +
                      "Sa antas na ito, natututo silang mag-isip nang maayos kahit sa mahirap na sitwasyon at mas pinahahalagahan ang kahalagahan ng tamang oras sa tagumpay.";


                // Add more cases for other identifiers...

                default:
                    return "No description available.";
            }
        }
    }
}
