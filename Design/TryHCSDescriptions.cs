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
                    return "                            MATH PUZZLE (EASY)\n\n" +
                      "Description:\n" +
                      "The easy level of the Math Puzzle challenge introduces participants to fundamental mathematical concepts and operations in a fun and engaging way. This level is designed to build confidence as individuals tackle simple equations, patterns, and logical reasoning puzzles, enhancing their basic math skills.\n\n" +
                      "By participating in the easy challenge, individuals will develop their problem-solving abilities while enjoying the process of working through various math puzzles. This level serves as a foundation for more advanced challenges, ensuring participants are well-prepared as they progress. Regular practice at this level fosters a positive attitude toward mathematics and encourages critical thinking from an early stage.\n\n" +
                      "Engaging with the easy math puzzle challenge helps participants recognize the relevance of math in everyday life, reinforcing the idea that math can be both enjoyable and practical.";

                case "Math_Puzzle__Medium_M":
                    return "                            MATH PUZZLE (MEDIUM)\n\n" +
                      "Description:\n" +
                      "The medium level of the Math Puzzle challenge escalates the complexity of the puzzles, requiring participants to apply a deeper understanding of mathematical concepts and relationships. This level encourages critical thinking and logical reasoning as individuals work through more intricate problems that challenge their math skills.\n\n" +
                      "By engaging with the medium challenge, participants will refine their problem-solving strategies and enhance their ability to think critically under pressure. This level is designed to build on the foundation established in the easy challenge, allowing individuals to explore more advanced mathematical relationships and techniques. Regular practice at this level prepares participants for higher-level math and fosters a love for solving complex problems.\n\n" +
                      "As individuals tackle the medium math puzzle challenge, they will develop greater confidence in their mathematical abilities, paving the way for success in future academic endeavors.";

                case "Math_Puzzle__Hard_M":
                    return "                            MATH PUZZLE (HARD)\n\n" +
                      "Description:\n" +
                      "The hard level of the Math Puzzle challenge presents participants with complex and challenging puzzles that require advanced mathematical thinking and creativity. This level is designed for those who are eager to push their limits and tackle intricate problems that involve multiple steps and sophisticated concepts.\n\n" +
                      "Engaging with hard math puzzles fosters resilience and adaptability, as individuals learn to navigate challenging scenarios and apply their knowledge in innovative ways. This level promotes a high degree of analytical thinking and encourages participants to approach problems with a strategic mindset. Tackling these challenging puzzles builds confidence and enhances participants' abilities to think outside the box.\n\n" +
                      "Committing to the hard math puzzle challenge not only sharpens mathematical skills but also cultivates a profound appreciation for the beauty and complexity of mathematics. Participants will emerge as proficient problem solvers, ready to face advanced mathematical challenges with confidence and ingenuity.";


                // Add more cases for other identifiers...

                default:
                    return "No description available.";
            }
        }
    }
}