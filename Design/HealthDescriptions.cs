using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace DCP
{
    public class HealthDescriptios
    {
        // Dictionary for mapping images to unique identifiers
        public Dictionary<Image, string> ImageIdentifiers { get; private set; }

        // Dictionary for mapping identifiers to descriptions
        public Dictionary<string, string> ImageDescriptions { get; private set; }
        public List<Image> Images { get; private set; }

        public HealthDescriptios()
        {
            {// Initialize images from .resx with unique identifiers
                ImageIdentifiers = new Dictionary<Image, string>
            {
            //Health
            //Nutritional
            { DCP.Properties.Resources.Hydration__Easy_H, "Hydration__Easy_H" },
            { DCP.Properties.Resources.Hydration__Medium_H, "Hydration__Medium_H" },
            { DCP.Properties.Resources.Hydration__Hard_H, "Hydration__Hard_H" },
            { DCP.Properties.Resources.Mindful_Eating__Easy_H, "Mindful_Eating__Easy_H" },
            { DCP.Properties.Resources.Mindful_Eating__Medium_H, "Mindful_Eating__Medium_H" },
            { DCP.Properties.Resources.Mindful_Eating__Hard_H, "Mindful_Eating__Hard_H" },
            //Mental
            { DCP.Properties.Resources.Gratitude__Easy_H, "Gratitude__Easy_H" },
            { DCP.Properties.Resources.Gratitude__Medium_H, "Gratitude__Medium_H" },
            { DCP.Properties.Resources.Gratitude__Hard_H, "Gratitude__Hard_H" },
            { DCP.Properties.Resources.Mindful_Breathing__Easy_H, "Mindful_Breathing__Easy_H" },
            { DCP.Properties.Resources.Mindful_Breathing__Medium_H, "Mindful_Breathing__Medium_H" },
            { DCP.Properties.Resources.Mindful_Breathing__Hard_H, "Mindful_Breathing__Hard_H" },
            { DCP.Properties.Resources.Quiz_Mental__Easy_H, "Quiz_Mental__Easy_H" },
            { DCP.Properties.Resources.Quiz_Mental__Meduim_H, "Quiz_Mental__Medium_H" },
            { DCP.Properties.Resources.Quiz_Mental__Hard_H, "Quiz_Mental__Hard_H" },
            { DCP.Properties.Resources.Plan_Your_Week_Mental__Easy_H, "Plan_Your_Week_Mental__Easy_H" },
            { DCP.Properties.Resources.Plan_Your_Week_Mental__Medium_H, "Plan_Your_Week_Mental__Medium_H" },
            { DCP.Properties.Resources.Plan_Your_Week_Mental__Hard_H, "Plan_Your_Week_Mental__Hard_H" },
            { DCP.Properties.Resources.Reading_Time__Easy_H, "Reading_Time__Easy_H" },
            { DCP.Properties.Resources.Reading_Time__Medium_H, "Reading_Time__Medium_H" },
            { DCP.Properties.Resources.Reading_Time__Hard_H, "Reading_Time__Hard_H" },
            //LifeStyle
            { DCP.Properties.Resources.Gratitude__Easy_H, "Gratitude__Easy_H" },
            { DCP.Properties.Resources.Gratitude__Medium_H, "Gratitude__Medium_H" },
            { DCP.Properties.Resources.Gratitude__Hard_H, "Gratitude__Hard_H" },
            { DCP.Properties.Resources.Hold_Your_Breath__Easy_H, "Hold_Your_Breath__Easy_H" },
            { DCP.Properties.Resources.Hold_Your_Breath__Medium_H, "Hold_Your_Breath__Medium_H" },
            { DCP.Properties.Resources.Hold_Your_Breath__Hard_H, "Hold_Your_Breath__Hard_H" },
            { DCP.Properties.Resources.No_Blinking__Easy_H, "No_Blinking__Easy_H" },
            { DCP.Properties.Resources.No_Blinking__Medium_H, "No_Blinking__Medium_H" },
            { DCP.Properties.Resources.No_Blinking__Hard_H, "No_Blinking__Hard_H" },
            { DCP.Properties.Resources.Quick_Stretching__Easy_H, "Quick_Stretching__Easy_H" },
            { DCP.Properties.Resources.Quick_Stretching__Medium_H, "Quick_Stretching__Medium_H" },
            { DCP.Properties.Resources.Quick_Stretching__Hard_H, "Quick_Stretching__Hard_H" },
            { DCP.Properties.Resources.Plan_your_week__Easy_H, "Plan_your_week__Easy_H" },
            { DCP.Properties.Resources.Plan_your_week__Medium_H, "Plan_your_week__Medium_H" },
            { DCP.Properties.Resources.Plan_your_week__Hard_H, "Plan_your_week__Hard_H" },
            { DCP.Properties.Resources.Take_a_Cold_Shower__Easy_H, "Take_a_Cold_Shower__Easy_H" },
            { DCP.Properties.Resources.Take_a_Cold_Shower__Medium_H, "Take_a_Cold_Shower__Medium_H" },
            { DCP.Properties.Resources.Take_a_Cold_Shower__Hard_H, "Take_a_Cold_Shower__Hard_H" },
            { DCP.Properties.Resources.Walking_Easy_H, "Walking_Easy_H" },
            { DCP.Properties.Resources.Walking_Medium_H, "Walking_Medium_H" },
            { DCP.Properties.Resources.Walking_Hard_H, "Walking_Hard_H" },


            };
                Images = new List<Image>(ImageIdentifiers.Keys);
            }

            // Initialize descriptions for each identifier
            ImageDescriptions = new Dictionary<string, string>
            {
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

            };
        }
        private string GenerateDescription(string identifier)
        {
            // Sample long description, modify as needed
            switch (identifier)
            {
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

                default:
                    return "No description available.";
            }
        }
    }
}