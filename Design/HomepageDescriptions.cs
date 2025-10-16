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
            { DCP.Properties.Resources.Time_Challenge__Hard_M, "Time_Challenge__Hard_M" }

            };
                Images = new List<Image>(ImageIdentifiers.Keys);
            }




            // Initialize descriptions for each identifier
            ImageDescriptions = new Dictionary<string, string>
            {
                //Fitness
                { "Push_Ups__Easy__F", GenerateDescription("Push_Ups__Easy__F")},
                { "Push_Ups__Medium_F", GenerateDescription("Push_Ups__Medium_F")},
                { "Push_Ups__Hard_F", GenerateDescription("Push_Ups__Hard_F")},
                { "Bear_Crawl__Easy_F", GenerateDescription("Bear_Crawl__Easy_F")},
                { "Bear_Crawl__Medium_F", GenerateDescription("Bear_Crawl__Medium_F")},
                { "Bear_Crawl__Hard_F", GenerateDescription("Bear_Crawl__Hard_F")},
                { "Bicycle_Crunches__Easy_F", GenerateDescription("Bicycle_Crunches__Easy_F")},
                { "Bicycle_Crunches__Medium_F", GenerateDescription("Bicycle_Crunches__Medium_F")},
                { "Bicycle_Crunches__Hard_F", GenerateDescription("Bicycle_Crunches__Hard_F")},
                { "Calf_Raises__Easy_F", GenerateDescription("Calf_Raises__Easy_F")},
                { "Calf_Raises__Medium_F", GenerateDescription("Calf_Raises__Medium_F")},
                { "Calf_Raises__Hard_F", GenerateDescription("Calf_Raises__Hard_F")},
                { "Diamond_Push_Ups__Easy_F", GenerateDescription("Diamond_Push_Ups__Easy_F")},
                { "Diamond_Push_Ups__Medium_F", GenerateDescription("Diamond_Push_Ups__Medium_F")},
                { "Diamond_Push_Ups__Hard_F", GenerateDescription("Diamond_Push_Ups__Hard_F")},
                { "Glute_Bridges__Medium_F", GenerateDescription("Glute_Bridges__Medium_F")},
                { "Glute_Bridges__Hard_F", GenerateDescription("Glute_Bridges__Hard_F")},
                { "Glute_Bridges__Easy_F", GenerateDescription("Glute_Bridges__Easy_F")},
                { "High_Knees__Easy_F", GenerateDescription("High_Knees__Easy_F")},
                { "High_Knees__Medium_F", GenerateDescription("High_Knees__Medium_F")},
                { "High_Knees__Hard_F", GenerateDescription("High_Knees__Hard_F")},
                { "Jogging__Easy_F", GenerateDescription("Jogging__Easy_F")},
                { "Jogging__Medium_F", GenerateDescription("Jogging__Medium_F")},
                { "Jogging__Hard_F", GenerateDescription("Jogging__Hard_F")},
                { "Jumping_Jacks__Easy_F", GenerateDescription("Jumping_Jacks__Easy_F")},
                { "Jumping_Jacks__Medium_F", GenerateDescription("Jumping_Jacks__Medium_F")},
                { "Jumping_Jacks__Hard_F", GenerateDescription("Jumping_Jacks__Hard_F")},
                { "Jumping_Squat__Easy_F", GenerateDescription("Jumping_Squat__Easy_F")},
                { "Jumping_Squat__Medium_F", GenerateDescription("Jumping_Squat__Medium_F")},
                { "Jumping_Squat__Hard_F", GenerateDescription("Jumping_Squat__Hard_F")},
                { "Leg_Raise__Easy_F", GenerateDescription("Leg_Raise__Easy_F")},
                { "Leg_Raise__Medium_F", GenerateDescription("Leg_Raise__Medium_F")},
                { "Leg_Raise__Hard_F", GenerateDescription("Leg_Raise__Hard_F")},
                { "Mountain_Climbers__Easy_F", GenerateDescription("Mountain_Climbers__Easy_F")},
                { "Mountain_Climbers__Medium_F", GenerateDescription("Mountain_Climbers__Medium_F")},
                { "Mountain_Climbers__Hard_F", GenerateDescription("Mountain_Climbers__Hard_F")},
                { "Planking__Easy_F", GenerateDescription("Planking__Easy_F")},
                { "Planking__Medium_F", GenerateDescription("Planking__Medium_F")},
                { "Planking__Hard_F", GenerateDescription("Planking__Hard_F")},
                { "Reverse_Lunges__Easy_F", GenerateDescription("Reverse_Lunges__Easy_F")},
                { "Reverse_Lunges__Medium_F", GenerateDescription("Reverse_Lunges__Medium_F")},
                { "Reverse_Lunges__Hard_F", GenerateDescription("Reverse_Lunges__Hard_F")},
                { "Russian_Twist__Easy_F", GenerateDescription("Russian_Twist__Easy_F")},
                { "Russian_Twist__Medium_F", GenerateDescription("Russian_Twist__Medium_F")},
                { "Russian_Twist__Hard_F", GenerateDescription("Russian_Twist__Hard_F")},
                { "Side_Lunges__Easy_F", GenerateDescription("Side_Lunges__Easy_F")},
                { "Side_Lunges__Medium_F", GenerateDescription("Side_Lunges__Medium_F")},
                { "Side_Lunges__Hard_F", GenerateDescription("Side_Lunges__Hard_F")},
                { "Side_Plank__Easy_F", GenerateDescription("Side_Plank__Easy_F")},
                { "Side_Plank__Medium_F", GenerateDescription("Side_Plank__Medium_F")},
                { "Side_Plank__Hard_F", GenerateDescription("Side_Plank__Hard_F")},
                { "Squat__Easy_F", GenerateDescription("Squat__Easy_F")},
                { "Squat__Medium_F", GenerateDescription("Squat__Medium_F")},
                { "Squat__Hard_F", GenerateDescription("Squat__Hard_F")},
                { "Standing_Side_Leg_Raises__Easy_F", GenerateDescription("Standing_Side_Leg_Raises__Easy_F")},
                { "Standing_Side_Leg_Raises__Medium__F", GenerateDescription("Standing_Side_Leg_Raises__Medium__F")},
                { "Standing_Side_Leg_Raises__Hard_F", GenerateDescription("Standing_Side_Leg_Raises__Hard_F")},
                { "Step_Up__Easy_F", GenerateDescription("Step_Up__Easy_F")},
                { "Step_Up__Medium_F", GenerateDescription("Step_Up__Medium_F")},
                { "Step_Up__Hard_F", GenerateDescription("Step_Up__Hard_F")},
                { "Toe_Top__Easy_F", GenerateDescription("Toe_Top__Easy_F")},
                { "Toe_Top__Medium_F", GenerateDescription("Toe_Top__Medium_F")},
                { "Toe_Top__Hard_F", GenerateDescription("Toe_Top__Hard_F")},
                { "Wall_Sit__Easy_F", GenerateDescription("Wall_Sit__Easy_F")},
                { "Wall_Sit__Medium_F", GenerateDescription("Wall_Sit__Medium_F")},
                { "Wall_Sit__Hard_F", GenerateDescription("Wall_Sit__Hard_F")},
                //Health
                { "Gratitude__Easy_H", GenerateDescription("Gratitude__Easy_H")},
                { "Gratitude__Medium_H", GenerateDescription("Gratitude__Medium_H")},
                { "Gratitude__Hard_H", GenerateDescription("Gratitude__Hard_H")},
                { "Hold_Your_Breath__Easy_H", GenerateDescription("Hold_Your_Breath__Easy_H")},
                { "Hold_Your_Breath__Medium_H", GenerateDescription("Hold_Your_Breath__Medium_H")},
                { "Hold_Your_Breath__Hard_H", GenerateDescription("Hold_Your_Breath__Hard_H")},
                { "Hydration__Easy_H", GenerateDescription("Hydration__Easy_H")},
                { "Hydration__Medium_H", GenerateDescription("Hydration__Medium_H")},
                { "Hydration__Hard_H", GenerateDescription("Hydration__Hard_H")},
                { "Mindful_Breathing__Easy_H", GenerateDescription("Mindful_Breathing__Easy_H")},
                { "Mindful_Breathing__Medium_H", GenerateDescription("Mindful_Breathing__Medium_H")},
                { "Mindful_Breathing__Hard_H", GenerateDescription("Mindful_Breathing__Hard_H")},
                { "Mindful_Eating__Easy_H", GenerateDescription("Mindful_Eating__Easy_H")},
                { "Mindful_Eating__Medium_H", GenerateDescription("Mindful_Eating__Medium_H")},
                { "Mindful_Eating__Hard_H", GenerateDescription("Mindful_Eating__Hard_H")},
                { "No_Blinking__Easy_H", GenerateDescription("No_Blinking__Easy_H")},
                { "No_Blinking__Medium_H", GenerateDescription("No_Blinking__Medium_H")},
                { "No_Blinking__Hard_H", GenerateDescription("No_Blinking__Hard_H")},
                { "Quick_Stretching__Easy_H", GenerateDescription("Quick_Stretching__Easy_H")},
                { "Quick_Stretching__Medium_H", GenerateDescription("Quick_Stretching__Medium_H")},
                { "Quick_Stretching__Hard_H", GenerateDescription("Quick_Stretching__Hard_H")},
                { "Plan_your_week__Easy_H", GenerateDescription("Plan_your_week__Easy_H")},
                { "Plan_your_week__Medium_H", GenerateDescription("Plan_your_week__Medium_H")},
                { "Plan_your_week__Hard_H", GenerateDescription("Plan_your_week__Hard_H")},
                { "Plan_Your_Week_Mental__Easy_H", GenerateDescription("Plan_Your_Week_Mental__Easy_H")},
                { "Plan_Your_Week_Mental__Medium_H", GenerateDescription("Plan_Your_Week_Mental__Medium_H") },
                { "Plan_Your_Week_Mental__Hard_H", GenerateDescription("Plan_Your_Week_Mental__Hard_H") },
                { "Quiz_Mental__Easy_H", GenerateDescription("Quiz_Mental__Easy_H") },
                { "Quiz_Mental__Medium_H", GenerateDescription("Quiz_Mental__Medium_H") },
                { "Quiz_Mental__Hard_H", GenerateDescription("Quiz_Mental__Hard_H") },
                { "Reading_Time__Easy_H", GenerateDescription("Reading_Time__Easy_H") },
                { "Reading_Time__Medium_H", GenerateDescription("Reading_Time__Medium_H") },
                { "Reading_Time__Hard_H", GenerateDescription("Reading_Time__Hard_H") },
                { "Take_a_Cold_Shower__Easy_H", GenerateDescription("Take_a_Cold_Shower__Easy_H") },
                { "Take_a_Cold_Shower__Medium_H", GenerateDescription("Take_a_Cold_Shower__Medium_H") },
                { "Take_a_Cold_Shower__Hard_H", GenerateDescription("Take_a_Cold_Shower__Hard_H") },
                { "Walking_Easy_H", GenerateDescription("Walking_Easy_H") },
                { "Walking_Medium_H", GenerateDescription("Walking_Medium_H") },
                { "Walking_Hard_H", GenerateDescription("Walking_Hard_H") },
                //Learning
                { "Book_Summary__Easy_E", GenerateDescription("Book_Summary__Easy_E") },
                { "Book_Summary__Medium_E", GenerateDescription("Book_Summary__Medium_E") },
                { "Book_Summary__Hard_E", GenerateDescription("Book_Summary__Hard_E") },
                { "Character_Analysis_English__Easy_E", GenerateDescription("Character_Analysis_English__Easy_E") },
                { "Character_Analysis_English_Medium_E", GenerateDescription("Character_Analysis_English_Medium_E") },
                { "Character_Analysis_English__Hard_E", GenerateDescription("Character_Analysis_English__Hard_E") },
                { "Characteristic_Analysis_Filipino__Easy_F", GenerateDescription("Characteristic_Analysis_Filipino__Easy_F") },
                { "Characteristic_Analysis_Filipino___Medium_F", GenerateDescription("Characteristic_Analysis_Filipino___Medium_F") },
                { "Characteristic_Analysis_Filipino___Hard_F", GenerateDescription("Characteristic_Analysis_Filipino___Hard_F") },
                { "Grammar__Easy_E", GenerateDescription("Grammar__Easy_E") },
                { "Grammar__Medium_E", GenerateDescription("Grammar__Medium_E") },
                { "Grammar__Hard_E", GenerateDescription("Grammar__Hard_E") },
                { "Vocabulary_Challenge__Easy_E", GenerateDescription("Vocabulary_Challenge__Easy_E") },
                { "Vocabulary_Challenge__Medium_E", GenerateDescription("Vocabulary_Challenge__Medium_E") },
                { "Vocabulary_Challenge__Hard_E", GenerateDescription("Vocabulary_Challenge__Hard_E") },
                { "Word_Count_Challenge_Easy_E", GenerateDescription("Word_Count_Challenge_Easy_E") },
                { "Word_Count_Challenge_Medium_E", GenerateDescription("Word_Count_Challenge_Medium_E") },
                { "Word_Count_Challenge_Hard_E", GenerateDescription("Word_Count_Challenge_Hard_E") },
                { "Dialog_Analysis__Easy_F", GenerateDescription("Dialog_Analysis__Easy_F") },
                { "Dialog_Analysis__Medium_F", GenerateDescription("Dialog_Analysis__Medium_F") },
                { "Dialog_Analysis__Hard_F", GenerateDescription("Dialog_Analysis__Hard_F") },
                { "Filipino_Quiz__Easy_F", GenerateDescription("Filipino_Quiz__Easy_F") },
                { "Filipino_Quiz__Medium_F", GenerateDescription("Filipino_Quiz__Medium_F") },
                { "Filipino_Quiz__Hard_F", GenerateDescription("Filipino_Quiz__Hard_F") },
                { "Poetry_Challenge__Easy_F", GenerateDescription("Poetry_Challenge__Easy_F") },
                { "Poetry_Challenge__Medium_F", GenerateDescription("Poetry_Challenge__Medium_F") },
                { "Poetry_Challenge__Hard_F", GenerateDescription("Poetry_Challenge__Hard_F") },
                { "Story_Retelling__Easy_F", GenerateDescription("Story_Retelling__Easy_F") },
                { "Story_Retelling__Medium_F", GenerateDescription("Story_Retelling__Medium_F") },
                { "Story_Retelling__Hard_F", GenerateDescription("Story_Retelling__Hard_F") },
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
                { "Time_Challenge__Hard_M", GenerateDescription("Time_Challenge__Hard_M") }
                // Additional descriptions here...
            };

        }
        private string GenerateDescription(string identifier)
        {
            // Sample long description, modify as needed
            switch (identifier)
            {
                //Fitness
                case "Push_Ups__Easy__F":
                    return "                            PUSH UPS (EASY)\n\n" +
                      "Description:\n" +
                      "Push-ups at the easy level introduce beginners to this fundamental bodyweight exercise, targeting the chest, shoulders, and triceps. To perform an easy push-up, participants can start on their knees to reduce the load, ensuring proper form with hands placed shoulder-width apart and body aligned from head to knees. This modification allows for gradual building of strength and endurance in the upper body while focusing on technique.\n\n" +
                      "As individuals grow more comfortable, they should concentrate on engaging their core and maintaining a straight line from head to knees during the movement. The key is to lower the chest towards the ground by bending the elbows, then pressing back up to the starting position. Incorporating push-ups into a regular fitness routine can lead to increased muscle tone and overall upper body strength.\n\n" +
                      "Over time, participants can progress to more challenging variations as they build confidence and strength, making push-ups a versatile exercise that can be adapted to various fitness levels.";

                case "Push_Ups__Medium_F":
                    return "                            PUSH UPS (MEDIUM)\n\n" +
                      "Description:\n" +
                      "The medium level of push-ups introduces a standard form, where participants perform push-ups from the toes, which significantly increases the intensity. This level emphasizes proper form and strength development in the chest, shoulders, and triceps. To perform a medium push-up, start in a plank position with feet together and body straight from head to heels, lowering the body towards the ground while keeping elbows at a 45-degree angle.\n\n" +
                      "As strength improves, individuals should focus on maintaining control throughout the movement, ensuring the core remains engaged to prevent sagging or arching of the back. Regular practice of medium push-ups can enhance overall upper body strength, stability, and endurance, making them an effective addition to any workout routine.\n\n" +
                      "Participants may also explore variations such as wide grip or close grip push-ups to target different muscle groups, providing a well-rounded approach to building upper body strength and versatility in workouts.";

                case "Push_Ups__Hard_F":
                    return "                            PUSH UPS (HARD)\n\n" +
                      "Description:\n" +
                      "The hard level of push-ups challenges participants to perform advanced variations that significantly increase strength and endurance demands. This may include exercises such as decline push-ups, where the feet are elevated, or explosive push-ups, which involve pushing off the ground with enough force to lift the hands off the floor. These variations not only enhance upper body strength but also engage core stability and improve explosive power.\n\n" +
                      "Performing hard-level push-ups requires strong form and technique to prevent injury. It’s essential to focus on controlled movements, ensuring a full range of motion and proper alignment. Incorporating these advanced push-up variations into training routines can lead to remarkable improvements in upper body strength, muscle definition, and athletic performance.\n\n" +
                      "As participants progress through these challenging exercises, they should continue to listen to their bodies, allowing for adequate recovery and avoiding overtraining. This dedication to mastering push-ups can ultimately lead to increased fitness levels and enhanced physical capabilities.";

                case "Bear_Crawl__Easy_F":
                    return "                            BEAR CRAWL (EASY)\n\n" +
                      "Description:\n" +
                      "The easy level of the bear crawl is an excellent introduction to this dynamic exercise that engages multiple muscle groups while enhancing coordination and stability. To perform the bear crawl, begin on all fours with hands positioned directly beneath the shoulders and knees beneath the hips. Keeping the back flat and core engaged, move forward by simultaneously reaching one hand and the opposite knee forward, creating a crawling motion that mimics a bear.\n\n" +
                      "This modification is accessible for beginners and helps develop foundational strength in the shoulders, core, and legs. The focus should be on moving slowly and deliberately, ensuring proper alignment and stability throughout the movement. Regular practice of the easy bear crawl can enhance overall body awareness and strength, making it a valuable addition to any fitness routine.\n\n" +
                      "As participants progress, they can gradually increase the intensity and speed of the bear crawl, preparing for more challenging variations. This exercise not only promotes physical fitness but also improves functional movement patterns essential for daily activities.";

                case "Bear_Crawl__Medium_F":
                    return "                            BEAR CRAWL (MEDIUM)\n\n" +
                      "Description:\n" +
                      "The medium level of the bear crawl builds on the foundational movement established in the easy variation, increasing the intensity and challenge. In this version, participants perform the bear crawl at a quicker pace while maintaining proper form, which enhances cardiovascular fitness in addition to strength. Starting in the same all-fours position, participants should focus on maintaining a strong core and a neutral spine while moving in a controlled yet brisk manner.\n\n" +
                      "The medium bear crawl engages the shoulders, arms, and core even more, as participants must stabilize their bodies while moving. Emphasizing rhythm and coordination, this exercise helps improve overall agility and body control. Practicing the medium bear crawl regularly contributes to building endurance and functional strength, vital for various physical activities.\n\n" +
                      "As individuals become more proficient, they can experiment with adding directional changes or incorporating obstacle courses to further enhance the difficulty and variety of their workouts. This adaptability makes the bear crawl a dynamic and enjoyable exercise option.";

                case "Bear_Crawl__Hard_F":
                    return "                            BEAR CRAWL (HARD)\n\n" +
                      "Description:\n" +
                      "The hard level of the bear crawl presents an advanced challenge that significantly tests strength, endurance, and coordination. In this variation, participants increase the distance covered and may incorporate elements such as lateral movement or increased speed to intensify the workout. Starting from the all-fours position, individuals focus on maintaining a low and stable posture while moving swiftly across the ground, engaging the core and stabilizing muscles throughout the movement.\n\n" +
                      "This intense exercise not only targets the upper body and core but also engages the lower body, enhancing functional strength and overall athleticism. Proper form is essential in the hard bear crawl to prevent injuries, with emphasis on keeping the back flat and hips low while ensuring each movement is deliberate and controlled. Incorporating this advanced variation into workout routines can lead to significant improvements in strength and fitness levels.\n\n" +
                      "To further challenge themselves, participants can combine the bear crawl with other exercises in a circuit format, creating a comprehensive workout that boosts cardiovascular endurance and builds muscle. As with all exercises, consistency and proper recovery are key to maximizing the benefits of this powerful movement.";

                case "Jogging__Easy_F":
                    return "                            JOGGING (EASY)\n\n" +
                      "Description:\n" +
                      "Jogging is a moderate cardiovascular exercise that promotes endurance, boosts cardiovascular health, and helps in maintaining a healthy weight. " +
                      "Easy jogging is ideal for beginners or those warming up, as it allows the body to ease into a steady pace, gradually enhancing stamina and comfort.\n\n" +
                      "Jogging at an easy pace helps reduce joint stress and risk of injury, making it an accessible and sustainable way to improve cardiovascular fitness. " +
                      "This steady pace strengthens the heart, improves lung capacity, and can increase energy levels over time, contributing to overall physical health.\n\n" +
                      "Regular jogging is a powerful way to build a lasting fitness routine. By maintaining a comfortable pace, individuals can develop consistency, making jogging " +
                      "an enjoyable activity that enhances both physical and mental well-being.";

                case "Jogging__Medium_F":
                    return "                            JOGGING (MEDIUM)\n\n" +
                      "Description:\n" +
                      "Medium-intensity jogging is effective for building endurance, burning calories, and enhancing cardiovascular health. " +
                      "With a moderate effort, this pace allows you to challenge yourself while still being able to sustain the jog for a longer duration, ideal for fitness progression.\n\n" +
                      "Jogging at a medium pace works both the aerobic and muscular systems, strengthening the heart and lower body while promoting efficient breathing. " +
                      "This intensity level supports fat loss, muscle tone, and overall endurance, making it suitable for intermediate exercisers.\n\n" +
                      "Incorporating medium-intensity jogging into your workout routine can lead to a balanced improvement in stamina and strength, " +
                      "encouraging consistent, manageable progress without overexertion.";

                case "Jogging__Hard_F":
                    return "                            JOGGING (HARD)\n\n" +
                      "Description:\n" +
                      "High-intensity jogging pushes cardiovascular limits, increases calorie burn, and improves speed and endurance. " +
                      "Jogging at this intensity challenges the body to adapt to faster paces and greater physical demands, ideal for seasoned joggers or those aiming to push boundaries.\n\n" +
                      "This hard jogging pace requires significant strength and stamina, working both aerobic capacity and lower-body muscle groups. " +
                      "Jogging at a faster pace helps in building resilience, enhancing lung function, and maximizing fitness gains, though it can be demanding on joints and muscles.\n\n" +
                      "Integrating hard jogging sessions into a routine can accelerate fitness improvements, particularly when combined with rest and recovery days, " +
                      "making it a strategic choice for achieving higher endurance levels and cardiovascular strength.";

                case "Bicycle_Crunches__Easy_F":
                    return "                            BICYCLE CRUNCHES (EASY)\n\n" +
                      "Description:\n" +
                      "Bicycle crunches are a core exercise that engages the abdominal muscles, especially the obliques. In the easy version, the movements are slower " +
                      "and more controlled, allowing beginners to focus on proper form and technique without overstraining.\n\n" +
                      "This level of bicycle crunches promotes core stability and muscular awareness. Starting on your back with knees bent, alternate bringing one knee " +
                      "toward the opposite elbow while keeping the other leg extended, all while engaging the core.\n\n" +
                      "The easy level is great for developing core strength at a manageable pace, helping beginners build confidence in their form and gradually improve " +
                      "endurance and flexibility.";

                case "Bicycle_Crunches__Medium_F":
                    return "                            BICYCLE CRUNCHES (MEDIUM)\n\n" +
                      "Description:\n" +
                      "The medium version of bicycle crunches adds intensity by increasing the speed and pace of the exercise, challenging the core muscles more " +
                      "intensively. This level works the abdominals and obliques while also engaging the hip flexors, helping to build a more defined core.\n\n" +
                      "To perform, lie on your back and alternate elbow-to-knee touches with a quicker pace while keeping the opposite leg extended. This level " +
                      "focuses on maintaining good form while increasing the difficulty.\n\n" +
                      "This level provides a balanced challenge for those looking to progress in their core workout, promoting strength, endurance, and a stronger midsection.";

                case "Bicycle_Crunches__Hard_F":
                    return "                            BICYCLE CRUNCHES (HARD)\n\n" +
                      "Description:\n" +
                      "The hard version of bicycle crunches is an advanced variation designed to maximize core activation, especially in the obliques and lower abs. " +
                      "Performing the exercise at a high intensity and faster pace challenges muscular endurance and coordination.\n\n" +
                      "This level requires strict form, with quick, controlled movements that engage the entire core. The advanced pace makes it an intense abdominal " +
                      "workout for strength and stability.\n\n" +
                      "This hard variation is suitable for those with experience in core exercises, providing an intense and effective workout for a stronger, more toned midsection.";

                case "Calf_Raises__Easy_F":
                    return "                            CALF RAISES (EASY)\n\n" +
                      "Description:\n" +
                      "Calf raises are a lower body exercise that targets the calf muscles, which are essential for movements involving the ankles and feet. In the easy version, " +
                      "calf raises are performed with a slow and controlled motion, focusing on proper form and engaging the calves without adding excessive strain.\n\n" +
                      "This level is ideal for beginners, as it promotes balance and muscle activation without intense pressure. To perform, stand with feet hip-width apart " +
                      "and slowly lift your heels off the ground, then lower back down with control.\n\n" +
                      "The easy level is beneficial for improving stability and increasing calf endurance gradually, making it suitable for those new to lower body exercises.";

                case "Calf_Raises__Medium_F":
                    return "                            CALF RAISES (MEDIUM)\n\n" +
                      "Description:\n" +
                      "The medium level of calf raises adds more intensity by incorporating a faster tempo or slightly increasing the range of motion, which engages the calf muscles more " +
                      "effectively. This level helps build strength and endurance in the calves, supporting improved lower body stability.\n\n" +
                      "To perform, stand with feet hip-width apart, lift your heels off the ground, and control the movement as you lower back down. At this level, you may add a pause " +
                      "at the top to increase engagement.\n\n" +
                      "The medium level is effective for those progressing in their lower body workouts, providing a moderate challenge that enhances calf strength and balance.";

                case "Calf_Raises__Hard_F":
                    return "                            CALF RAISES (HARD)\n\n" +
                      "Description:\n" +
                      "The hard version of calf raises is an advanced exercise that maximizes calf engagement by increasing the pace or incorporating additional resistance, " +
                      "such as holding weights. This variation challenges muscular endurance and strength in the lower legs.\n\n" +
                      "To perform, lift your heels with control and, if possible, hold a weight for added intensity. You can also try single-leg calf raises to further increase the difficulty.\n\n" +
                      "This hard variation is excellent for those experienced in calf exercises, aiming to build a strong, stable foundation for activities that rely on lower body power.";

                case "Diamond_Push_Ups__Easy_F":
                    return "                            DIAMOND PUSH UPS (EASY)\n\n" +
                      "Description:\n" +
                      "Diamond push-ups are a variation of the traditional push-up that emphasizes the triceps more intensely. In the easy version, diamond push-ups " +
                      "are performed at a slower pace or with knees on the ground to reduce difficulty, allowing beginners to build strength gradually in the triceps and chest.\n\n" +
                      "This level is perfect for those new to diamond push-ups, focusing on proper hand placement and engaging the core. Place your hands in a diamond shape beneath " +
                      "your chest, bend your elbows, and lower your body.\n\n" +
                      "The easy level of diamond push-ups helps to improve upper body strength while building confidence in the movement, making it a great entry point for this exercise.";

                case "Diamond_Push_Ups__Medium_F":
                    return "                            DIAMOND PUSH UPS (MEDIUM)\n\n" +
                      "Description:\n" +
                      "The medium level of diamond push-ups adds a moderate challenge by performing the exercise with proper form, targeting the triceps, chest, and shoulders effectively. " +
                      "This level requires balance and a strong core, making it suitable for those looking to increase upper body strength.\n\n" +
                      "To perform, place your hands in a diamond shape beneath your chest, maintain a straight body line, and bend your elbows to lower down before pressing back up.\n\n" +
                      "This medium variation is excellent for progressing strength in the upper body, providing a balanced challenge that builds endurance and muscle tone.";

                case "Diamond_Push_Ups__Hard_F":
                    return "                            DIAMOND PUSH UPS (HARD)\n\n" +
                      "Description:\n" +
                      "The hard version of diamond push-ups is an advanced exercise that fully engages the triceps, chest, and core with a high level of difficulty. This variation " +
                      "can include adding speed, resistance, or elevation (such as lifting one leg) to intensify the workout.\n\n" +
                      "To perform, place your hands in a diamond shape and keep your body aligned from head to heels. Lower your chest towards your hands and press back up, " +
                      "maintaining control and strength.\n\n" +
                      "The hard level of diamond push-ups is ideal for experienced exercisers aiming to maximize tricep engagement and upper body power, pushing endurance to new limits.";

                case "Glute_Bridges__Easy_F":
                    return "                            GLUTE BRIDGES (EASY)\n\n" +
                      "Description:\n" +
                      "Glute bridges are a core and lower-body exercise that focuses on strengthening the glutes, hamstrings, and lower back. The easy variation of glute bridges is " +
                      "performed at a controlled pace, allowing beginners to build awareness of the movement and focus on form.\n\n" +
                      "To perform, lie on your back with knees bent and feet flat on the floor, then lift your hips towards the ceiling by squeezing your glutes and hold briefly before lowering.\n\n" +
                      "This easy level is perfect for those new to exercise or recovering from injury, helping to improve glute activation, core stability, and lower body strength in a gentle way.";

                case "Glute_Bridges__Medium_F":
                    return "                            GLUTE BRIDGES (MEDIUM)\n\n" +
                      "Description:\n" +
                      "The medium variation of glute bridges increases the challenge slightly, encouraging more engagement from the glutes and hamstrings. This level helps to " +
                      "enhance strength in the posterior chain, promoting better stability and power.\n\n" +
                      "In this variation, perform glute bridges with a controlled lift and lower, focusing on squeezing the glutes at the top of the movement and holding for a brief pause.\n\n" +
                      "This medium level builds core and lower body strength, making it ideal for those who want to improve posture, stability, and overall lower body endurance.";

                case "Glute_Bridges__Hard_F":
                    return "                            GLUTE BRIDGES (HARD)\n\n" +
                      "Description:\n" +
                      "The hard level of glute bridges is an advanced exercise variation that fully engages the glutes, hamstrings, and core. This level can include added resistance, " +
                      "such as weights or a single-leg variation, to intensify the workout.\n\n" +
                      "To perform, lie on your back with knees bent, lift your hips up by engaging the glutes, and hold at the top position before slowly lowering. Control and stability are key.\n\n" +
                      "The hard level is ideal for those experienced in lower-body exercises, providing maximum glute activation and challenging endurance, strength, and power.";

                case "High_Knees__Easy_F":
                    return "                            HIGH KNEES (EASY)\n\n" +
                      "Description:\n" +
                      "High knees are a cardio exercise that involves lifting the knees alternately to waist height, engaging the core, and improving heart rate. The easy level " +
                      "focuses on form and controlled movement, perfect for beginners or as a warm-up.\n\n" +
                      "To perform, stand tall, and alternate lifting each knee to hip level while keeping a steady pace and maintaining balance.\n\n" +
                      "This level of high knees is excellent for improving coordination, increasing blood flow, and preparing the body for more intense exercises.";

                case "High_Knees__Medium_F":
                    return "                            HIGH KNEES (MEDIUM)\n\n" +
                      "Description:\n" +
                      "The medium level of high knees involves a quicker pace and greater intensity, elevating the heart rate and engaging the core, thighs, and calves. " +
                      "This variation is great for those looking to boost cardiovascular endurance and leg strength.\n\n" +
                      "To perform, lift each knee to hip height at a moderate pace, focusing on controlled breathing and strong core engagement throughout the movement.\n\n" +
                      "High knees at this level provide an effective way to enhance stamina, coordination, and overall agility, making it ideal for moderate-intensity workouts.";

                case "High_Knees__Hard_F":
                    return "                            HIGH KNEES (HARD)\n\n" +
                      "Description:\n" +
                      "The hard level of high knees is an intense cardiovascular exercise, requiring quick movements and maximal knee height to push endurance and agility. " +
                      "It involves maintaining a fast pace while lifting each knee as high as possible.\n\n" +
                      "To perform, alternate lifting knees to at least hip height at a rapid pace, keeping core engaged and focusing on breathing.\n\n" +
                      "This advanced level of high knees is perfect for athletes or those seeking a challenging cardio session, enhancing leg strength, core stability, and explosive speed.";

                case "Jumping_Jacks__Easy_F":
                    return "                            JUMPING JACKS (EASY)\n\n" +
                      "Description:\n" +
                      "Jumping jacks are a full-body cardio exercise that engages the legs, arms, and core while boosting heart rate. The easy level focuses on slower, controlled movements, making it suitable for beginners or as a gentle warm-up.\n\n" +
                      "To perform, stand with feet together and arms at your sides, then jump, spreading legs shoulder-width apart and raising arms overhead. Return to start position in a controlled manner.\n\n" +
                      "This level is excellent for improving basic cardiovascular fitness, coordination, and getting the body moving with minimal impact.";

                case "Jumping_Jacks__Medium_F":
                    return "                            JUMPING JACKS (MEDIUM)\n\n" +
                      "Description:\n" +
                      "The medium level of jumping jacks increases speed and intensity, offering a moderate cardio workout that strengthens the lower and upper body and raises endurance.\n\n" +
                      "To perform, jump with legs opening shoulder-width apart and arms raised overhead at a steady pace. Land softly with knees slightly bent to absorb impact.\n\n" +
                      "This level is ideal for enhancing aerobic fitness and providing an energy boost, making it a perfect addition to moderate-intensity workout routines.";

                case "Jumping_Jacks__Hard_F":
                    return "                            JUMPING JACKS (HARD)\n\n" +
                      "Description:\n" +
                      "The hard level of jumping jacks is a high-intensity cardio exercise designed to push endurance and strengthen the full body, requiring a quick pace and high energy.\n\n" +
                      "To perform, jump rapidly while opening legs and raising arms overhead, aiming for maximum speed and core engagement.\n\n" +
                      "This advanced level is effective for those seeking a challenging cardiovascular boost, helping to improve agility, coordination, and muscle tone.";

                case "Jumping_Squat__Easy_F":
                    return "                            JUMPING SQUAT (EASY)\n\n" +
                      "Description:\n" +
                      "Jumping squats are a plyometric exercise that engages the glutes, thighs, and calves. The easy level emphasizes proper squat form with a small jump, suitable for beginners looking to build strength.\n\n" +
                      "To perform, squat down and then push off slightly from the ground, landing softly back into a squat position.\n\n" +
                      "This level is excellent for building foundational leg strength and practicing form, preparing the body for more intense jumping exercises.";

                case "Jumping_Squat__Medium_F":
                    return "                            JUMPING SQUAT (MEDIUM)\n\n" +
                      "Description:\n" +
                      "The medium level of jumping squats involves a stronger push-off, requiring more effort to leave the ground and work the lower body muscles more effectively.\n\n" +
                      "To perform, squat down and push off with enough power to leave the ground, landing gently back in a squat position to protect knees.\n\n" +
                      "This level is perfect for building strength and endurance, enhancing both leg power and cardiovascular fitness.";

                case "Jumping_Squat__Hard_F":
                    return "                            JUMPING SQUAT (HARD)\n\n" +
                      "Description:\n" +
                      "The hard level of jumping squats is an intense plyometric exercise that requires maximum power and explosiveness, ideal for advanced fitness levels seeking a challenge.\n\n" +
                      "To perform, squat deeply and push off explosively to achieve maximum height with each jump, landing back softly into a squat position.\n\n" +
                      "This advanced level is effective for enhancing athletic performance, increasing lower body strength, and building explosive power in the legs.";

                case "Leg_Raise__Easy_F":
                    return "                            LEG RAISE (EASY)\n\n" +
                      "Description:\n" +
                      "Leg raises are a core strengthening exercise that primarily targets the abdominal muscles. The easy level focuses on controlled movements, making it suitable for beginners looking to enhance core stability without straining.\n\n" +
                      "To perform, lie flat on your back with legs extended, then slowly lift your legs towards the ceiling while keeping your lower back pressed into the ground. Lower your legs back down without touching the floor to maintain tension in the abs.\n\n" +
                      "This level helps in building foundational core strength and improving body awareness, essential for progressing to more challenging variations.";

                case "Leg_Raise__Medium_F":
                    return "                            LEG RAISE (MEDIUM)\n\n" +
                      "Description:\n" +
                      "The medium level of leg raises increases the challenge by incorporating a higher range of motion and tempo, providing a more intense workout for the abdominal muscles and hip flexors.\n\n" +
                      "To perform, engage your core as you lift your legs higher towards the ceiling, ensuring control and stability. Lower your legs slowly, keeping your core tight to avoid arching your back.\n\n" +
                      "This level effectively strengthens the core, improves flexibility, and enhances muscle tone, making it a great addition to an intermediate workout routine.";

                case "Leg_Raise__Hard_F":
                    return "                            LEG RAISE (HARD)\n\n" +
                      "Description:\n" +
                      "The hard level of leg raises adds significant intensity, requiring advanced core strength and control to execute correctly. This variation is ideal for those looking to challenge their abdominal muscles further.\n\n" +
                      "To perform, raise your legs fully towards the ceiling and hold for a moment before lowering them slowly to just above the ground, then raise them back up, ensuring to keep your back flat on the floor.\n\n" +
                      "This level is effective for developing strong core muscles, improving endurance, and increasing overall body control, making it a vital exercise for advanced fitness enthusiasts.";

                case "Mountain_Climbers__Easy_F":
                    return "                            MOUNTAIN CLIMBERS (EASY)\n\n" +
                      "Description:\n" +
                      "Mountain climbers are a dynamic full-body exercise that targets the core, shoulders, and legs. The easy level introduces this movement at a manageable pace, making it suitable for beginners who want to build endurance and strength gradually.\n\n" +
                      "To perform, start in a plank position with your hands under your shoulders. Drive one knee towards your chest while keeping the other leg extended. Alternate legs in a controlled manner, ensuring your hips stay low and your core is engaged.\n\n" +
                      "This level focuses on developing basic coordination, cardiovascular fitness, and core stability, which are essential for progressing to more advanced variations.";

                case "Mountain_Climbers__Medium_F":
                    return "                            MOUNTAIN CLIMBERS (MEDIUM)\n\n" +
                      "Description:\n" +
                      "The medium level of mountain climbers increases the speed and intensity of the movement, providing a more challenging cardiovascular workout that also engages the core and lower body effectively.\n\n" +
                      "To perform, maintain a plank position and quickly alternate driving your knees towards your chest, aiming for a brisk pace. Focus on keeping your form tight and hips low to maximize engagement of the core muscles.\n\n" +
                      "This level enhances cardiovascular endurance, strengthens the core, and improves agility, making it an excellent addition to a well-rounded fitness routine.";

                case "Mountain_Climbers__Hard_F":
                    return "                            MOUNTAIN CLIMBERS (HARD)\n\n" +
                      "Description:\n" +
                      "The hard level of mountain climbers elevates the challenge by incorporating explosive movements, requiring advanced coordination, strength, and endurance to perform effectively. This variation is perfect for those seeking to push their limits.\n\n" +
                      "To perform, start in a plank position and explosively switch legs, driving one knee towards your chest while extending the other leg. Aim for maximum speed while maintaining proper form, keeping your core engaged throughout.\n\n" +
                      "This level effectively boosts heart rate, builds muscular endurance, and enhances overall athletic performance, making it a staple for high-intensity workouts.";

                case "Planking__Easy_F":
                    return "                            PLANKING (EASY)\n\n" +
                      "Description:\n" +
                      "Planking is a foundational exercise that focuses on core stability and strength. The easy level allows beginners to develop proper form and endurance in this essential position. To perform an easy plank, lie face down, then lift your body off the ground, balancing on your forearms and toes, keeping your body in a straight line.\n\n" +
                      "This level emphasizes building a strong core, which is crucial for overall stability and injury prevention. Holding the plank for short durations initially can help beginners gradually increase their endurance and confidence in the movement.\n\n" +
                      "Incorporating planking into your routine will lead to improved posture and core strength, laying a solid foundation for more advanced variations in the future.";

                case "Planking__Medium_F":
                    return "                            PLANKING (MEDIUM)\n\n" +
                      "Description:\n" +
                      "The medium level of planking increases the challenge by extending the duration and introducing variations that further engage different muscle groups. This progression helps to build on the foundational strength gained in the easy level.\n\n" +
                      "To perform, maintain a forearm plank position while focusing on your breathing and core engagement. You can also add dynamic movements, such as leg lifts or arm reaches, to enhance the challenge and incorporate more stability work.\n\n" +
                      "This level promotes greater core strength, balance, and endurance, making it a vital part of any fitness regimen aimed at enhancing overall physical performance.";

                case "Planking__Hard_F":
                    return "                            PLANKING (HARD)\n\n" +
                      "Description:\n" +
                      "The hard level of planking challenges advanced exercisers by incorporating variations that significantly engage the core and other muscle groups. This includes longer hold times and more dynamic movements to elevate the intensity.\n\n" +
                      "To perform, you can transition from a forearm plank to a full plank, or incorporate movements like shoulder taps and plank jacks while maintaining proper alignment and core engagement. These variations not only increase difficulty but also promote greater functional strength.\n\n" +
                      "This level enhances overall body strength, stability, and endurance, making it a key component in advanced workout routines aimed at maximizing core and muscular fitness.";

                case "Reverse_Lunges__Easy_F":
                    return "                            REVERSE LUNGES (EASY)\n\n" +
                      "Description:\n" +
                      "Reverse lunges are a fundamental lower body exercise that targets the glutes, quadriceps, and hamstrings. In the easy level, the focus is on mastering proper form and balance. To perform a reverse lunge, step back with one leg while keeping the other foot planted, lowering your body until both knees are bent at a 90-degree angle.\n\n" +
                      "This exercise helps to improve lower body strength and stability, making it an essential part of any workout routine. Beginners should concentrate on slow, controlled movements to ensure safety and effectiveness, gradually increasing the range of motion as confidence builds.\n\n" +
                      "Incorporating reverse lunges into your fitness routine will help enhance muscle tone, functional strength, and balance, laying the groundwork for more advanced variations.";

                case "Reverse_Lunges__Medium_F":
                    return "                            REVERSE LUNGES (MEDIUM)\n\n" +
                      "Description:\n" +
                      "At the medium level, reverse lunges become more challenging by increasing the depth of the lunge and incorporating weights if desired. This progression emphasizes building strength and stability in the lower body.\n\n" +
                      "To perform, execute the reverse lunge as before, ensuring that your front knee does not extend past your toes. You can also add dumbbells for added resistance to further engage the muscles and enhance the workout's effectiveness.\n\n" +
                      "This level helps to improve balance and coordination while continuing to build strength in the legs, making it a valuable addition to a comprehensive fitness program.";

                case "Reverse_Lunges__Hard_F":
                    return "                            REVERSE LUNGES (HARD)\n\n" +
                      "Description:\n" +
                      "The hard level of reverse lunges challenges advanced exercisers with longer sets and explosive movements. This variation further increases strength, power, and endurance in the lower body.\n\n" +
                      "To perform, execute the reverse lunge and add a jump as you come back to the starting position, engaging your core for stability. This plyometric element not only intensifies the workout but also enhances cardiovascular fitness.\n\n" +
                      "Incorporating hard reverse lunges into your routine will significantly improve lower body strength, coordination, and overall athletic performance.";

                case "Russian_Twist__Easy_F":
                    return "                            RUSSIAN TWIST (EASY)\n\n" +
                      "Description:\n" +
                      "The Russian twist is a core exercise that focuses on rotational strength and stability. In the easy level, the emphasis is on maintaining proper form and controlled movement. To perform, sit on the ground with your knees bent, lean back slightly, and rotate your torso side to side, tapping the ground beside you with your hands.\n\n" +
                      "This exercise targets the obliques and helps improve core stability, making it crucial for various athletic activities. Beginners should focus on engaging their core and performing the twist slowly to ensure effectiveness and reduce the risk of injury.\n\n" +
                      "Incorporating Russian twists into your workout routine will enhance core strength and improve balance, providing a solid foundation for more advanced core exercises.";

                case "Russian_Twist__Medium_F":
                    return "                            RUSSIAN TWIST (MEDIUM)\n\n" +
                      "Description:\n" +
                      "At the medium level, the Russian twist becomes more challenging by increasing the duration and adding resistance, such as a weight or medicine ball. This progression emphasizes building strength and endurance in the core.\n\n" +
                      "To perform, maintain the seated position while holding the weight and rotating your torso from side to side. Ensure your movements are controlled to engage the core effectively while maintaining proper posture.\n\n" +
                      "This level further enhances core strength, improving overall stability and function, making it an essential exercise in any fitness routine.";

                case "Russian_Twist__Hard_F":
                    return "                            RUSSIAN TWIST (HARD)\n\n" +
                      "Description:\n" +
                      "The hard level of the Russian twist introduces advanced variations that challenge your core and improve functional strength. This includes longer sets and more dynamic movements, such as incorporating a twist with a press.\n\n" +
                      "To perform, use a heavier weight while maintaining the twisting motion and add an overhead press as you rotate. This not only increases the challenge but also engages the shoulders and arms, providing a comprehensive workout for the upper body.\n\n" +
                      "Incorporating hard Russian twists into your routine will significantly enhance core strength and overall athletic performance.";

                case "Side_Lunges__Easy_F":
                    return "                            SIDE LUNGES (EASY)\n\n" +
                      "Description:\n" +
                      "Side lunges are an excellent lower body exercise that targets the inner and outer thighs, glutes, and quadriceps. The easy level focuses on mastering proper form and balance. To perform, step to the side with one leg while keeping the other leg straight, bending the knee of the lunging leg and lowering your body.\n\n" +
                      "This exercise helps to improve lateral strength and flexibility, making it essential for overall leg development. Beginners should focus on slow, controlled movements to ensure safety and effectiveness, gradually increasing their range of motion.\n\n" +
                      "Incorporating side lunges into your fitness routine will enhance muscle tone, functional strength, and balance, laying the groundwork for more advanced variations.";

                case "Side_Lunges__Medium_F":
                    return "                            SIDE LUNGES (MEDIUM)\n\n" +
                      "Description:\n" +
                      "At the medium level, side lunges increase in difficulty by deepening the lunge and incorporating weights if desired. This progression emphasizes building strength and stability in the lower body.\n\n" +
                      "To perform, execute the side lunge while ensuring proper form and alignment, and consider using dumbbells for added resistance. This will help engage the muscles further and enhance the effectiveness of the exercise.\n\n" +
                      "This level helps to improve lateral strength and coordination, making it a valuable addition to a comprehensive fitness program.";

                case "Side_Lunges__Hard_F":
                    return "                            SIDE LUNGES (HARD)\n\n" +
                      "Description:\n" +
                      "The hard level of side lunges challenges advanced exercisers with longer sets and explosive movements. This variation further increases strength, power, and endurance in the lower body.\n\n" +
                      "To perform, execute the side lunge and add a jump as you return to the starting position, engaging your core for stability. This plyometric element intensifies the workout and improves cardiovascular fitness.\n\n" +
                      "Incorporating hard side lunges into your routine will significantly improve lateral strength, coordination, and overall athletic performance.";

                case "Side_Plank__Easy_F":
                    return "                            SIDE PLANK (EASY)\n\n" +
                      "Description:\n" +
                      "The side plank is an excellent exercise for building core stability and strength, targeting the obliques and shoulders. The easy level focuses on mastering proper form and balance. To perform, lie on your side with your feet stacked, then lift your body off the ground, balancing on your forearm and the side of your foot.\n\n" +
                      "This exercise helps improve core strength and stability, which is crucial for overall fitness and injury prevention. Beginners should concentrate on holding the position for short durations while maintaining proper alignment.\n\n" +
                      "Incorporating side planks into your routine will enhance overall core strength and stability, laying the foundation for more advanced variations in the future.";

                case "Side_Plank__Medium_F":
                    return "                            SIDE PLANK (MEDIUM)\n\n" +
                      "Description:\n" +
                      "At the medium level, the side plank becomes more challenging by extending the duration and introducing variations that further engage the core. This progression emphasizes building strength and stability in the obliques and shoulders.\n\n" +
                      "To perform, hold the side plank position while focusing on your breathing and core engagement. You can also add leg lifts or arm reaches to enhance the challenge and incorporate more stability work.\n\n" +
                      "This level promotes greater core strength, balance, and endurance, making it a vital part of any fitness regimen aimed at enhancing overall physical performance.";

                case "Side_Plank__Hard_F":
                    return "                            SIDE PLANK (HARD)\n\n" +
                      "Description:\n" +
                      "The hard level of the side plank challenges advanced exercisers with longer hold times and more dynamic movements. This variation significantly engages the core and upper body muscles.\n\n" +
                      "To perform, you can transition from a side plank to a full plank or incorporate movements like hip dips while maintaining proper alignment. These variations not only increase difficulty but also promote greater functional strength.\n\n" +
                      "This level enhances overall body strength, stability, and endurance, making it a key component in advanced workout routines aimed at maximizing core and muscular fitness.";

                case "Squat__Easy_F":
                    return "                            SQUAT (EASY)\n\n" +
                      "Description:\n" +
                      "Squats are a fundamental lower body exercise that targets the glutes, quadriceps, and hamstrings. The easy level focuses on mastering proper form and technique. To perform an easy squat, stand with your feet shoulder-width apart, lower your body by bending your knees while keeping your chest up and back straight.\n\n" +
                      "This exercise helps improve lower body strength, stability, and flexibility, making it an essential part of any workout routine. Beginners should concentrate on slow, controlled movements to ensure safety and effectiveness, gradually increasing the depth of their squats as confidence builds.\n\n" +
                      "Incorporating squats into your fitness routine will enhance muscle tone, functional strength, and overall fitness, laying the groundwork for more advanced variations.";

                case "Squat__Medium_F":
                    return "                            SQUAT (MEDIUM)\n\n" +
                      "Description:\n" +
                      "At the medium level, squats become more challenging by increasing the depth of the squat and incorporating weights if desired. This progression emphasizes building strength and stability in the lower body.\n\n" +
                      "To perform, execute the squat while ensuring proper form and alignment, and consider using dumbbells or a barbell for added resistance. This will help engage the muscles further and enhance the effectiveness of the exercise.\n\n" +
                      "This level improves lower body strength, coordination, and stability, making it a valuable addition to a comprehensive fitness program.";

                case "Squat__Hard_F":
                    return "                            SQUAT (HARD)\n\n" +
                      "Description:\n" +
                      "The hard level of squats challenges advanced exercisers with longer sets and explosive movements. This variation further increases strength, power, and endurance in the lower body.\n\n" +
                      "To perform, execute the squat and add a jump as you come back up to the starting position, engaging your core for stability. This plyometric element intensifies the workout and improves cardiovascular fitness.\n\n" +
                      "Incorporating hard squats into your routine will significantly improve lower body strength, coordination, and overall athletic performance.";

                case "Standing_Side_Leg_Raises__Easy_F":
                    return "                            STANDING SIDE LEG RAISES (EASY)\n\n" +
                      "Description:\n" +
                      "Standing side leg raises are a fantastic exercise for targeting the hip muscles, glutes, and improving balance. The easy level focuses on mastering proper form and control. To perform, stand tall and lift one leg out to the side while keeping your upper body stable.\n\n" +
                      "This exercise helps enhance hip strength and stability, making it crucial for overall lower body development. Beginners should focus on slow, controlled movements to ensure safety and effectiveness, gradually increasing the height of the leg raise as confidence builds.\n\n" +
                      "Incorporating standing side leg raises into your fitness routine will improve hip strength, functional stability, and overall leg development, laying the groundwork for more advanced variations.";

                case "Standing_Side_Leg_Raises__Medium__F":
                    return "                            STANDING SIDE LEG RAISES (MEDIUM)\n\n" +
                      "Description:\n" +
                      "At the medium level, standing side leg raises increase in difficulty by adding resistance, such as ankle weights. This progression emphasizes building strength and stability in the hip and leg muscles.\n\n" +
                      "To perform, execute the leg raise while maintaining proper posture and control. The added resistance will engage the muscles further and enhance the effectiveness of the exercise.\n\n" +
                      "This level promotes greater hip strength, balance, and coordination, making it a valuable addition to a comprehensive fitness program.";

                case "Standing_Side_Leg_Raises__Hard_F":
                    return "                            STANDING SIDE LEG RAISES (HARD)\n\n" +
                      "Description:\n" +
                      "The hard level of standing side leg raises challenges advanced exercisers with longer sets and more dynamic movements. This variation significantly engages the hip and glute muscles.\n\n" +
                      "To perform, you can incorporate a resistance band or perform the leg raise on an unstable surface to increase the challenge. These variations not only increase difficulty but also promote greater functional strength and stability.\n\n" +
                      "This level enhances overall lower body strength, coordination, and balance, making it a key component in advanced workout routines aimed at maximizing hip and leg fitness.";

                case "Step_Up__Easy_F":
                    return "                            STEP UP (EASY)\n\n" +
                      "Description:\n" +
                      "The step-up exercise is an excellent lower body workout that targets the glutes, quadriceps, and hamstrings. The easy level focuses on mastering proper form and technique. To perform a step-up, find a sturdy step or platform, step up with one foot, and then bring the other foot up, ensuring a controlled movement throughout.\n\n" +
                      "This exercise helps improve lower body strength, coordination, and stability, making it an essential part of any workout routine. Beginners should focus on slow, controlled movements to ensure safety and effectiveness, gradually increasing the height of the step as confidence builds.\n\n" +
                      "Incorporating step-ups into your fitness routine will enhance muscle tone, functional strength, and overall fitness, laying the groundwork for more advanced variations.";

                case "Step_Up__Medium_F":
                    return "                            STEP UP (MEDIUM)\n\n" +
                      "Description:\n" +
                      "At the medium level, step-ups become more challenging by increasing the height of the step and incorporating weights if desired. This progression emphasizes building strength and stability in the lower body.\n\n" +
                      "To perform, execute the step-up while holding dumbbells or a barbell for added resistance. This will engage the muscles further and enhance the effectiveness of the exercise.\n\n" +
                      "This level improves lower body strength, coordination, and stability, making it a valuable addition to a comprehensive fitness program.";

                case "Step_Up__Hard_F":
                    return "                            STEP UP (HARD)\n\n" +
                      "Description:\n" +
                      "The hard level of step-ups challenges advanced exercisers with longer sets and explosive movements. This variation further increases strength, power, and endurance in the lower body.\n\n" +
                      "To perform, execute the step-up and add a jump as you come back down to the starting position, engaging your core for stability. This plyometric element intensifies the workout and improves cardiovascular fitness.\n\n" +
                      "Incorporating hard step-ups into your routine will significantly improve lower body strength, coordination, and overall athletic performance.";

                case "Toe_Top__Easy_F":
                    return "                            TOE TOP (EASY)\n\n" +
                      "Description:\n" +
                      "Toe tops are an excellent exercise for improving lower body flexibility, coordination, and core strength. In the easy level, the focus is on mastering the basic movement while ensuring proper form and balance. To perform a toe top, stand tall with your feet shoulder-width apart, then lift one leg and touch your toes with your hand, alternating legs with each repetition.\n\n" +
                      "This exercise helps enhance flexibility in the hamstrings and calves, while also engaging the core for stability. Beginners should concentrate on slow, controlled movements to ensure safety and effectiveness, gradually increasing their range of motion as they become more comfortable with the movement.\n\n" +
                      "Incorporating toe tops into your fitness routine will improve flexibility, balance, and overall body awareness, laying the groundwork for more advanced variations.";

                case "Toe_Top__Medium_F":
                    return "                            TOE TOP (MEDIUM)\n\n" +
                      "Description:\n" +
                      "At the medium level, toe tops become more challenging by increasing the speed and duration of the exercise. This progression emphasizes building coordination and core strength.\n\n" +
                      "To perform, execute the toe top as before, but aim for a quicker pace while maintaining proper form. You can also incorporate a slight squat or lunge as you reach for your toes to enhance the challenge further.\n\n" +
                      "This level helps to improve overall flexibility and strength in the lower body while increasing the heart rate, making it a valuable addition to a comprehensive fitness program.";

                case "Toe_Top__Hard_F":
                    return "                            TOE TOP (HARD)\n\n" +
                      "Description:\n" +
                      "The hard level of toe tops challenges advanced exercisers with more dynamic movements and longer sets. This variation significantly enhances coordination, balance, and core strength.\n\n" +
                      "To perform, you can add a jumping element, such as jumping as you reach for your toes, or perform the exercise on an unstable surface to increase the challenge. These variations not only boost difficulty but also promote greater functional strength and stability.\n\n" +
                      "Incorporating hard toe tops into your routine will significantly improve overall flexibility, coordination, and athletic performance.";

                case "Wall_Sit__Easy_F":
                    return "                            WALL SIT (EASY)\n\n" +
                      "Description:\n" +
                      "The wall sit is a simple yet effective exercise that targets the muscles of the thighs, glutes, and core. At the easy level, this exercise is performed by standing with your back against a wall and sliding down until your thighs are parallel to the ground. This position should be held for a specified duration, typically starting from 15 to 30 seconds.\n\n" +
                      "This exercise helps build lower body strength and endurance, making it an excellent addition to any fitness routine. For beginners, it is essential to focus on maintaining proper form, ensuring that the knees do not extend beyond the toes and that the back is pressed firmly against the wall.\n\n" +
                      "As you become more comfortable with the movement, try to gradually increase the time spent in the wall sit position, which will enhance your strength and stability.";

                case "Wall_Sit__Medium_F":
                    return "                            WALL SIT (MEDIUM)\n\n" +
                      "Description:\n" +
                      "At the medium level, wall sits involve increased time or additional challenges, such as holding weights in your hands or performing calf raises while maintaining the wall sit position. This progression increases the intensity of the exercise, engaging more muscle fibers and enhancing overall strength.\n\n" +
                      "During a medium wall sit, you should focus on your breathing and posture, ensuring that your core is engaged and your back remains straight against the wall. The goal is to hold the position for longer, usually between 30 to 60 seconds.\n\n" +
                      "Incorporating medium wall sits into your routine will not only improve your leg strength but also enhance muscular endurance, preparing you for more advanced lower body workouts.";

                case "Wall_Sit__Hard_F":
                    return "                            WALL SIT (HARD)\n\n" +
                      "Description:\n" +
                      "The hard level of wall sits presents a significant challenge, combining increased duration and advanced variations. To perform a hard wall sit, aim to hold the position for 60 seconds or longer while also incorporating movements, such as alternating leg raises or even adding resistance bands around the thighs for added intensity.\n\n" +
                      "This advanced variation not only targets the major muscles of the lower body but also engages the core and improves balance. Maintaining proper form is crucial; ensure your knees remain behind your toes and your back is flat against the wall.\n\n" +
                      "Including hard wall sits in your workout routine will significantly enhance lower body strength, stability, and muscular endurance, preparing you for more complex movements in your fitness journey.";

                //Health
                case "Gratitude__Easy_H":
                    return "                            GRATITUDE (EASY)\n\n" +
                      "Description:\n" +
                      "The 'Gratitude' challenge encourages participants to reflect on the positive aspects of their lives by writing a short essay about what they are thankful for. " +
                      "The easy version focuses on expressing gratitude for simple, everyday blessings, making it an accessible and uplifting task for anyone.\n\n" +
                      "This exercise promotes mental well-being by shifting attention to positive experiences, cultivating a sense of appreciation, and reducing negative thought patterns. " +
                      "Participants are encouraged to take their time and focus on genuine gratitude, which helps build a habit of positive reflection.\n\n" +
                      "Practicing gratitude regularly enhances overall happiness and emotional resilience, providing a simple but powerful tool to improve one's perspective on life.";

                case "Gratitude__Medium_H":
                    return "                            GRATITUDE (MEDIUM)\n\n" +
                      "Description:\n" +
                      "The 'Gratitude' challenge at the medium level requires participants to write a more detailed essay about meaningful moments or relationships that have had a profound impact. " +
                      "This version encourages deeper introspection and thoughtful acknowledgment of specific events or people that have made a difference in their lives.\n\n" +
                      "By focusing on significant sources of gratitude, participants strengthen emotional connections and foster a greater appreciation for the supportive elements of their life. " +
                      "This exercise challenges participants to reflect beyond surface-level blessings and dive into the underlying reasons for their gratitude.\n\n" +
                      "Engaging in this level of gratitude can lead to stronger interpersonal relationships and a deeper understanding of personal values, making it a transformative experience.";

                case "Gratitude__Hard_H":
                    return "                            GRATITUDE (HARD)\n\n" +
                      "Description:\n" +
                      "The 'Gratitude' challenge at the hard level pushes participants to write a comprehensive essay exploring gratitude for challenges and difficulties they've overcome. " +
                      "This advanced version highlights the ability to find positivity even in adversity, fostering a mindset of growth and resilience.\n\n" +
                      "By reflecting on tough experiences and acknowledging the lessons learned, participants build emotional strength and transform their perspective on hardships. " +
                      "This exercise encourages self-awareness, helping individuals recognize the hidden blessings in their struggles and how they have shaped them.\n\n" +
                      "Practicing gratitude at this level develops a profound sense of optimism and inner strength, paving the way for a more balanced and fulfilling outlook on life.";


                case "No_Blinking__Easy_H":
                    return "                            NO BLINKING (EASY)\n\n" +
                      "Description:\n" +
                      "The 'No Blinking' challenge tests eye control and concentration by requiring participants to keep their eyes open without blinking for a set period. " +
                      "In the easy version, the time is shorter, making it a manageable start for beginners, helping them improve focus and resist minor distractions.\n\n" +
                      "This exercise engages mental focus and determination, as the eyes naturally want to blink to stay moisturized. Practicing this can help enhance " +
                      "discipline and awareness, providing a fun way to develop a greater level of concentration.\n\n" +
                      "Performing the no-blinking challenge regularly can improve eye stamina and patience, laying the foundation for higher endurance in eye control and focus tasks.";

                case "No_Blinking__Medium_H":
                    return "                            NO BLINKING (MEDIUM)\n\n" +
                      "Description:\n" +
                      "The medium no-blinking challenge increases the time duration, making it more challenging to resist blinking. This level tests mental endurance and eye control " +
                      "to a higher degree, requiring focus to prevent involuntary blinks.\n\n" +
                      "By maintaining eye focus for a longer period, participants strengthen their concentration and self-control. This exercise also promotes awareness of natural " +
                      "eye movements and enhances control over them, making it useful for tasks that require steady focus.\n\n" +
                      "Consistent practice at this level builds mental discipline and resilience, providing a medium-intensity mental workout that reinforces patience and concentration.";

                case "No_Blinking__Hard_H":
                    return "                            NO BLINKING (HARD)\n\n" +
                      "Description:\n" +
                      "The hard level of the no-blinking challenge is designed to push the boundaries of eye control and concentration. " +
                      "Participants must resist the natural urge to blink for an extended duration, requiring intense focus and self-control.\n\n" +
                      "This high-intensity challenge helps to cultivate mental strength and endurance, as participants learn to ignore discomfort and stay focused on the task. " +
                      "It is also beneficial for training focus under pressure, as it requires resisting strong natural impulses.\n\n" +
                      "This difficult challenge can serve as a unique exercise in mental resilience, concentration, and patience, making it a valuable addition for anyone seeking to strengthen " +
                      "their mental and focus-related capabilities.";

                case "Hold_Your_Breath__Easy_H":
                    return "                            HOLD BREATH (EASY)\n\n" +
                      "Description:\n" +
                      "The hold breath challenge at the easy level encourages participants to focus on their breathing techniques and lung capacity. This exercise starts with a deep inhale, followed by holding the breath for a duration of 10 to 15 seconds. It serves as an excellent introduction to breath control, which is beneficial for overall respiratory health.\n\n" +
                      "Practicing breath holds can help increase oxygen efficiency and improve focus. For beginners, it is essential to remain relaxed and calm throughout the process, ensuring not to strain the body. This practice can also be a great way to enhance mindfulness and reduce stress levels by promoting a sense of tranquility.\n\n" +
                      "As you become more comfortable with this exercise, it can be seamlessly integrated into daily routines, enhancing both physical and mental well-being, setting the foundation for more advanced breath-holding techniques.";

                case "Hold_Your_Breath__Medium_H":
                    return "                            HOLD BREATH (MEDIUM)\n\n" +
                      "Description:\n" +
                      "At the medium level, the hold breath challenge intensifies by increasing the duration of the breath hold to 20 to 30 seconds. This exercise further develops lung capacity and introduces participants to the concept of breath control under mild stress. It also enhances awareness of one's body responses during breath retention.\n\n" +
                      "Practicing medium-level breath holds can improve cardiovascular efficiency and increase stamina, making it an excellent addition to fitness routines. Participants should focus on maintaining a relaxed state, controlling any feelings of discomfort, and gently pushing their limits while being mindful of their bodies.\n\n" +
                      "Incorporating this challenge into regular workouts can lead to improved breathing patterns during physical activities, benefiting overall athletic performance and enhancing mental clarity.";

                case "Hold_Your_Breath__Hard_H":
                    return "                            HOLD BREATH (HARD)\n\n" +
                      "Description:\n" +
                      "The hard level of the hold breath challenge pushes participants to hold their breath for 30 seconds or longer. This advanced exercise demands a strong mental focus and physical resilience, enhancing lung capacity significantly. It's recommended that individuals practice this level after mastering the previous stages to ensure proper technique and safety.\n\n" +
                      "During the hard level, individuals should pay close attention to their body's signals, ensuring they do not exceed safe limits. This stage builds mental toughness and promotes awareness of how the body reacts to prolonged breath retention. The challenge can be further intensified by incorporating movements, such as light exercises, while holding the breath.\n\n" +
                      "Regularly practicing hard-level breath holds can lead to substantial improvements in physical fitness, respiratory efficiency, and stress management. This challenge serves as a powerful tool for enhancing athletic performance and promoting a deeper connection with one's breath.";

                case "Hydration__Easy_H":
                    return "                            HYDRATION (EASY)\n\n" +
                      "Description:\n" +
                      "The easy hydration challenge encourages participants to increase their water intake gradually. This level focuses on establishing a baseline for daily hydration needs and emphasizes the importance of drinking water throughout the day. Participants are guided to drink at least eight glasses of water daily, making it a manageable and achievable goal.\n\n" +
                      "By completing this easy challenge, individuals develop a routine that can significantly enhance their overall health. Proper hydration supports vital bodily functions, improves skin health, and boosts energy levels. Participants are also encouraged to notice how increased water consumption positively affects their well-being, such as reducing fatigue and improving concentration.\n\n" +
                      "As individuals become more accustomed to drinking enough water, they build a solid foundation for healthier habits. This easy challenge serves as a stepping stone toward more advanced hydration goals, promoting long-term benefits and encouraging participants to stay mindful of their hydration habits.";

                case "Hydration__Medium_H":
                    return "                            HYDRATION (MEDIUM)\n\n" +
                      "Description:\n" +
                      "The medium hydration challenge takes the goal of daily water intake a step further, requiring participants to consume water consistently throughout the day. This level encourages drinking water before meals, during workouts, and at regular intervals to ensure optimal hydration levels. Participants are challenged to increase their daily intake to ten or more glasses of water, making it a more significant commitment to their health.\n\n" +
                      "Engaging in this medium challenge not only reinforces the importance of hydration but also helps participants recognize the signs of dehydration. Maintaining adequate hydration supports metabolism, enhances physical performance, and aids in digestion. As individuals track their progress, they may notice improvements in energy levels, mental clarity, and overall vitality.\n\n" +
                      "The medium hydration challenge serves as a practical approach to instill the habit of mindful water consumption, setting the stage for participants to further improve their hydration strategies. It builds upon the foundation established in the easy level and encourages participants to make hydration a priority in their daily routines.";

                case "Hydration__Hard_H":
                    return "                            HYDRATION (HARD)\n\n" +
                      "Description:\n" +
                      "The hard hydration challenge presents a rigorous approach to water intake, pushing participants to consistently meet their hydration goals throughout the day. In this level, individuals are challenged to drink a minimum of twelve glasses of water daily, integrating hydration into their exercise routines and daily activities. Participants must plan their water consumption strategically, ensuring they maintain hydration during physical exertion and throughout various tasks.\n\n" +
                      "This advanced challenge highlights the critical role of hydration in enhancing athletic performance, improving recovery, and optimizing overall health. By committing to this level, participants become more attuned to their body's hydration needs, recognizing how it impacts energy, mood, and cognitive function. The challenge also emphasizes the importance of adjusting water intake based on activity levels and environmental factors.\n\n" +
                      "Completing the hard hydration challenge not only fosters a deep understanding of hydration's significance but also cultivates lifelong habits that can lead to improved health outcomes. This level empowers individuals to take ownership of their hydration strategies, ensuring they remain hydrated in various circumstances and encouraging sustained wellness.";

                case "Mindful_Breathing__Easy_H":
                    return "                            MINDFUL BREATHING (EASY)\n\n" +
                      "Description:\n" +
                      "The easy mindful breathing challenge introduces participants to the basics of conscious breathing techniques. This level encourages individuals to practice deep breathing for a few minutes each day, focusing on inhaling through the nose and exhaling through the mouth. The aim is to create a sense of calm and relaxation, helping participants to become more aware of their breath and its impact on their mental state.\n\n" +
                      "By engaging in this simple exercise, individuals learn to manage stress and anxiety effectively. The practice promotes relaxation and mindfulness, encouraging participants to take a moment to pause and center themselves amidst daily distractions. With regular practice, individuals can experience improvements in emotional well-being and enhanced mental clarity, laying the groundwork for deeper mindfulness techniques.\n\n" +
                      "The easy mindful breathing challenge serves as an accessible entry point for those new to mindfulness. It fosters a habit of self-reflection and awareness, encouraging participants to incorporate breathing exercises into their daily routines for improved mental health.";

                case "Mindful_Breathing__Medium_H":
                    return "                            MINDFUL BREATHING (MEDIUM)\n\n" +
                      "Description:\n" +
                      "The medium mindful breathing challenge builds on the foundational practices learned in the easy level, introducing participants to more advanced breathing techniques. This level encourages individuals to practice focused breathing for longer periods, aiming for ten to fifteen minutes daily. Participants are guided to explore techniques such as box breathing or diaphragmatic breathing, enhancing their ability to regulate their breath and promote relaxation.\n\n" +
                      "By committing to this medium challenge, individuals deepen their understanding of how breath affects their physical and emotional states. This exercise promotes a heightened sense of mindfulness, encouraging participants to remain present and engaged in their breathing patterns. The medium level also helps individuals cultivate resilience, allowing them to manage stress and anxiety more effectively in everyday situations.\n\n" +
                      "Through consistent practice, participants may notice significant improvements in their overall well-being, including increased focus, reduced tension, and a greater sense of calm. This medium challenge serves to enhance the participants' mindfulness journey, setting them up for more advanced practices in the future.";

                case "Mindful_Breathing__Hard_H":
                    return "                            MINDFUL BREATHING (HARD)\n\n" +
                      "Description:\n" +
                      "The hard mindful breathing challenge represents an advanced level of practice, pushing participants to incorporate longer sessions and complex breathing techniques into their routines. Participants are encouraged to dedicate twenty minutes or more each day to mindful breathing, exploring various methods such as alternate nostril breathing or extended exhalation techniques. This level emphasizes the connection between breath and mental clarity, enabling participants to achieve a deeper state of relaxation.\n\n" +
                      "Engaging in the hard mindful breathing challenge helps participants develop a profound awareness of their mental and physical states. By mastering their breath, individuals can effectively manage stress, enhance focus, and improve emotional regulation. This level of practice not only cultivates mindfulness but also empowers participants to apply these techniques in challenging situations, fostering resilience and adaptability.\n\n" +
                      "Completing the hard challenge solidifies participants' commitment to mindfulness and promotes a lifestyle centered around mental well-being. The skills developed during this challenge can lead to long-term benefits, including improved emotional health and a heightened sense of self-awareness, encouraging individuals to continue their journey toward mindfulness beyond the structured challenges.";

                case "Mindful_Eating__Easy_H":
                    return "                            MINDFUL EATING (EASY)\n\n" +
                      "Description:\n" +
                      "The easy mindful eating challenge introduces participants to the concept of savoring food and being fully present during meals. This level encourages individuals to take their time while eating, focusing on the textures, flavors, and aromas of each bite. Participants are prompted to eliminate distractions, such as screens or multitasking, to foster a deeper connection with their food and promote a sense of gratitude for what they consume.\n\n" +
                      "By engaging in this simple practice, individuals learn to appreciate their meals more fully and develop a healthier relationship with food. The challenge promotes awareness of hunger and fullness cues, empowering participants to listen to their bodies and eat according to their needs. With consistent practice, participants may experience a reduction in overeating and an improved sense of satisfaction during meals.\n\n" +
                      "The easy mindful eating challenge serves as an accessible entry point for those looking to improve their eating habits. It encourages individuals to slow down, reflect, and enjoy the sensory experience of eating, laying the groundwork for more advanced mindful eating practices.";

                case "Mindful_Eating__Medium_H":
                    return "                            MINDFUL EATING (MEDIUM)\n\n" +
                      "Description:\n" +
                      "The medium mindful eating challenge builds upon the basic principles of the easy level, introducing participants to more structured practices for enhancing their eating experiences. This level encourages individuals to extend their mindful eating sessions, focusing on specific meals or snacks and taking time to reflect on the origins of their food, the effort that went into preparing it, and its nutritional value. Participants are guided to practice gratitude before meals, fostering a deeper connection to their food.\n\n" +
                      "By committing to this medium challenge, individuals develop a greater understanding of how their eating habits affect their overall health and well-being. This exercise promotes self-awareness, helping participants recognize emotional triggers related to food and empowering them to make conscious choices. The medium level also emphasizes the importance of portion control and mindful food selection, leading to healthier eating patterns over time.\n\n" +
                      "Through consistent practice, participants may notice improvements in their relationship with food, including enhanced satisfaction with meals, reduced cravings, and a greater appreciation for the nourishment they provide to their bodies. This medium challenge deepens the participants' journey into mindful eating, paving the way for more advanced techniques and practices.";

                case "Mindful_Eating__Hard_H":
                    return "                            MINDFUL EATING (HARD)\n\n" +
                      "Description:\n" +
                      "The hard mindful eating challenge represents an advanced stage of practice, pushing participants to fully integrate mindfulness into their daily eating habits. Participants are encouraged to dedicate time to every meal, exploring a variety of foods and mindful eating techniques such as intuitive eating and mindful cooking. This level emphasizes the importance of being present throughout the entire eating experience, from preparation to consumption, encouraging individuals to engage all their senses.\n\n" +
                      "Engaging in the hard mindful eating challenge helps participants cultivate a profound awareness of their body's signals and emotional responses related to food. By mastering mindful eating, individuals can effectively address issues such as emotional eating, binge eating, and food guilt. This level of practice not only promotes healthier eating habits but also empowers participants to make choices that align with their values and health goals.\n\n" +
                      "Completing the hard challenge solidifies participants' commitment to a mindful approach to eating and encourages long-term lifestyle changes. The skills developed during this challenge can lead to lasting benefits, including improved digestion, greater enjoyment of food, and a deeper sense of well-being, motivating individuals to continue their mindful eating journey beyond the structured challenges.";

                case "Quick_Stretching__Easy_H":
                    return "                            QUICK STRETCHING (EASY)\n\n" +
                      "Description:\n" +
                      "The easy quick stretching challenge is designed to introduce individuals to the benefits of incorporating short stretching sessions into their daily routines. This level emphasizes gentle stretches that are accessible to everyone, promoting flexibility and relaxation without requiring extensive time commitments. Participants are encouraged to take a few minutes throughout the day to perform simple stretches, targeting major muscle groups to relieve tension and improve circulation.\n\n" +
                      "By practicing quick stretching regularly, individuals can enhance their overall mobility and reduce the risk of injuries associated with prolonged periods of inactivity or sedentary behavior. This challenge fosters a sense of well-being and refreshes both the body and mind, making it easier to transition between tasks or to take breaks during the day. Participants often report feeling more energized and focused after completing these short stretching sessions.\n\n" +
                      "The easy level serves as a gateway for individuals new to stretching, allowing them to develop a habit that can lead to more advanced stretching practices. With consistent participation, individuals can experience increased range of motion, improved posture, and a greater sense of body awareness, setting a strong foundation for a healthy lifestyle.";

                case "Quick_Stretching__Medium_H":
                    return "                            QUICK STRETCHING (MEDIUM)\n\n" +
                      "Description:\n" +
                      "The medium quick stretching challenge builds upon the easy level by introducing more dynamic and varied stretches that enhance flexibility and strength. This level encourages participants to extend their stretching sessions and includes a mix of static and dynamic stretches that engage multiple muscle groups. The challenge prompts individuals to focus on form and breathing while performing these stretches, promoting greater physical awareness and control.\n\n" +
                      "Participants engaging in the medium challenge can expect to see improvements in their flexibility, balance, and overall physical performance. Regular practice of these stretches helps alleviate muscle stiffness and tension, particularly for those who lead active lifestyles or spend long hours sitting at a desk. The medium level encourages individuals to push their limits safely, fostering a deeper connection between their body and movement.\n\n" +
                      "Through this challenge, participants develop a more comprehensive stretching routine that can easily be integrated into their daily lives. This level not only promotes physical benefits but also enhances mental focus and relaxation, helping to reduce stress and improve overall well-being. Participants may find themselves feeling more refreshed and capable in their daily activities as they incorporate these medium-level stretches into their routine.";

                case "Quick_Stretching__Hard_H":
                    return "                            QUICK STRETCHING (HARD)\n\n" +
                      "Description:\n" +
                      "The hard quick stretching challenge represents an advanced approach to stretching, focusing on deepening flexibility, mobility, and strength. This level includes a variety of advanced stretches and techniques that require a higher level of skill and body awareness. Participants are encouraged to explore their limits while maintaining proper technique, ensuring that they perform stretches safely and effectively.\n\n" +
                      "Engaging in the hard challenge helps individuals develop a greater understanding of their body's capabilities and promotes resilience. By committing to this advanced level, participants can experience significant improvements in flexibility and athletic performance, making it an essential practice for athletes and fitness enthusiasts alike. This challenge not only enhances physical fitness but also contributes to better recovery and injury prevention.\n\n" +
                      "Completing the hard quick stretching challenge solidifies participants' commitment to their physical well-being and establishes a routine that promotes long-term health benefits. Participants often report feeling more agile, balanced, and prepared for physical activities after integrating these advanced stretches into their lives. The skills gained from this challenge empower individuals to take ownership of their physical fitness and inspire a lifelong journey of movement and health.";

                case "Plan_your_week__Easy_H":
                    return "                            PLAN YOUR WEEK (EASY)\n\n" +
                      "Description:\n" +
                      "The easy level of the 'Plan Your Week' challenge is designed to help individuals establish a structured approach to their weekly tasks and commitments. This level encourages participants to set aside time to outline their goals and responsibilities, ensuring a balanced distribution of work and leisure. By planning ahead, individuals can reduce stress and enhance productivity throughout the week.\n\n" +
                      "Participants engaging in this challenge will find that taking a few moments to plan their week allows them to prioritize essential tasks, allocate time effectively, and avoid last-minute rushes. This practice cultivates a sense of control over one's time, leading to improved time management skills and greater overall satisfaction. Individuals may also discover opportunities for self-care and relaxation, contributing to a more fulfilling week.\n\n" +
                      "As participants practice this easy level, they build a foundation for more complex planning strategies. By consistently planning their week, individuals can develop a routine that fosters organization, clarity, and a sense of accomplishment, ultimately leading to better work-life balance.";

                case "Plan_your_week__Medium_H":
                    return "                            PLAN YOUR WEEK (MEDIUM)\n\n" +
                      "Description:\n" +
                      "The medium level of the 'Plan Your Week' challenge delves deeper into effective planning strategies, encouraging participants to refine their organizational skills. At this level, individuals are prompted to not only outline their tasks but also to consider the time required for each activity and potential obstacles that may arise. This foresight promotes adaptability and better prepares participants for unexpected challenges throughout the week.\n\n" +
                      "Participants can expect to experience improved efficiency as they practice allocating time blocks for different activities, balancing work and personal commitments. This challenge also encourages individuals to reflect on their priorities and make conscious decisions about how they spend their time. By integrating self-assessment into their planning process, participants can identify areas for improvement and cultivate a mindset of continuous growth.\n\n" +
                      "Engaging in this medium challenge enables individuals to develop more advanced time management techniques that lead to increased productivity. Participants often report feeling more accomplished and focused, as they gain a clearer understanding of their goals and how to achieve them. This level reinforces the importance of proactive planning in achieving long-term objectives and maintaining a healthy balance between responsibilities and leisure.";

                case "Plan_your_week__Hard_H":
                    return "                            PLAN YOUR WEEK (HARD)\n\n" +
                      "Description:\n" +
                      "The hard level of the 'Plan Your Week' challenge elevates the planning process to a strategic level, focusing on long-term vision and goal-setting. Participants are encouraged to set ambitious objectives for the week while considering their overall life goals. This level emphasizes the importance of aligning daily tasks with broader aspirations, fostering a sense of purpose and direction.\n\n" +
                      "At this advanced stage, individuals will practice breaking down larger goals into manageable action steps, creating a clear roadmap for the week ahead. This approach enhances accountability and motivation, as participants can track their progress towards meaningful outcomes. The hard challenge promotes self-discipline and resilience, preparing individuals to face challenges head-on while staying committed to their objectives.\n\n" +
                      "By mastering the hard level of planning, participants can develop a comprehensive and sustainable approach to time management. They will find that this level not only contributes to increased productivity but also empowers them to cultivate a proactive mindset. As they consistently engage in this challenge, individuals often experience greater fulfillment and satisfaction as they align their daily actions with their long-term goals.";

                case "Plan_Your_Week_Mental__Easy_H":
                    return "                            PLAN YOUR MENTAL WEEK (EASY)\n\n" +
                      "Description:\n" +
                      "The easy level of the 'Plan Your Mental Week' challenge focuses on enhancing mental well-being through structured planning. Participants are encouraged to take time to reflect on their emotional and mental needs for the week ahead. By identifying potential stressors and planning for self-care activities, individuals can create a supportive framework for maintaining positive mental health.\n\n" +
                      "This challenge emphasizes the importance of mindfulness and intentionality in mental health planning. Participants may find it beneficial to schedule activities that promote relaxation, such as meditation, exercise, or engaging in hobbies. The easy level aims to cultivate a proactive approach to mental well-being, allowing individuals to prioritize their emotional health alongside their responsibilities.\n\n" +
                      "By consistently practicing this easy challenge, participants can establish healthy habits that contribute to resilience and emotional balance. Individuals often report feeling more centered and prepared for the week, as they integrate mental health considerations into their overall planning process.";

                case "Plan_Your_Week_Mental__Medium_H":
                    return "                            PLAN YOUR MENTAL WEEK (MEDIUM)\n\n" +
                      "Description:\n" +
                      "The medium level of the 'Plan Your Mental Week' challenge builds upon the foundations of mental health planning by encouraging participants to explore their feelings and thoughts in greater depth. This level prompts individuals to set specific mental health goals for the week, such as practicing gratitude or addressing negative thought patterns. Participants are encouraged to engage in reflective practices that enhance self-awareness and emotional intelligence.\n\n" +
                      "By committing to this medium challenge, individuals can create a balanced approach to their mental well-being. They may find that allocating time for mindfulness practices, journaling, or connecting with supportive individuals leads to a more fulfilling and positive mindset. This level reinforces the idea that mental health is an ongoing journey, requiring consistent attention and care.\n\n" +
                      "As participants engage in this challenge, they will likely experience improvements in their emotional resilience and coping strategies. This medium level encourages individuals to take ownership of their mental health, equipping them with tools to navigate challenges effectively and fostering a deeper connection to themselves and their experiences.";

                case "Plan_Your_Week_Mental__Hard_H":
                    return "                            PLAN YOUR MENTAL WEEK (HARD)\n\n" +
                      "Description:\n" +
                      "The hard level of the 'Plan Your Mental Week' challenge emphasizes advanced strategies for mental health planning and emotional growth. Participants are encouraged to reflect on their long-term mental health aspirations and create actionable plans to achieve them. This level focuses on setting ambitious yet achievable goals that align with personal values and desires for emotional well-being.\n\n" +
                      "At this advanced stage, individuals will practice creating comprehensive mental health strategies, including stress management techniques, coping mechanisms, and self-care rituals. The hard challenge promotes accountability, as participants track their progress towards their mental health goals, fostering a sense of achievement and self-discovery. This level empowers individuals to cultivate a resilient mindset, equipping them to face life's challenges with confidence and clarity.\n\n" +
                      "By engaging in the hard level of mental planning, participants not only enhance their emotional well-being but also contribute to personal growth and self-awareness. This challenge encourages individuals to view mental health as an integral aspect of overall well-being, reinforcing the importance of proactive care and continuous improvement in achieving a balanced, fulfilling life.";

                case "Quiz_Mental__Easy_H":
                    return "                            QUIZ MENTAL (EASY)\n\n" +
                      "Description:\n" +
                      "The easy level of the 'Quiz Mental' challenge is designed to enhance cognitive skills through engaging and interactive questions. This level focuses on basic mental exercises that stimulate critical thinking and memory recall. Participants will encounter straightforward questions that encourage them to think creatively while promoting a sense of fun and learning.\n\n" +
                      "By engaging with the quiz at this level, individuals can expect to improve their mental agility and quick thinking. The easy format allows participants to familiarize themselves with quiz structures, making it an excellent starting point for those looking to boost their mental capabilities. This challenge also promotes a positive learning environment, as participants can enjoy the process of discovery and knowledge acquisition.\n\n" +
                      "Consistent practice at this easy level fosters a habit of lifelong learning. Participants may find that this approach not only enhances their mental faculties but also builds confidence in their ability to tackle more challenging cognitive tasks in the future.";

                case "Quiz_Mental__Medium_H":
                    return "                            QUIZ MENTAL (MEDIUM)\n\n" +
                      "Description:\n" +
                      "The medium level of the 'Quiz Mental' challenge elevates the cognitive demands by introducing more complex questions and problem-solving scenarios. At this level, participants will be prompted to apply their knowledge and reasoning skills to tackle questions that require deeper understanding and analysis. This level is designed to sharpen critical thinking while providing an enjoyable learning experience.\n\n" +
                      "Engaging with the quiz at this medium level encourages individuals to stretch their mental capabilities and develop a strategic approach to answering questions. Participants may find that the challenge of tackling medium-difficulty questions enhances their focus and determination, ultimately leading to improved cognitive performance. This level serves as a bridge, preparing individuals for advanced mental exercises and reinforcing the value of persistence in learning.\n\n" +
                      "As participants progress through the medium quiz, they will likely experience greater satisfaction and achievement, as they recognize their improved ability to handle more intricate questions. This challenge not only enhances cognitive skills but also instills a growth mindset, empowering individuals to embrace new learning opportunities and challenges with confidence.";

                case "Quiz_Mental__Hard_H":
                    return "                            QUIZ MENTAL (HARD)\n\n" +
                      "Description:\n" +
                      "The hard level of the 'Quiz Mental' challenge presents participants with a series of rigorous questions designed to push cognitive boundaries. At this advanced stage, individuals will face intricate problem-solving scenarios that require high levels of analytical thinking, creativity, and mental endurance. This level is crafted to stimulate the brain, offering a rewarding and intellectually challenging experience.\n\n" +
                      "Engaging with the hard quiz encourages participants to refine their mental strategies and improve their ability to think critically under pressure. The complexity of questions at this level not only tests existing knowledge but also challenges individuals to think outside the box, fostering innovative problem-solving skills. Participants may find that the effort invested in tackling these hard questions leads to significant cognitive growth and development.\n\n" +
                      "By committing to the hard level of the quiz, participants will likely experience enhanced mental resilience and increased confidence in their cognitive abilities. This challenge serves as an excellent tool for personal development, allowing individuals to expand their intellectual horizons and develop a deeper understanding of various subjects through rigorous engagement and reflection.";

                case "Reading_Time__Easy_H":
                    return "                            READING TIME (EASY)\n\n" +
                      "Description:\n" +
                      "The easy level of the 'Reading Time' challenge is aimed at promoting a love for reading through accessible and engaging materials. Participants at this level will explore short texts or articles that are designed to be enjoyable and straightforward, making it perfect for building foundational reading skills. This level encourages individuals to develop a consistent reading habit without overwhelming them with complex content.\n\n" +
                      "By participating in this easy reading challenge, individuals can expect to improve their comprehension skills and expand their vocabulary in a relaxed setting. The focus is on enjoyment and exploration, allowing participants to discover various genres and topics that pique their interest. This foundational level fosters a positive attitude toward reading, promoting mental engagement and creativity.\n\n" +
                      "Consistent practice at the easy level reinforces the habit of reading regularly. Participants may find that this challenge not only enhances their reading abilities but also instills a lifelong appreciation for literature and learning.";

                case "Reading_Time__Medium_H":
                    return "                            READING TIME (MEDIUM)\n\n" +
                      "Description:\n" +
                      "The medium level of the 'Reading Time' challenge encourages participants to engage with more complex texts that require greater comprehension and critical analysis. At this level, individuals will explore a variety of topics and genres, prompting deeper thought and reflection. The medium reading challenge is designed to enhance analytical skills while maintaining a focus on enjoyment and discovery.\n\n" +
                      "As participants engage with medium-level reading materials, they will develop the ability to interpret and evaluate information critically. This level encourages individuals to think about the author's intent, themes, and perspectives, promoting a deeper understanding of the texts. The medium challenge aims to strike a balance between enjoyment and intellectual engagement, allowing readers to expand their knowledge while having fun.\n\n" +
                      "By consistently participating in the medium reading challenge, individuals will likely experience significant growth in their reading proficiency and critical thinking skills. This level fosters an environment where readers can explore new ideas and perspectives, empowering them to engage in meaningful discussions and reflections on the materials they consume.";

                case "Reading_Time__Hard_H":
                    return "                            READING TIME (HARD)\n\n" +
                      "Description:\n" +
                      "The hard level of the 'Reading Time' challenge presents participants with advanced texts that require deep analysis, synthesis, and evaluation of complex ideas. This level is crafted to challenge individuals' reading abilities, promoting critical thinking and comprehensive understanding of intricate subjects. Participants will encounter texts that stimulate intellectual curiosity and encourage thoughtful engagement with the material.\n\n" +
                      "By engaging with hard reading materials, individuals will develop their analytical skills and enhance their ability to articulate their thoughts and opinions. This level challenges participants to explore sophisticated themes and concepts, pushing them to think critically and draw connections between various ideas. The hard challenge fosters resilience and persistence, equipping individuals with the tools necessary for engaging with complex literature.\n\n" +
                      "Consistent practice at the hard level of reading can lead to profound cognitive growth and greater appreciation for nuanced narratives and arguments. Participants may find that this challenge not only enriches their knowledge but also empowers them to engage in sophisticated discussions and debates, reinforcing the value of reading as a tool for personal and intellectual development.";

                case "Take_a_Cold_Shower__Easy_H":
                    return "                            COLD SHOWER (EASY)\n\n" +
                      "Description:\n" +
                      "The easy level of the 'Cold Shower' challenge encourages participants to gradually acclimate to colder water temperatures. This level is ideal for those new to the concept, allowing them to experience the invigorating effects of cold showers without overwhelming discomfort. By starting with shorter durations, participants can build confidence and learn to appreciate the refreshing sensation.\n\n" +
                      "Engaging with cold showers at this easy level helps promote circulation and awaken the senses. The experience is often revitalizing, providing an energizing boost to start the day or refresh after a workout. Additionally, this level encourages mindfulness, as individuals focus on their breath and reactions to the cold water, enhancing mental clarity and presence.\n\n" +
                      "Regularly incorporating easy cold showers can lead to increased tolerance and readiness for more intense experiences. Participants may find that this practice not only enhances physical wellness but also cultivates resilience and a sense of achievement as they step out of their comfort zone.";

                case "Take_a_Cold_Shower__Medium_H":
                    return "                            COLD SHOWER (MEDIUM)\n\n" +
                      "Description:\n" +
                      "The medium level of the 'Cold Shower' challenge intensifies the experience by encouraging longer durations under colder water. This level is designed for individuals who are familiar with the benefits of cold exposure and are ready to challenge themselves further. By gradually increasing the time spent in cold water, participants can enhance their physical and mental resilience while experiencing the invigorating effects of the cold.\n\n" +
                      "Cold showers at the medium level promote improved circulation, boost immunity, and aid in muscle recovery after exercise. Participants will likely experience heightened alertness and improved mood, thanks to the release of endorphins triggered by the cold water. This level also encourages individuals to focus on their breathing and mental fortitude, making the experience a powerful exercise in mindfulness.\n\n" +
                      "As participants engage with the medium cold shower challenge, they will likely notice increased stamina and a greater sense of control over their physical responses. This level not only enhances the benefits of cold exposure but also instills a deeper understanding of personal limits and the ability to push beyond them.";

                case "Take_a_Cold_Shower__Hard_H":
                    return "                            COLD SHOWER (HARD)\n\n" +
                      "Description:\n" +
                      "The hard level of the 'Cold Shower' challenge pushes participants to fully embrace cold exposure for extended periods. This level is crafted for those who have mastered the basics and are ready to explore the full benefits of cold showers. Participants at this level may spend several minutes under cold water, significantly enhancing their physical and mental resilience.\n\n" +
                      "Engaging in hard cold showers promotes a range of health benefits, including improved circulation, reduced inflammation, and enhanced recovery after strenuous workouts. Participants will likely experience a surge of energy and clarity, as the body responds to the cold stimulus. This level also serves as an intense practice in mental discipline, as individuals must cultivate a strong mindset to endure and embrace the cold.\n\n" +
                      "Consistently practicing hard cold showers can lead to profound changes in both physical health and mental toughness. Participants may discover new levels of resilience and confidence, equipping them with the ability to face challenges both in and out of the shower with renewed vigor.";

                case "Walking_Easy_H":
                    return "                            WALKING (EASY)\n\n" +
                      "Description:\n" +
                      "The easy level of the 'Walking' challenge is designed for individuals looking to incorporate gentle movement into their daily routine. This level encourages leisurely walks that focus on enjoying the surroundings and promoting a sense of well-being. Participants can choose scenic routes or familiar areas, making the experience enjoyable and stress-relieving.\n\n" +
                      "Walking at this easy level provides numerous health benefits, including improved cardiovascular health, enhanced mood, and increased energy levels. It allows individuals to connect with nature and engage in mindfulness as they notice their environment. This level is especially suitable for beginners or those looking to ease into a more active lifestyle without feeling overwhelmed.\n\n" +
                      "Regularly practicing easy walks can help participants build a consistent routine, making physical activity a natural part of their day. This gentle approach fosters a positive relationship with movement, encouraging individuals to enjoy the process and cultivate a sense of achievement through their efforts.";

                case "Walking_Medium_H":
                    return "                            WALKING (MEDIUM)\n\n" +
                      "Description:\n" +
                      "The medium level of the 'Walking' challenge encourages participants to increase their pace and duration, providing a more invigorating walking experience. This level is ideal for individuals who have become comfortable with walking and are ready to challenge their cardiovascular fitness. By incorporating brisk walking or adding slight inclines, participants can elevate their heart rate and enhance their overall health.\n\n" +
                      "Walking at the medium level promotes improved cardiovascular endurance, better stamina, and greater calorie expenditure. Participants may find that this level helps them discover their physical capabilities while also providing mental clarity and stress relief. The combination of movement and mindful awareness fosters a holistic approach to well-being, making each walk both a physical and mental workout.\n\n" +
                      "Consistent engagement in medium walking challenges can lead to significant improvements in physical fitness and mental resilience. Participants may notice increased energy levels and motivation to further explore their walking journeys, paving the way for more intense forms of exercise in the future.";

                case "Walking_Hard_H":
                    return "                            WALKING (HARD)\n\n" +
                      "Description:\n" +
                      "The hard level of the 'Walking' challenge takes participants to the next level by introducing intense walking sessions that may include longer distances, faster paces, or incorporating varied terrains. This level is designed for those who are looking to push their limits and maximize the benefits of walking as a form of exercise. Participants will experience a physically demanding workout that challenges their endurance and strength.\n\n" +
                      "Engaging in hard walking sessions can lead to remarkable improvements in cardiovascular health, muscle tone, and overall fitness levels. Participants may experience heightened endorphin levels, contributing to improved mood and motivation. This level also serves as an excellent way to build mental resilience as individuals push through physical challenges and experience the satisfaction of reaching their walking goals.\n\n" +
                      "By committing to the hard walking challenge, participants can expect significant gains in both physical and mental health. This level fosters a strong sense of achievement and encourages individuals to embrace a more active lifestyle, setting the stage for even greater fitness aspirations in the future.";

                //Learning
                case "Book_Summary__Easy_E":
                    return "                            BOOK SUMMARY (EASY)\n\n" +
                      "Description:\n" +
                      "The easy level of the 'Book Summary' challenge is designed to introduce readers to the practice of summarizing key concepts and ideas from literature. At this level, participants will focus on shorter texts or chapters, allowing them to grasp the main themes and messages without feeling overwhelmed. This challenge is ideal for beginners or those looking to enhance their comprehension skills in a manageable way.\n\n" +
                      "By engaging with easy book summaries, participants will improve their reading retention and critical thinking abilities. Summarizing encourages individuals to distill information into concise points, enhancing their understanding of the material. This practice not only reinforces learning but also builds confidence in discussing literature, making it a valuable skill for academic and personal growth.\n\n" +
                      "Regularly participating in the easy book summary challenge can set a solid foundation for deeper literary analysis. As individuals become more comfortable summarizing, they will find themselves more eager to explore a variety of texts, fostering a lifelong love for reading and learning.";

                case "Book_Summary__Medium_E":
                    return "                            BOOK SUMMARY (MEDIUM)\n\n" +
                      "Description:\n" +
                      "The medium level of the 'Book Summary' challenge elevates the experience by encouraging participants to summarize more complex texts or entire books. This level is tailored for individuals who have developed basic summarizing skills and are ready to tackle longer and denser material. The challenge emphasizes the importance of understanding nuanced themes, character development, and plot dynamics.\n\n" +
                      "Engaging in medium book summaries helps participants sharpen their analytical skills and enhances their ability to synthesize information. By summarizing a wider range of literature, individuals will learn to identify significant ideas and draw connections between different concepts. This practice is particularly beneficial for students and lifelong learners looking to enhance their comprehension and critical analysis of texts.\n\n" +
                      "Consistent practice at the medium level fosters a deeper appreciation for literature and strengthens participants’ ability to articulate their thoughts on various subjects. As individuals grow more adept at summarizing complex works, they will gain confidence in their reading and discussion skills, enriching their overall learning experience.";

                case "Book_Summary__Hard_E":
                    return "                            BOOK SUMMARY (HARD)\n\n" +
                      "Description:\n" +
                      "The hard level of the 'Book Summary' challenge demands a high degree of comprehension and critical thinking as participants are tasked with summarizing challenging and intricate texts. This level is designed for experienced readers who are ready to engage deeply with complex themes, diverse perspectives, and advanced literary techniques. Participants will be encouraged to analyze the underlying messages and implications of the material.\n\n" +
                      "Engaging in hard book summaries enhances cognitive abilities, such as analytical thinking and problem-solving. Participants will learn to evaluate different interpretations of texts, leading to richer discussions and insights. This practice is invaluable for anyone aiming to refine their academic skills or delve into advanced literary studies, as it develops the ability to critique and synthesize complex information effectively.\n\n" +
                      "By committing to the hard book summary challenge, participants will cultivate a profound understanding of literature and its context. This level not only sharpens reading skills but also empowers individuals to express their insights confidently, paving the way for future academic or professional endeavors in fields that require strong analytical and communication skills.";

                case "Character_Analysis_English__Easy_E":
                    return "                            CHARACTER ANALYSIS ENGLISH (EASY)\n\n" +
                      "Description:\n" +
                      "The easy level of 'Character Analysis' in English introduces participants to the fundamental aspects of analyzing characters in literature. At this stage, individuals will explore basic character traits, motivations, and relationships within simpler texts. This challenge aims to enhance comprehension and encourage critical thinking about character development, making it ideal for novice readers.\n\n" +
                      "Through engaging with character analysis, participants will learn to identify key characteristics that drive actions and decisions in a narrative. This skill not only improves reading comprehension but also encourages empathy and deeper connections with the material. By practicing at the easy level, individuals build a strong foundation for understanding more complex characters in literature.\n\n" +
                      "As participants become more comfortable with character analysis, they will gain the confidence to discuss their observations and insights with others. This practice fosters a love for literature and prepares them for more challenging texts, enriching their overall reading experience.";

                case "Character_Analysis_English_Medium_E":
                    return "                            CHARACTER ANALYSIS ENGLISH (MEDIUM)\n\n" +
                      "Description:\n" +
                      "The medium level of 'Character Analysis' in English delves deeper into the intricacies of character development and motivations. Participants will engage with more complex texts, exploring nuanced traits and multifaceted relationships. This stage challenges readers to think critically about the characters’ roles within the story and their connections to the overarching themes.\n\n" +
                      "By analyzing characters at this level, participants enhance their ability to recognize how character choices affect the narrative. This analysis sharpens their critical thinking skills and deepens their understanding of literary devices used by authors. The medium-level challenge prepares individuals for discussions that require a more sophisticated grasp of character dynamics and narrative structure.\n\n" +
                      "Consistent practice at this level builds confidence in interpreting character-driven stories and encourages thoughtful analysis. As participants articulate their findings, they strengthen their communication skills, enabling richer conversations about literature and its impact on readers.";

                case "Character_Analysis_English__Hard_E":
                    return "                            CHARACTER ANALYSIS ENGLISH (HARD)\n\n" +
                      "Description:\n" +
                      "The hard level of 'Character Analysis' in English presents an advanced challenge, requiring participants to engage with complex characters and sophisticated narratives. This stage demands a thorough understanding of character arcs, conflicts, and thematic elements. Readers will analyze how various factors, including social context and personal experiences, shape character behavior and development.\n\n" +
                      "Engaging in hard character analysis enhances critical thinking and analytical skills, as participants evaluate diverse interpretations of characters. This practice cultivates a deeper appreciation for the art of storytelling and encourages readers to question and explore character motivations and implications. By analyzing intricate character relationships, participants gain insights into human behavior and societal influences.\n\n" +
                      "Committing to the hard level of character analysis not only sharpens literary skills but also empowers individuals to engage thoughtfully with challenging texts. This level prepares participants for advanced studies in literature and equips them with the tools to discuss complex character dynamics, enriching their understanding of narrative and its broader impact.";

                case "Characteristic_Analysis_Filipino__Easy_F":
                    return "                            CHARACTER ANALYSIS FILIPINO (EASY)\n\n" +
                      "Description:\n" +
                      "The easy level of 'Characteristic Analysis' in Filipino focuses on foundational skills for analyzing characters within Filipino literature. Participants will engage with simpler texts, identifying basic traits and motivations of characters. This challenge is designed to enhance comprehension and encourage critical thinking about character roles, making it suitable for beginners.\n\n" +
                      "By examining characters at this level, participants will develop the ability to recognize key attributes that influence actions and relationships in stories. This exercise not only aids in understanding narratives but also fosters empathy and connection with Filipino culture. Through consistent practice, individuals will build a solid foundation for analyzing more complex character portrayals.\n\n" +
                      "As participants progress, they will gain the confidence to express their thoughts about character dynamics in discussions. This practice nurtures a love for literature and prepares them for more challenging analysis, enriching their overall reading experience in Filipino texts.";

                case "Characteristic_Analysis_Filipino___Medium_F":
                    return "                            CHARACTER ANALYSIS FILIPINO (MEDIUM)\n\n" +
                      "Description:\n" +
                      "The medium level of 'Characteristic Analysis' in Filipino deepens participants' understanding of character development in more complex narratives. At this stage, readers will engage with a wider range of texts, exploring nuanced traits and relationships. This level challenges individuals to think critically about how characters' actions relate to larger themes in Filipino literature.\n\n" +
                      "By analyzing characters at the medium level, participants enhance their skills in recognizing the impact of character choices on the narrative. This practice promotes critical analysis and helps readers draw connections between character development and societal influences. The medium-level challenge prepares individuals for discussions that require a more sophisticated understanding of character dynamics.\n\n" +
                      "Consistent engagement at this level builds confidence in interpreting character-driven stories and encourages deeper analysis. As participants articulate their insights, they improve their communication skills, leading to richer conversations about Filipino literature and its cultural significance.";

                case "Characteristic_Analysis_Filipino___Hard_F":
                    return "                            CHARACTER ANALYSIS FILIPINO (HARD)\n\n" +
                      "Description:\n" +
                      "The hard level of 'Characteristic Analysis' in Filipino presents an advanced challenge, demanding a comprehensive engagement with complex characters and intricate narratives. Participants will analyze how various factors, including culture and personal experiences, shape character behavior in Filipino literature. This stage requires readers to explore the deeper meanings behind characters' actions and decisions.\n\n" +
                      "Engaging in hard characteristic analysis fosters critical thinking and analytical skills, enabling participants to evaluate multiple interpretations of characters. This practice deepens their understanding of the complexities of Filipino narratives and the themes they convey. By exploring character relationships, individuals gain insights into cultural context and human behavior.\n\n" +
                      "Committing to the hard level of characteristic analysis not only sharpens literary skills but also empowers participants to engage thoughtfully with challenging texts. This level prepares individuals for advanced studies in literature and equips them with the tools to discuss complex character dynamics, enriching their understanding of narrative in Filipino contexts.";

                case "Grammar__Easy_E":
                    return "                            GRAMMAR QUESTION ENGLISH (EASY)\n\n" +
                      "Description:\n" +
                      "The easy level of the Grammar Question challenge focuses on fundamental grammar rules and usage. Participants will engage with straightforward questions that test basic knowledge of sentence structure, punctuation, and word forms. This challenge is designed to enhance grammatical understanding and build confidence in language use.\n\n" +
                      "Through practice at this level, participants will reinforce their grasp of essential grammar concepts, making it easier to identify correct usage in writing and speaking. This exercise fosters a solid foundation in grammar, essential for effective communication in English. By mastering the easy level, individuals prepare themselves for more complex grammatical challenges.\n\n" +
                      "Consistent engagement with easy grammar questions not only improves language skills but also boosts confidence in applying these rules in various contexts. Participants will find themselves more comfortable with English grammar, setting the stage for ongoing learning and development.";

                case "Grammar__Medium_E":
                    return "                            GRAMMAR QUESTION ENGLISH (MEDIUM)\n\n" +
                      "Description:\n" +
                      "The medium level of the Grammar Question challenge delves deeper into grammatical structures and nuances. Participants will tackle more complex questions that explore advanced concepts such as verb tenses, subject-verb agreement, and modifiers. This stage is designed to sharpen grammatical skills and promote critical thinking about language use.\n\n" +
                      "By engaging with medium-level grammar questions, participants will enhance their ability to recognize and apply more intricate rules in writing and speech. This practice not only reinforces understanding but also fosters better communication skills, allowing individuals to express themselves more clearly and effectively. Mastering this level is essential for anyone looking to improve their overall language proficiency.\n\n" +
                      "As participants articulate their reasoning behind grammatical choices, they will develop greater confidence in their language abilities. The medium-level challenge prepares individuals for higher-level grammar analysis and encourages a deeper appreciation for the complexity of the English language.";

                case "Grammar__Hard_E":
                    return "                            GRAMMAR QUESTION ENGLISH (HARD)\n\n" +
                      "Description:\n" +
                      "The hard level of the Grammar Question challenge presents a rigorous examination of advanced grammar concepts. Participants will face challenging questions that require a nuanced understanding of language, including complex sentence structures, nuanced punctuation, and stylistic elements. This stage is designed for those seeking to refine their grammatical expertise.\n\n" +
                      "Engaging with hard grammar questions pushes participants to critically analyze their grammatical choices and understand the intricacies of language use. This level cultivates a deep appreciation for grammar as a tool for effective communication, empowering individuals to express themselves with precision and clarity. The hard level also prepares participants for academic or professional contexts where advanced language skills are essential.\n\n" +
                      "Committing to the hard grammar challenge not only enhances linguistic skills but also builds confidence in navigating complex grammatical situations. This level equips participants with the tools to tackle challenging writing tasks, ensuring they can articulate their thoughts effectively in any setting.";

                case "Vocabulary_Challenge__Easy_E":
                    return "                            VOCABULARY ENGLISH (EASY)\n\n" +
                      "Description:\n" +
                      "The easy level of the Vocabulary Challenge introduces participants to fundamental words and phrases essential for effective communication. This stage focuses on basic vocabulary acquisition, helping individuals build a strong lexicon to enhance their speaking and writing skills. Through simple exercises, participants will familiarize themselves with everyday terms and their meanings.\n\n" +
                      "By practicing at the easy level, individuals will improve their ability to recognize and use basic vocabulary in various contexts. This foundational knowledge is crucial for developing fluency and confidence in language use. Engaging with vocabulary exercises not only reinforces memory retention but also fosters a love for words and language.\n\n" +
                      "As participants progress through the easy vocabulary challenges, they lay the groundwork for exploring more complex words and expressions in the future. This level encourages continuous learning and sets the stage for advanced vocabulary development.";

                case "Vocabulary_Challenge__Medium_E":
                    return "                            VOCABULARY ENGLISH (MEDIUM)\n\n" +
                      "Description:\n" +
                      "The medium level of the Vocabulary Challenge delves into more nuanced and varied vocabulary. Participants will engage with exercises that challenge them to expand their lexicon, focusing on synonyms, antonyms, and context-based usage of words. This stage is designed to enhance language skills and improve overall communication effectiveness.\n\n" +
                      "By tackling medium-level vocabulary challenges, participants will refine their understanding of word nuances and learn to apply them appropriately in conversation and writing. This practice promotes greater expressiveness and precision in language use, which is vital for effective communication. Mastering this level also prepares individuals for higher-level vocabulary challenges that require deeper comprehension.\n\n" +
                      "As participants articulate their thoughts using an enriched vocabulary, they will gain confidence in their language abilities. Engaging with vocabulary at this level fosters a deeper appreciation for the richness of the English language and encourages continued exploration of new words.";

                case "Vocabulary_Challenge__Hard_E":
                    return "                            VOCABULARY ENGLISH (HARD)\n\n" +
                      "Description:\n" +
                      "The hard level of the Vocabulary Challenge presents an advanced examination of language, requiring participants to engage with complex and specialized vocabulary. This stage challenges individuals to comprehend and apply advanced words and phrases in appropriate contexts. This rigorous practice is designed for those aiming to master their vocabulary skills.\n\n" +
                      "Engaging with hard vocabulary challenges pushes participants to think critically about language and its nuances. This level cultivates a deep understanding of word origins, connotations, and usage, empowering individuals to communicate with sophistication and clarity. Tackling challenging vocabulary prepares participants for academic or professional environments where advanced language skills are essential.\n\n" +
                      "Committing to the hard vocabulary challenge not only enhances linguistic proficiency but also builds confidence in articulating complex ideas. This level equips participants with the necessary skills to navigate intricate discussions and writing tasks, ensuring they can express their thoughts effectively and persuasively.";

                case "Word_Count_Challenge_Easy_E":
                    return "                            WORD COUNT (EASY)\n\n" +
                      "Description:\n" +
                      "The easy level of the Word Count Challenge focuses on understanding and measuring the number of words in various texts. This challenge encourages participants to develop skills in identifying key phrases and assessing the length of written content. It is ideal for those looking to improve their writing efficiency and clarity.\n\n" +
                      "By participating in the easy level, individuals will learn how to effectively count words in different contexts, which is essential for tasks such as meeting word limits in assignments or creating concise communications. This exercise reinforces the importance of brevity in writing while still conveying the intended message. Regular practice at this level builds foundational skills that are critical for more advanced writing challenges.\n\n" +
                      "As participants gain confidence in counting and evaluating word use, they will be better equipped to manage their writing assignments and refine their language skills. This easy challenge lays the groundwork for future exploration of more complex writing tasks.";

                case "Word_Count_Challenge_Medium_E":
                    return "                            WORD COUNT (MEDIUM)\n\n" +
                      "Description:\n" +
                      "The medium level of the Word Count Challenge delves deeper into the analysis of text length and complexity. Participants will engage with exercises that require them to not only count words but also assess the effectiveness and clarity of their writing. This level encourages a more critical approach to text analysis, focusing on how word choice impacts communication.\n\n" +
                      "By tackling medium-level word count challenges, individuals will refine their ability to evaluate their writing against specific criteria, such as readability and engagement. This practice promotes awareness of how word count relates to conveying ideas effectively, preparing participants for more advanced writing scenarios. Mastering this level equips individuals with tools to enhance their writing clarity and impact.\n\n" +
                      "As participants articulate their ideas with a keen awareness of word usage, they will build confidence in their writing abilities. Engaging with medium-level challenges fosters a deeper understanding of the relationship between word count and effective communication, encouraging continuous growth in language skills.";

                case "Word_Count_Challenge_Hard_E":
                    return "                            WORD COUNT (HARD)\n\n" +
                      "Description:\n" +
                      "The hard level of the Word Count Challenge presents a rigorous examination of text analysis, requiring participants to engage with complex texts while measuring word count and evaluating the impact of their writing choices. This stage challenges individuals to assess how word economy can enhance or detract from the overall effectiveness of communication.\n\n" +
                      "Engaging with hard word count challenges pushes participants to think critically about their writing style and the nuances of language. This level cultivates an advanced understanding of how precise language can influence clarity and reader engagement. Tackling challenging writing tasks prepares participants for academic or professional environments where word count and content precision are paramount.\n\n" +
                      "Committing to the hard word count challenge not only enhances linguistic proficiency but also builds confidence in articulating complex ideas concisely. This level equips participants with the necessary skills to navigate intricate writing assignments, ensuring they can express their thoughts effectively and persuasively.";

                case "Dialog_Analysis__Easy_F":
                    return "                            DIALOG ANALYSIS FILIPINO (EASY)\n\n" +
                      "Description:\n" +
                      "The easy level of the Dialog Analysis Challenge focuses on understanding and analyzing conversational exchanges in Filipino texts. Participants will engage with straightforward dialogues, learning to identify key elements such as tone, context, and character motivations. This challenge is designed for those looking to improve their analytical skills in interpreting conversations.\n\n" +
                      "By participating at this level, individuals will develop a foundational understanding of how dialogue functions in storytelling and communication. This exercise enhances comprehension and critical thinking, allowing participants to draw insights about character interactions and narrative flow. Regular practice at the easy level sets the stage for deeper explorations of more complex dialogues.\n\n" +
                      "As participants gain confidence in analyzing conversations, they will be better equipped to understand the subtleties of Filipino literature and media. This easy challenge lays the groundwork for future analysis of more intricate dialog structures.";

                case "Dialog_Analysis__Medium_F":
                    return "                            DIALOG ANALYSIS FILIPINO (MEDIUM)\n\n" +
                      "Description:\n" +
                      "The medium level of the Dialog Analysis Challenge delves deeper into the nuances of conversation in Filipino texts. Participants will engage with more complex dialogues that require them to analyze not just the words spoken, but also the underlying emotions, conflicts, and character development. This stage promotes a richer understanding of dialogic interactions.\n\n" +
                      "By tackling medium-level dialog analysis, individuals will refine their ability to interpret characters' intentions and the impact of their words within the context of the story. This practice enhances awareness of narrative techniques, encouraging participants to appreciate how dialogue drives the plot and reveals character dynamics. Mastering this level prepares individuals for higher-level literary analysis.\n\n" +
                      "As participants articulate their interpretations of conversations, they will build confidence in their analytical abilities. Engaging with medium-level challenges fosters a deeper appreciation for the role of dialogue in literature and media, encouraging continued exploration of complex themes and interactions.";

                case "Dialog_Analysis__Hard_F":
                    return "                            DIALOG ANALYSIS FILIPINO (HARD)\n\n" +
                      "Description:\n" +
                      "The hard level of the Dialog Analysis Challenge presents a rigorous examination of complex conversations in Filipino texts. Participants will encounter intricate dialogues that challenge them to consider multiple layers of meaning, including cultural references, subtext, and character motivations. This stage is designed for those seeking to master their analytical skills in dialog interpretation.\n\n" +
                      "Engaging with hard dialog analysis pushes participants to think critically about language and its implications in various contexts. This level cultivates an advanced understanding of how dialogue contributes to character development and thematic depth. Tackling challenging conversations prepares participants for academic or professional analyses of literary and cinematic works.\n\n" +
                      "Committing to the hard dialog analysis challenge not only enhances comprehension skills but also builds confidence in interpreting complex narratives. This level equips participants with the analytical tools needed to navigate intricate character interactions, ensuring they can express their insights effectively and thoughtfully.";

                case "Filipino_Quiz__Easy_F":
                    return "                            FILIPINO QUIZ (EASY)\n\n" +
                      "Description:\n" +
                      "The easy level of the Filipino Quiz challenges participants to demonstrate their basic understanding of the Filipino language and culture. This quiz includes straightforward questions that test vocabulary, grammar, and cultural knowledge, making it accessible for beginners or those looking to refresh their skills.\n\n" +
                      "By engaging with the easy quiz, individuals will build a strong foundation in Filipino, enhancing their ability to communicate effectively. The questions are designed to encourage learning through fun and interactive methods, allowing participants to explore essential aspects of the language and culture. Regular practice at this level fosters confidence and prepares individuals for more advanced challenges.\n\n" +
                      "As participants gain familiarity with the language through the easy quiz, they will develop a deeper appreciation for Filipino culture and traditions. This level is perfect for anyone looking to strengthen their understanding of Filipino while enjoying the learning process.";

                case "Filipino_Quiz__Medium_F":
                    return "                            FILIPINO QUIZ (MEDIUM)\n\n" +
                      "Description:\n" +
                      "The medium level of the Filipino Quiz offers a more challenging experience for participants ready to test their skills beyond the basics. This quiz includes questions that delve into grammar intricacies, idiomatic expressions, and cultural references, pushing participants to apply their knowledge critically.\n\n" +
                      "Engaging with the medium quiz allows individuals to enhance their comprehension and analytical abilities, deepening their understanding of the Filipino language's nuances. Participants will encounter questions that encourage them to think critically about word usage and context, solidifying their language skills while increasing their cultural literacy. Mastering this level prepares individuals for even more complex language challenges.\n\n" +
                      "As participants refine their abilities through the medium quiz, they will become more confident in their communication skills. This level promotes an appreciation for the richness of Filipino culture and language, encouraging further exploration and mastery.";

                case "Filipino_Quiz__Hard_F":
                    return "                            FILIPINO QUIZ (HARD)\n\n" +
                      "Description:\n" +
                      "The hard level of the Filipino Quiz presents a rigorous challenge for those seeking to demonstrate their advanced knowledge of the Filipino language and culture. This quiz features complex questions that require critical thinking, in-depth understanding, and the ability to analyze linguistic nuances and cultural contexts.\n\n" +
                      "Engaging with the hard quiz pushes participants to confront intricate aspects of grammar, advanced vocabulary, and cultural concepts, enhancing their proficiency in Filipino. This level encourages participants to engage with the language at a higher cognitive level, preparing them for academic or professional use of Filipino. Tackling challenging questions builds resilience and confidence in language mastery.\n\n" +
                      "Committing to the hard Filipino quiz not only sharpens linguistic skills but also fosters a greater appreciation for the depth of Filipino culture. Participants will emerge more knowledgeable and articulate, ready to engage in complex conversations and analyses in Filipino.";

                case "Poetry_Challenge__Easy_F":
                    return "                            POETRY FILIPINO (EASY)\n\n" +
                      "Description:\n" +
                      "The easy level of the Poetry Challenge invites participants to explore the beauty of Filipino poetry through simple and accessible exercises. This challenge focuses on familiarizing individuals with poetic forms, themes, and vocabulary, providing a gentle introduction to the richness of Filipino literature.\n\n" +
                      "By participating in the easy poetry challenge, individuals will gain confidence in their ability to appreciate and analyze poetic works. The activities encourage creativity and self-expression, allowing participants to experiment with their writing while developing an understanding of poetic techniques. Regular practice at this level fosters a love for poetry and encourages exploration of deeper literary themes.\n\n" +
                      "As participants engage with the easy poetry challenge, they will cultivate their skills in reading and writing poetry in Filipino. This level is perfect for those looking to enhance their literary appreciation and creativity while enjoying the art of poetry.";

                case "Poetry_Challenge__Medium_F":
                    return "                            POETRY FILIPINO (MEDIUM)\n\n" +
                      "Description:\n" +
                      "The medium level of the Poetry Challenge provides a deeper exploration of Filipino poetry, encouraging participants to engage with more complex forms and themes. This challenge invites individuals to analyze various poetic styles and techniques, enhancing their understanding of the intricacies of poetic expression in Filipino literature.\n\n" +
                      "By tackling medium-level poetry challenges, participants will refine their analytical skills and develop a greater appreciation for the power of language in poetry. The exercises promote critical thinking and creativity, encouraging individuals to express themselves through poetry while exploring cultural and emotional themes. Mastering this level prepares participants for advanced poetic analysis and creation.\n\n" +
                      "As individuals engage with the medium poetry challenge, they will enhance their ability to interpret and create meaningful poetic works. This level fosters a deeper connection to the art of poetry, encouraging continued exploration and growth in literary skills.";

                case "Poetry_Challenge__Hard_F":
                    return "                            POETRY FILIPINO (HARD)\n\n" +
                      "Description:\n" +
                      "The hard level of the Poetry Challenge presents an intensive examination of Filipino poetry, inviting participants to engage with complex themes, advanced poetic structures, and intricate language. This challenge is designed for those seeking to master their poetic analysis and composition skills in a rigorous academic setting.\n\n" +
                      "Engaging with hard poetry challenges pushes participants to confront sophisticated literary concepts, requiring a deep understanding of cultural context and poetic devices. This level encourages individuals to explore the interplay between form and content, enhancing their ability to craft and critique poetry effectively. Tackling challenging exercises builds resilience and confidence in advanced poetic expression.\n\n" +
                      "Committing to the hard poetry challenge not only sharpens literary skills but also fosters a profound appreciation for the depth and diversity of Filipino poetry. Participants will emerge more articulate and knowledgeable, ready to engage in meaningful discussions about the art and craft of poetry.";

                case "Story_Retelling__Easy_F":
                    return "                            STORY RETELLING FILIPINO (EASY)\n\n" +
                      "Description:\n" +
                      "The easy level of the Story Retelling challenge invites participants to summarize and recount simple stories in Filipino. This exercise encourages creativity and comprehension as individuals practice articulating narratives in their own words. By engaging with straightforward stories, participants can focus on the essential elements of storytelling, such as characters, settings, and plot.\n\n" +
                      "Through the easy challenge, individuals enhance their language skills while developing their ability to communicate effectively in Filipino. This level fosters confidence in retelling stories and encourages a love for literature, making it accessible for beginners or those looking to improve their storytelling abilities. Regular practice at this level lays the groundwork for more complex narrative tasks in the future.\n\n" +
                      "As participants gain experience with story retelling, they will learn to express ideas clearly and coherently, honing their skills in both comprehension and oral communication.";

                case "Story_Retelling__Medium_F":
                    return "                            STORY RETELLING FILIPINO (MEDIUM)\n\n" +
                      "Description:\n" +
                      "The medium level of the Story Retelling challenge encourages participants to engage with more intricate narratives, requiring a deeper understanding of the story's themes, character motivations, and underlying messages. This level challenges individuals to think critically about the content and to express their interpretations in fluent Filipino.\n\n" +
                      "By participating in the medium challenge, individuals will refine their storytelling skills, focusing on clarity and creativity. They will learn to highlight significant plot points while providing their insights and reflections on the narrative. This exercise promotes both analytical thinking and effective communication, preparing participants for advanced storytelling techniques.\n\n" +
                      "Engaging with the medium story retelling challenge helps individuals develop a greater appreciation for literature and storytelling. This level encourages deeper engagement with texts, fostering a richer understanding of narratives and enhancing oral expression.";

                case "Story_Retelling__Hard_F":
                    return "                            STORY RETELLING FILIPINO (HARD)\n\n" +
                      "Description:\n" +
                      "The hard level of the Story Retelling challenge presents a rigorous examination of complex narratives, inviting participants to delve into intricate plots, multiple characters, and nuanced themes. This level is designed for those seeking to master their storytelling abilities and critical thinking in Filipino.\n\n" +
                      "Engaging with hard story retelling challenges pushes participants to analyze the narrative structure deeply, requiring them to interpret subtext, character development, and thematic elements. This level encourages individuals to express their insights while retelling stories, enhancing their ability to convey complex ideas clearly and engagingly. Tackling challenging narratives builds confidence and expertise in oral storytelling.\n\n" +
                      "Committing to the hard story retelling challenge not only sharpens language skills but also fosters a profound appreciation for the art of storytelling. Participants will emerge as more skilled narrators, ready to engage with diverse audiences and contribute meaningfully to discussions about literature.";

                case "Budget_Problem__Easy_M":
                    return "                            PROBLEM SOLVING (EASY)\n\n" +
                      "Description:\n" +
                      "The easy level of the Budget Problem challenge invites participants to engage in basic problem-solving scenarios involving budgeting and financial planning. This exercise focuses on simple calculations and decision-making skills, allowing individuals to practice their understanding of financial concepts in an approachable manner.\n\n" +
                      "By participating in the easy challenge, individuals will gain confidence in managing finances and making informed choices. The questions are designed to be straightforward, helping participants develop foundational skills in budgeting that are essential for everyday life. Regular practice at this level enhances financial literacy and prepares individuals for more complex budgeting scenarios.\n\n" +
                      "As participants engage with the easy budget problem challenge, they will learn the importance of planning and prioritizing expenses, fostering a sense of responsibility in financial management.";

                case "Budget_Problem__Medium_M":
                    return "                            PROBLEM SOLVING (MEDIUM)\n\n" +
                      "Description:\n" +
                      "The medium level of the Budget Problem challenge offers participants a more complex set of scenarios, requiring them to analyze financial situations that involve multiple variables and decision points. This level challenges individuals to apply their budgeting skills in a more realistic context, enhancing their problem-solving capabilities.\n\n" +
                      "Engaging with the medium challenge helps participants develop critical thinking and analytical skills as they navigate more intricate financial decisions. This level encourages individuals to explore various solutions and outcomes, enhancing their ability to make informed choices in budgeting. Mastering this level prepares participants for advanced financial planning and problem-solving challenges.\n\n" +
                      "As individuals engage with the medium budget problem challenge, they will become more adept at managing their finances and making strategic decisions, fostering a sense of empowerment in their financial literacy journey.";

                case "Budget_Problem__Hard_M":
                    return "                            PROBLEM SOLVING (HARD)\n\n" +
                      "Description:\n" +
                      "The hard level of the Budget Problem challenge presents a rigorous examination of complex budgeting scenarios, inviting participants to tackle advanced financial problems that require sophisticated analytical skills and strategic thinking. This level is designed for those seeking to deepen their understanding of financial management and budgeting principles.\n\n" +
                      "Engaging with hard budget problem challenges pushes participants to confront multifaceted financial issues, requiring them to consider long-term consequences and trade-offs in their decision-making. This level encourages individuals to apply their knowledge creatively and critically, enhancing their ability to devise effective budgeting strategies. Tackling challenging problems builds resilience and confidence in financial literacy.\n\n" +
                      "Committing to the hard budget problem challenge not only sharpens financial skills but also fosters a profound appreciation for the complexities of budgeting. Participants will emerge as more knowledgeable and strategic planners, ready to navigate intricate financial landscapes with confidence.";

                case "Pattern_Recognition__Easy_M":
                    return "                            PATTERN RECOGNITION (EASY)\n\n" +
                      "Description:\n" +
                      "The easy level of the Pattern Recognition challenge introduces participants to fundamental patterns in various contexts, encouraging them to identify and articulate simple sequences or relationships. This exercise helps individuals build their observational skills and enhances their ability to recognize recurring elements in data or everyday situations.\n\n" +
                      "By engaging in the easy challenge, participants develop foundational skills that are essential for more advanced pattern recognition tasks. This level is designed to be approachable, allowing individuals to gain confidence as they practice spotting patterns in familiar scenarios. Regular participation strengthens critical thinking and lays the groundwork for deeper analytical skills in future challenges.\n\n" +
                      "As individuals hone their pattern recognition skills, they will learn to apply these techniques to real-world situations, fostering a greater appreciation for the structure and order found in both nature and human-made environments.";

                case "Pattern_Recognition__Medium_M":
                    return "                            PATTERN RECOGNITION (MEDIUM)\n\n" +
                      "Description:\n" +
                      "The medium level of the Pattern Recognition challenge escalates the complexity of the patterns presented, requiring participants to analyze more intricate relationships and sequences. This level challenges individuals to employ critical thinking skills as they identify and interpret patterns in varied contexts, enhancing their analytical abilities.\n\n" +
                      "By participating in the medium challenge, individuals will refine their pattern recognition skills, focusing on understanding the underlying principles and logic behind the patterns. This exercise promotes deeper engagement with data and fosters the ability to make connections across different disciplines. Regular practice at this level enhances participants' problem-solving capabilities and prepares them for advanced analytical tasks.\n\n" +
                      "Engaging with the medium pattern recognition challenge helps individuals develop a keen eye for detail and the ability to synthesize information, paving the way for success in academic and professional pursuits.";

                case "Pattern_Recognition__Hard_M":
                    return "                            PATTERN RECOGNITION (HARD)\n\n" +
                      "Description:\n" +
                      "The hard level of the Pattern Recognition challenge presents participants with complex patterns that require sophisticated analytical and logical reasoning skills. This level is designed for those seeking to master their ability to discern intricate patterns and relationships, pushing their cognitive limits.\n\n" +
                      "Engaging with hard pattern recognition challenges encourages participants to confront advanced scenarios that demand innovative thinking and problem-solving strategies. This level promotes resilience and adaptability, as individuals learn to navigate ambiguity and draw meaningful conclusions from complex data sets. Tackling challenging patterns builds confidence and sharpens critical thinking skills.\n\n" +
                      "Committing to the hard pattern recognition challenge not only enhances cognitive abilities but also fosters a profound appreciation for the intricacies of logic and reasoning. Participants will emerge as adept analysts, ready to tackle complex problems in various fields with confidence and precision.";

                case "Real_Life_Application__Easy_M":
                    return "                            REAL APPLICATION (EASY)\n\n" +
                      "Description:\n" +
                      "The easy level of the Real Life Application challenge encourages participants to explore basic concepts and scenarios where theoretical knowledge can be applied in everyday situations. This exercise emphasizes the importance of practical skills and problem-solving abilities in real-world contexts, making learning relevant and engaging.\n\n" +
                      "By participating in the easy challenge, individuals will gain confidence in applying their knowledge to solve straightforward problems and make informed decisions. This level is designed to be accessible, allowing participants to connect theory with practice, which is essential for deeper learning and retention. Regular practice at this level enhances critical thinking and prepares individuals for more complex applications in the future.\n\n" +
                      "As individuals engage with the easy real life application challenge, they will learn to recognize opportunities to apply their learning in daily life, fostering a sense of responsibility and empowerment in their decision-making processes.";

                case "Real_Life_Application__Medium_M":
                    return "                            REAL APPLICATION (MEDIUM)\n\n" +
                      "Description:\n" +
                      "The medium level of the Real Life Application challenge presents participants with more complex scenarios that require them to apply theoretical concepts in a nuanced manner. This level challenges individuals to think critically and creatively as they navigate real-world problems that have multiple variables and potential outcomes.\n\n" +
                      "Engaging with the medium challenge helps participants develop problem-solving skills and the ability to analyze situations from various perspectives. This level encourages individuals to draw connections between theory and practice, enhancing their ability to make informed choices in more complicated scenarios. Mastering this level prepares participants for advanced applications in diverse fields.\n\n" +
                      "As individuals engage with the medium real life application challenge, they will enhance their understanding of how theoretical knowledge can be utilized effectively in everyday situations, fostering a deeper appreciation for the practicality of their learning.";

                case "Real_Life_Application__Hard_M":
                    return "                            REAL APPLICATION (HARD)\n\n" +
                      "Description:\n" +
                      "The hard level of the Real Life Application challenge invites participants to confront intricate real-world problems that require advanced analytical and strategic thinking skills. This level is designed for those seeking to master their application abilities and to explore the complexities of practical decision-making in various contexts.\n\n" +
                      "Engaging with hard real life application challenges pushes participants to think critically and creatively as they address multifaceted issues, requiring them to consider long-term consequences and trade-offs. This level promotes resilience and adaptability, as individuals learn to navigate complex problems and devise innovative solutions. Tackling challenging applications builds confidence and expertise in practical decision-making.\n\n" +
                      "Committing to the hard real life application challenge not only sharpens problem-solving skills but also fosters a profound appreciation for the interconnectedness of knowledge and its application in real life. Participants will emerge as adept problem solvers, ready to tackle diverse challenges with confidence and ingenuity.";

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

                case "Time_Challenge__Easy_M":
                    return "                            TIME CHALLENGE (EASY)\n\n" +
                      "Description:\n" +
                      "The easy level of the Time Challenge focuses on helping participants develop a basic understanding of time management and awareness. This challenge introduces simple tasks that encourage individuals to become more conscious of how they allocate their time throughout the day, fostering a sense of responsibility and efficiency.\n\n" +
                      "By participating in the easy challenge, individuals will learn to prioritize tasks and make the most of their time in a manageable way. This level is designed to be approachable, allowing participants to gain confidence as they practice time management techniques. Regular engagement at this level enhances personal accountability and lays the groundwork for more advanced time management skills.\n\n" +
                      "As individuals engage with the easy time challenge, they will learn to appreciate the importance of effective time management in achieving personal and academic goals.";

                case "Time_Challenge__Medium_M":
                    return "                            TIME CHALLENGE (MEDIUM)\n\n" +
                      "Description:\n" +
                      "The medium level of the Time Challenge presents participants with more complex scenarios that require them to apply time management strategies in a nuanced manner. This challenge encourages individuals to think critically about how they allocate their time while juggling multiple tasks and responsibilities.\n\n" +
                      "Engaging with the medium challenge helps participants refine their planning and prioritization skills, focusing on balancing efficiency with effectiveness. This level is designed to build on the foundation established in the easy challenge, enabling individuals to explore more advanced techniques for managing their time. Mastering this level prepares participants for the demands of academic and professional environments.\n\n" +
                      "As individuals engage with the medium time challenge, they will enhance their understanding of the impact of time management on productivity, fostering a greater appreciation for the role of organization in achieving success.";

                case "Time_Challenge__Hard_M":
                    return "                            TIME CHALLENGE (HARD)\n\n" +
                      "Description:\n" +
                      "The hard level of the Time Challenge invites participants to confront intricate scenarios that require advanced time management skills and strategic planning. This level is designed for those seeking to master their ability to efficiently allocate time in complex environments, pushing their organizational skills to new heights.\n\n" +
                      "Engaging with hard time challenges encourages participants to develop innovative strategies for managing their time effectively, navigating competing priorities, and making critical decisions under pressure. This level promotes resilience and adaptability, as individuals learn to manage stress while achieving their goals. Tackling challenging time management scenarios builds confidence and expertise in personal efficiency.\n\n" +
                      "Committing to the hard time challenge not only sharpens time management skills but also fosters a profound appreciation for the impact of effective planning on success in various aspects of life. Participants will emerge as skilled organizers, ready to tackle diverse challenges with confidence and clarity.";

                // Add more cases for other identifiers...

                default:
                    return "No description available.";
            }
        }
    }
}
