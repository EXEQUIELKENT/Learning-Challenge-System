using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace DCP
{
    public class LearningDescriptions
    {
        // Dictionary for mapping images to unique identifiers
        public Dictionary<Image, string> ImageIdentifiers { get; private set; }

        // Dictionary for mapping identifiers to descriptions
        public Dictionary<string, string> ImageDescriptions { get; private set; }
        public List<Image> Images { get; private set; }

        public LearningDescriptions()
        {
            {// Initialize images from .resx with unique identifiers
                ImageIdentifiers = new Dictionary<Image, string>
            {
            //Learning
            //English
            { DCP.Properties.Resources.Book_Summary__Easy_E, "Book_Summary__Easy_E" },
            { DCP.Properties.Resources.Book_Summary__Medium_E, "Book_Summary__Medium_E" },
            { DCP.Properties.Resources.Book_Summary__Hard_E, "Book_Summary__Hard_E" },
            { DCP.Properties.Resources.Character_Analysis_English__Easy_E, "Character_Analysis_English__Easy_E" },
            { DCP.Properties.Resources.Character_Analysis_English_Medium_E, "Character_Analysis_English_Medium_E" },
            { DCP.Properties.Resources.Character_Analysis_English__Hard_E, "Character_Analysis_English__Hard_E" },
            { DCP.Properties.Resources.Grammar__Easy_E, "Grammar__Easy_E" },
            { DCP.Properties.Resources.Grammar__Medium_E, "Grammar__Medium_E" },
            { DCP.Properties.Resources.Grammar__Hard_E, "Grammar__Hard_E" },
            { DCP.Properties.Resources.Vocabulary_Challenge__Easy_E, "Vocabulary_Challenge__Easy_E" },
            { DCP.Properties.Resources.Vocabulary_Challenge__Medium_E, "Vocabulary_Challenge__Medium_E" },
            { DCP.Properties.Resources.Vocabulary_Challenge__Hard_E, "Vocabulary_Challenge__Hard_E" },
            { DCP.Properties.Resources.Word_Count_Challenge_Easy_E, "Word_Count_Challenge_Easy_E" },
            { DCP.Properties.Resources.Word_Count_Challenge_Medium_E, "Word_Count_Challenge_Medium_E" },
            { DCP.Properties.Resources.Word_Count_Challenge_Hard_E, "Word_Count_Challenge_Hard_E" },
            //Filipino
            { DCP.Properties.Resources.Characteristic_Analysis_Filipino__Easy_F, "Characteristic_Analysis_Filipino__Easy_F" },
            { DCP.Properties.Resources.Characteristic_Analysis_Filipino___Medium_F, "Characteristic_Analysis_Filipino___Medium_F" },
            { DCP.Properties.Resources.Characteristic_Analysis_Filipino___Hard_F, "Characteristic_Analysis_Filipino___Hard_F" },
            { DCP.Properties.Resources.Dialog_Analysis__Easy_F, "Dialog_Analysis__Easy_F" },
            { DCP.Properties.Resources.Dialog_Analysis__Medium_F, "Dialog_Analysis__Medium_F" },
            { DCP.Properties.Resources.Dialog_Analysis__Hard_F, "Dialog_Analysis__Hard_F" },
            { DCP.Properties.Resources.Filipino_Quiz__Easy_F, "Filipino_Quiz__Easy_F" },
            { DCP.Properties.Resources.Filipino_Quiz__Medium_F, "Filipino_Quiz__Medium_F" },
            { DCP.Properties.Resources.Filipino_Quiz__Hard_F, "Filipino_Quiz__Hard_F" },
            { DCP.Properties.Resources.Poetry_Challenge__Easy_F, "Poetry_Challenge__Easy_F" },
            { DCP.Properties.Resources.Poetry_Challenge__Medium_F, "Poetry_Challenge__Medium_F" },
            { DCP.Properties.Resources.Poetry_Challenge__Hard_F, "Poetry_Challenge__Hard_F" },
            { DCP.Properties.Resources.Story_Retelling__Easy_F, "Story_Retelling__Easy_F" },
            { DCP.Properties.Resources.Story_Retelling__Medium_F, "Story_Retelling__Medium_F" },
            { DCP.Properties.Resources.Story_Retelling__Hard_F, "Story_Retelling__Hard_F" },
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

            };
        }
        private string GenerateDescription(string identifier)
        {
            // Sample long description, modify as needed
            switch (identifier)
            {
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

                default:
                    return "No description available.";
            }
        }
    }
}