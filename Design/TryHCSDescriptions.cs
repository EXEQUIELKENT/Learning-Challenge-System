using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace Design
{
    public class TryDCRDescriptions
    {
        // Dictionary for mapping images to unique identifiers
        public Dictionary<Image, string> ImageIdentifiers { get; private set; }

        // Dictionary for mapping identifiers to descriptions
        public Dictionary<string, string> ImageDescriptions { get; private set; }
        public List<Image> Images { get; private set; }

        public TryDCRDescriptions()
        {
            {
                ImageIdentifiers = new Dictionary<Image, string>
            // Initialize images from .resx with unique identifiers
           
            {
            { DCP.Properties.Resources.Math_Puzzle__Easy_M, "Math_Puzzle__Easy_M" },
            { DCP.Properties.Resources.Math_Puzzle__Medium_M, "Math_Puzzle__Medium_M" },
            { DCP.Properties.Resources.Math_Puzzle__Hard_M, "Math_Puzzle__Hard_M" },
            };



                // Initialize descriptions for each identifier
                ImageDescriptions = new Dictionary<string, string>
            {
                { "Math_Puzzle__Easy_M", GenerateDescription("Math_Puzzle__Easy_M")},
                { "Math_Puzzle__Medium_M", GenerateDescription("Math_Puzzle__Medium_M")},
                { "Math_Puzzle__Hard_M", GenerateDescription("Math_Puzzle__Hard_M") },
                // Additional descriptions here...
            };
                Images = new List<Image>(ImageIdentifiers.Keys);
            }
            
        }
            private string GenerateDescription(string identifier)
        {
            // Sample long description, modify as needed
            switch (identifier)
            {
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


                // Add more cases for other identifiers...

                default:
                    return "No description available.";
            }
        }
    }
}