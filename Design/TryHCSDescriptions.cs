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
            { DCP.Properties.Resources.Push_Ups__Easy__F, "Push_Ups__Easy__F" },
            { DCP.Properties.Resources.Hold_Your_Breath__Easy_H, "Hold_Your_Breath__Easy_H" },
            { DCP.Properties.Resources.Grammar__Easy_E, "Grammar__Easy_E" },
            };



                // Initialize descriptions for each identifier
                ImageDescriptions = new Dictionary<string, string>
            {
                { "Push_Ups__Easy__F", GenerateDescription("Push_Ups__Easy__F")},
                { "Hold_Your_Breath__Easy_H", GenerateDescription("Hold_Your_Breath__Easy_H")},
                { "Grammar__Easy_E", GenerateDescription("Grammar__Easy_E") },
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
                case "Push_Ups__Easy__F":
                    return "                            PUSH UPS\n\n" +
                      "Description:\n" +
                      "Push-ups introduce individuals to this fundamental bodyweight exercise, targeting the chest, shoulders, and triceps. Participants can start with modifications, such as performing push-ups on their knees, to ensure proper form with hands placed shoulder-width apart and body aligned. This allows for gradual building of strength and endurance in the upper body while focusing on technique.\n\n" +
                      "Individuals should concentrate on engaging their core and maintaining a straight line during the movement. The key is to lower the chest towards the ground by bending the elbows, then pressing back up to the starting position. Incorporating push-ups into a regular fitness routine can lead to increased muscle tone and overall upper body strength.\n\n" +
                      "Over time, participants can progress to more challenging variations as they build confidence and strength, making push-ups a versatile exercise adaptable to various fitness levels.";


                case "Hold_Your_Breath__Easy_H":
                    return "                            HOLD BREATH\n\n" +
                      "Description:\n" +
                      "The hold breath challenge encourages participants to focus on their breathing techniques and lung capacity. This exercise starts with a deep inhale, followed by holding the breath for a set duration. It serves as an excellent introduction to breath control, which is beneficial for overall respiratory health.\n\n" +
                      "Practicing breath holds can help increase oxygen efficiency and improve focus. It is essential to remain relaxed and calm throughout the process, ensuring not to strain the body. This practice can also be a great way to enhance mindfulness and reduce stress levels by promoting a sense of tranquility.\n\n" +
                      "As participants become more comfortable with this exercise, it can be seamlessly integrated into daily routines, enhancing both physical and mental well-being and setting the foundation for more advanced breath-holding techniques.";

                case "Grammar__Easy_E":
                    return "                            GRAMMAR QUESTION ENGLISH\n\n" +
                     "Description:\n" +
                     "The Grammar Question challenge focuses on fundamental grammar rules and usage. Participants will engage with questions that test knowledge of sentence structure, punctuation, and word forms. This challenge is designed to enhance grammatical understanding and build confidence in language use.\n\n" +
                     "Through practice, participants will reinforce their grasp of essential grammar concepts, making it easier to identify correct usage in writing and speaking. This exercise fosters a solid foundation in grammar, essential for effective communication in English. By mastering these concepts, individuals prepare themselves for more complex grammatical challenges.\n\n" +
                     "Consistent engagement with grammar questions improves language skills and boosts confidence in applying these rules in various contexts. Participants will find themselves more comfortable with English grammar, setting the stage for ongoing learning and development.";

                // Add more cases for other identifiers...

                default:
                    return "No description available.";
            }
        }
    }
}