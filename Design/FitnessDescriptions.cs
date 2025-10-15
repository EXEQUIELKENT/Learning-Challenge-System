using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace DCP
{
    public class FitnessDescriptions
    {
        // Dictionary for mapping images to unique identifiers
        public Dictionary<Image, string> ImageIdentifiers { get; private set; }

        // Dictionary for mapping identifiers to descriptions
        public Dictionary<string, string> ImageDescriptions { get; private set; }
        public List<Image> Images { get; private set; }

        public FitnessDescriptions()
        {
            {// Initialize images from .resx with unique identifiers
                ImageIdentifiers = new Dictionary<Image, string>
            {
            //Fitness
            //Upper Body
            { DCP.Properties.Resources.Side_Plank__Easy_F, "Side_Plank__Easy_F" },
            { DCP.Properties.Resources.Side_Plank__Medium_F, "Side_Plank__Medium_F" },
            { DCP.Properties.Resources.Side_Plank__Hard_F, "Side_Plank__Hard_F" },
            { DCP.Properties.Resources.Russian_Twist__Easy_F, "Russian_Twist__Easy_F" },
            { DCP.Properties.Resources.Russian_Twist__Medium_F, "Russian_Twist__Medium_F" },
            { DCP.Properties.Resources.Russian_Twist__Hard_F, "Russian_Twist__Hard_F" },
            { DCP.Properties.Resources.Planking__Easy_F, "Planking__Easy_F" },
            { DCP.Properties.Resources.Planking__Medium_F, "Planking__Medium_F" },
            { DCP.Properties.Resources.Planking__Hard_F, "Planking__Hard_F" },
            { DCP.Properties.Resources.Mountain_Climbers__Easy_F, "Mountain_Climbers__Easy_F" },
            { DCP.Properties.Resources.Mountain_Climbers__Medium_F, "Mountain_Climbers__Medium_F" },
            { DCP.Properties.Resources.Mountain_Climbers__Hard_F, "Mountain_Climbers__Hard_F" },
            { DCP.Properties.Resources.Diamond_Push_Ups__Easy_F, "Diamond_Push_Ups__Easy_F" },
            { DCP.Properties.Resources.Diamond_Push_Ups__Medium_F, "Diamond_Push_Ups__Medium_F" },
            { DCP.Properties.Resources.Diamond_Push_Ups__Hard_F, "Diamond_Push_Ups__Hard_F" },
            { DCP.Properties.Resources.Push_Ups__Easy__F, "Push_Ups__Easy__F" },
            { DCP.Properties.Resources.Push_Ups__Medium_F, "Push_Ups__Medium_F" },
            { DCP.Properties.Resources.Push_Ups__Hard_F, "Push_Ups__Hard_F" },
            { DCP.Properties.Resources.Bear_Crawl__Easy_F, "Bear_Crawl__Easy_F" },
            { DCP.Properties.Resources.Bear_Crawl__Medium_F, "Bear_Crawl__Medium_F" },
            { DCP.Properties.Resources.Bear_Crawl__Hard_F, "Bear_Crawl__Hard_F" },
            //Lower Body
            { DCP.Properties.Resources.Wall_Sit__Easy_F, "Wall_Sit__Easy_F" },
            { DCP.Properties.Resources.Wall_Sit__Medium_F, "Wall_Sit__Medium_F" },
            { DCP.Properties.Resources.Wall_Sit__Hard_F, "Wall_Sit__Hard_F" },
            { DCP.Properties.Resources.Step_Up__Easy_F, "Step_Up__Easy_F" },
            { DCP.Properties.Resources.Step_Up__Medium_F, "Step_Up__Medium_F" },
            { DCP.Properties.Resources.Step_Up__Hard_F, "Step_Up__Hard_F" },
            { DCP.Properties.Resources.Standing_Side_Leg_Raises__Easy_F, "Standing_Side_Leg_Raises__Easy_F" },
            { DCP.Properties.Resources.Standing_Side_Leg_Raises__Medium__F, "Standing_Side_Leg_Raises__Medium__F" },
            { DCP.Properties.Resources.Standing_Side_Leg_Raises__Hard_F, "Standing_Side_Leg_Raises__Hard_F" },
            { DCP.Properties.Resources.Squat__Easy_F, "Squat__Easy_F" },
            { DCP.Properties.Resources.Squat__Medium_F, "Squat__Medium_F" },
            { DCP.Properties.Resources.Squat__Hard_F, "Squat__Hard_F" },
            { DCP.Properties.Resources.Side_Lunges__Easy_F, "Side_Lunges__Easy_F" },
            { DCP.Properties.Resources.Side_Lunges__Medium_F, "Side_Lunges__Medium_F" },
            { DCP.Properties.Resources.Side_Lunges__Hard_F, "Side_Lunges__Hard_F" },
            { DCP.Properties.Resources.Reverse_Lunges__Easy_F, "Reverse_Lunges__Easy_F" },
            { DCP.Properties.Resources.Reverse_Lunges__Medium_F, "Reverse_Lunges__Medium_F" },
            { DCP.Properties.Resources.Reverse_Lunges__Hard_F, "Reverse_Lunges__Hard_F" },
            { DCP.Properties.Resources.Leg_Raise__Easy_F, "Leg_Raise__Easy_F" },
            { DCP.Properties.Resources.Leg_Raise__Medium_F, "Leg_Raise__Medium_F" },
            { DCP.Properties.Resources.Leg_Raise__Hard_F, "Leg_Raise__Hard_F" },
            { DCP.Properties.Resources.Jumping_Squat__Easy_F, "Jumping_Squat__Easy_F" },
            { DCP.Properties.Resources.Jumping_Squat__Medium_F, "Jumping_Squat__Medium_F" },
            { DCP.Properties.Resources.Jumping_Squat__Hard_F, "Jumping_Squat__Hard_F" },
            { DCP.Properties.Resources.Glute_Bridges__Easy_F, "Glute_Bridges__Easy_F" },
            { DCP.Properties.Resources.Glute_Bridges__Medium_F, "Glute_Bridges__Medium_F" },
            { DCP.Properties.Resources.Glute_Bridges__Hard_F, "Glute_Bridges__Hard_F" },
            { DCP.Properties.Resources.Calf_Raises__Easy_F, "Calf_Raises__Easy_F" },
            { DCP.Properties.Resources.Calf_Raises__Medium_F, "Calf_Raises__Medium_F" },
            { DCP.Properties.Resources.Calf_Raises__Hard_F, "Calf_Raises__Hard_F" },
            //Cardio
            { DCP.Properties.Resources.Bicycle_Crunches__Easy_F, "Bicycle_Crunches__Easy_F" },
            { DCP.Properties.Resources.Bicycle_Crunches__Medium_F, "Bicycle_Crunches__Medium_F" },
            { DCP.Properties.Resources.Bicycle_Crunches__Hard_F, "Bicycle_Crunches__Hard_F" },
            { DCP.Properties.Resources.High_Knees__Easy_F, "High_Knees__Easy_F" },
            { DCP.Properties.Resources.High_Knees__Medium_F, "High_Knees__Medium_F" },
            { DCP.Properties.Resources.High_Knees__Hard_F, "High_Knees__Hard_F" },
            { DCP.Properties.Resources.Jogging__Easy_F, "Jogging__Easy_F" },
            { DCP.Properties.Resources.Jogging__Medium_F, "Jogging__Medium_F" },
            { DCP.Properties.Resources.Jogging__Hard_F, "Jogging__Hard_F" },
            { DCP.Properties.Resources.Jumping_Jacks__Easy_F, "Jumping_Jacks__Easy_F" },
            { DCP.Properties.Resources.Jumping_Jacks__Medium_F, "Jumping_Jacks__Medium_F" },
            { DCP.Properties.Resources.Jumping_Jacks__Hard_F, "Jumping_Jacks__Hard_F" },
            { DCP.Properties.Resources.Toe_Top__Easy_F, "Toe_Top__Easy_F" },
            { DCP.Properties.Resources.Toe_Top__Medium_F, "Toe_Top__Medium_F" },
            { DCP.Properties.Resources.Toe_Top__Hard_F, "Toe_Top__Hard_F" },

            };

                // Initialize descriptions for each identifier
                ImageDescriptions = new Dictionary<string, string>
            {
                //Fitness
                { "Push_Ups__Easy__F", GenerateDescription("Push_Ups__Easy__F") },
                { "Push_Ups__Medium_F", GenerateDescription("Push_Ups__Medium_F") },
                { "Push_Ups__Hard_F", GenerateDescription("Push_Ups__Hard_F") },
                { "Bear_Crawl__Easy_F", GenerateDescription("Bear_Crawl__Easy_F") },
                { "Bear_Crawl__Medium_F", GenerateDescription("Bear_Crawl__Medium_F") },
                { "Bear_Crawl__Hard_F", GenerateDescription("Bear_Crawl__Hard_F") },
                { "Bicycle_Crunches__Easy_F", GenerateDescription("Bicycle_Crunches__Easy_F") },
                { "Bicycle_Crunches__Medium_F", GenerateDescription("Bicycle_Crunches__Medium_F") },
                { "Bicycle_Crunches__Hard_F", GenerateDescription("Bicycle_Crunches__Hard_F") },
                { "Calf_Raises__Easy_F", GenerateDescription("Calf_Raises__Easy_F") },
                { "Calf_Raises__Medium_F", GenerateDescription("Calf_Raises__Medium_F") },
                { "Calf_Raises__Hard_F", GenerateDescription("Calf_Raises__Hard_F") },
                { "Diamond_Push_Ups__Easy_F", GenerateDescription("Diamond_Push_Ups__Easy_F") },
                { "Diamond_Push_Ups__Medium_F", GenerateDescription("Diamond_Push_Ups__Medium_F") },
                { "Diamond_Push_Ups__Hard_F", GenerateDescription("Diamond_Push_Ups__Hard_F") },
                { "Glute_Bridges__Medium_F", GenerateDescription("Glute_Bridges__Medium_F") },
                { "Glute_Bridges__Hard_F", GenerateDescription("Glute_Bridges__Hard_F") },
                { "Glute_Bridges__Easy_F", GenerateDescription("Glute_Bridges__Easy_F") },
                { "High_Knees__Easy_F", GenerateDescription("High_Knees__Easy_F") },
                { "High_Knees__Medium_F", GenerateDescription("High_Knees__Medium_F") },
                { "High_Knees__Hard_F", GenerateDescription("High_Knees__Hard_F") },
                { "Jogging__Easy_F", GenerateDescription("Jogging__Easy_F") },
                { "Jogging__Medium_F", GenerateDescription("Jogging__Medium_F") },
                { "Jogging__Hard_F", GenerateDescription("Jogging__Hard_F") },
                { "Jumping_Jacks__Easy_F", GenerateDescription("Jumping_Jacks__Easy_F") },
                { "Jumping_Jacks__Medium_F", GenerateDescription("Jumping_Jacks__Medium_F") },
                { "Jumping_Jacks__Hard_F", GenerateDescription("Jumping_Jacks__Hard_F") },
                { "Jumping_Squat__Easy_F", GenerateDescription("Jumping_Squat__Easy_F") },
                { "Jumping_Squat__Medium_F", GenerateDescription("Jumping_Squat__Medium_F") },
                { "Jumping_Squat__Hard_F", GenerateDescription("Jumping_Squat__Hard_F") },
                { "Leg_Raise__Easy_F", GenerateDescription("Leg_Raise__Easy_F") },
                { "Leg_Raise__Medium_F", GenerateDescription("Leg_Raise__Medium_F") },
                { "Leg_Raise__Hard_F", GenerateDescription("Leg_Raise__Hard_F") },
                { "Mountain_Climbers__Easy_F", GenerateDescription("Mountain_Climbers__Easy_F") },
                { "Mountain_Climbers__Medium_F", GenerateDescription("Mountain_Climbers__Medium_F") },
                { "Mountain_Climbers__Hard_F", GenerateDescription("Mountain_Climbers__Hard_F") },
                { "Planking__Easy_F", GenerateDescription("Planking__Easy_F") },
                { "Planking__Medium_F", GenerateDescription("Planking__Medium_F") },
                { "Planking__Hard_F", GenerateDescription("Planking__Hard_F") },
                { "Reverse_Lunges__Easy_F", GenerateDescription("Reverse_Lunges__Easy_F") },
                { "Reverse_Lunges__Medium_F", GenerateDescription("Reverse_Lunges__Medium_F") },
                { "Reverse_Lunges__Hard_F", GenerateDescription("Reverse_Lunges__Hard_F") },
                { "Russian_Twist__Easy_F", GenerateDescription("Russian_Twist__Easy_F") },
                { "Russian_Twist__Medium_F", GenerateDescription("Russian_Twist__Medium_F") },
                { "Russian_Twist__Hard_F", GenerateDescription("Russian_Twist__Hard_F") },
                { "Side_Lunges__Easy_F", GenerateDescription("Side_Lunges__Easy_F") },
                { "Side_Lunges__Medium_F", GenerateDescription("Side_Lunges__Medium_F") },
                { "Side_Lunges__Hard_F", GenerateDescription("Side_Lunges__Hard_F") },
                { "Side_Plank__Easy_F", GenerateDescription("Side_Plank__Easy_F") },
                { "Side_Plank__Medium_F", GenerateDescription("Side_Plank__Medium_F") },
                { "Side_Plank__Hard_F", GenerateDescription("Side_Plank__Hard_F") },
                { "Squat__Easy_F", GenerateDescription("Squat__Easy_F") },
                { "Squat__Medium_F", GenerateDescription("Squat__Medium_F") },
                { "Squat__Hard_F", GenerateDescription("Squat__Hard_F") },
                { "Standing_Side_Leg_Raises__Easy_F", GenerateDescription("Standing_Side_Leg_Raises__Easy_F") },
                { "Standing_Side_Leg_Raises__Medium__F", GenerateDescription("Standing_Side_Leg_Raises__Medium__F") },
                { "Standing_Side_Leg_Raises__Hard_F", GenerateDescription("Standing_Side_Leg_Raises__Hard_F") },
                { "Step_Up__Easy_F", GenerateDescription("Step_Up__Easy_F") },
                { "Step_Up__Medium_F", GenerateDescription("Step_Up__Medium_F") },
                { "Step_Up__Hard_F", GenerateDescription("Step_Up__Hard_F") },
                { "Toe_Top__Easy_F", GenerateDescription("Toe_Top__Easy_F") },
                { "Toe_Top__Medium_F", GenerateDescription("Toe_Top__Medium_F") },
                { "Toe_Top__Hard_F", GenerateDescription("Toe_Top__Hard_F") },
                { "Wall_Sit__Easy_F", GenerateDescription("Wall_Sit__Easy_F") },
                { "Wall_Sit__Medium_F", GenerateDescription("Wall_Sit__Medium_F") },
                { "Wall_Sit__Hard_F", GenerateDescription("Wall_Sit__Hard_F") },

            };
                Images = new List<Image>(ImageIdentifiers.Keys);
            }
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
                // Add more cases for other identifiers...

                default:
                    return "No description available.";
            }
        }
    }
}