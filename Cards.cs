using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WPFCarta
{
    public class Card
    {
        public int Number { get; private set; }
        public string Name { get; private set; }
        public string Quote1 { get; private set; }
        public string Quote2 { get; private set; }

        public BitmapSource Bitmap { get; set; }

        public Card(int number, string name, string quote1, string quote2)
        {
            Number = number;
            Name = name;
            Quote1 = quote1;
            Quote2 = quote2;
        }

        public byte[] GetQuote(int quoteNumber, int VoiceNumber)
        {
            string resName = "";
            int resourceNumber = 0;

            if (quoteNumber < 1 || quoteNumber > 2)
                throw new ArgumentException("Invalid quote number", "QuoteNumber");

            switch (VoiceNumber)
            {
                case 1:
                    if (Number <= 50) // first 50 voices are 00232..00331
                        resourceNumber = 2 * (Number - 1) + 232;
                    else // last 40 are 00532..00611
                        resourceNumber = 2 * (Number - 51) + 532;
                    break;
                case 2:
                    if (Number <= 50)
                        resourceNumber = 2 * (Number - 1) + 332;
                    else
                        resourceNumber = 2 * (Number - 51) + 612;
                    break;
                case 3:
                    if (Number <= 50)
                        resourceNumber = 2 * (Number - 1) + 432;
                    else
                        resourceNumber = 2 * (Number - 51) + 692;
                    break;
                default:
                    throw new ArgumentException("Invalid voice number", "VoiceNumber");
            }
            if (quoteNumber == 2)
                resourceNumber++;

            resName = String.Format("VOSCE16_{0:D5}", resourceNumber);

            if (VoiceNumber == 1)
                return Voice1.ResourceManager.GetObject(resName) as byte[];
            else if (VoiceNumber == 2)
                return Voice2.ResourceManager.GetObject(resName) as byte[];
            else if (VoiceNumber == 3)
                return Voice3.ResourceManager.GetObject(resName) as byte[];

            return null;
        }

        public override string ToString()
        {
            return String.Format("Card #{0} - {1}: \"{2}\" - \"{3}\"", Number, Name, Quote1, Quote2);
        }
    }

    public class Cards
    {
        static List<Card> cards = new List<Card>()
        {
            new Card(1,"Stahn", "Th-The sword spoke!", "How in the world can you speak?"),
            new Card(2,"Rutee", "Hey Atwight? I have something to ask you...", "It's about Leon."),
            new Card(3,"Philia", "L-Look out!", "Philia Bomb!"),
            new Card(4,"Garr", "Allow me to disengage my limiter!", "I leave the rest...to you..."),
            new Card(5,"Leon", "I hail from the Kingdom of Seingald...", "My name is Leon Magnus."),
            new Card(6,"Chelsea", "No way, I'm not lost!", "I was just hanging out with Keyaki and my other friends!"),
            new Card(7,"Mary", "I'm alive, Dalis.", "You saved my life twice."),
            new Card(8,"Karyl", "People call me \"Blue Lightning\"", "You can call me Karyl!"),
            new Card(9,"Bruiser", "I am the champion!", "Hey, kid! I got next! Remember that!"),
            new Card(10,"Lilith", "A ladle in my right hand, and a frying pan in my left!", "Arise! Wake the dead!"),
            new Card(11,"Dymlos", "It's all right, Stahn...", "We've lived too long already."),
            new Card(12,"Atwight", "I think I've fulfilled my promise to your mother.", "Never change, okay Rutee?"),
            new Card(13,"Igtenos", "Glory to Phandaria!", "And long live Garr!"),
            new Card(14,"Chaltier", "You will always be my young master.", "I will follow you, no matter what."),
            new Card(15,"Clemente", "You'll be all right. You're a strong girl.", "Take care of yourself, Philia."),
            new Card(16,"Yuri", "Intend to?", "I already have."),
            new Card(17,"Estelle", "The voices of corpses buried beneath the trees...", "creep up and lure us to our doom."),
            new Card(18,"Karol", "Until you really think I'm a boss...", "I'll just do my part as a fellow member of Brave Vesperia."),
            new Card(19,"Rita", "You thought you could win?", "Too bad!"),
            new Card(20,"Raven", "So this is the end, huh?", "Farewell, all my dear fans the world over."),
            new Card(21,"Judith", "This is the path I have chosen.", "...Goodbye."),
            new Card(22,"Kyle", "I want to be just like my dad!", "A great hero, just like him!"),
            new Card(23,"Loni", "Don't attack when your spirit is low!", "You'll get knocked back!"),
            new Card(24,"Judas", "Another nightmare...", "They always begin here, don't they, Chal?"),
            new Card(25,"Reala", "Kyle? Kyyyyle!", "Hey! Over here!"),
            new Card(26,"Nanaly", "I understood Lou better than anyone.", "If nothing else, I'm sure of that."),
            new Card(27,"Harold", "Always keep your rocket launcher ready to fire.", "It's my motto!"),
            new Card(28,"Cress", "We need magic to defeat Dhaos.", "Would you help us, please?"),
            new Card(29,"Mint", "I'm sure I heard my mother's voice!", "The voice that helped me pull through."),
            new Card(30,"Arche", "Hmph! Chester...", "I REALLY, REALLY HATE YOU!"),
            new Card(31,"Claus", "Defeat Dhaos? Are you...serious?", "Hmph, that's quite a tall tale."),
            new Card(32,"Suzu", "I'm not going to cry.", "A true ninja steels her heart against such emotions."),
            new Card(33,"Chester", "Cress...Did you know?", "Ami, she liked you."),
            new Card(34,"Lloyd", "How can we go on a quest to regenerate the world...", "if we can't even save the people standing right in front of us?!"),
            new Card(35,"Colette", "Hehe...thanks.", "I'm glad I was able to live to this day."),
            new Card(36,"Genis", "You don't have to apologize, Lloyd.", "I like you, as well as the people of Iselia."),
            new Card(37,"Raine", "Lloyd Irving, wake up!", "Lloyd!"),
            new Card(38,"Sheena", "The people of Mizuho were chased from this land.", "We all live in hiding."),
            new Card(39,"Kratos", "Hmph...and I thought I'd finally earned the right to die.", "But you're as softhearted as ever."),
            new Card(40,"Zelos", "Now, now, settle down, my darling hunnies!", "Hi there, my little cool beauty, are you hurt?"),
            new Card(41,"Presea", "Thank you for assisting me with my daddy's burial.", "...I've been a great burden on you all."),
            new Card(42,"Regal", "I swear upon my good name and these shackles that bind me...", "I shall not betray you."),
            new Card(43,"Luke", "I'm the ambassador!", "What I say goes! Understand?!"),
            new Card(44,"Guy", "My sword cuts all!", "Enter the amazing Guy!"),
            new Card(45,"Tear", "I didn't expect you to be a Seventh Fonist, too.", "That was careless of me."),
            new Card(46,"Anise", "Oh, no! Ion!", "The Commandant's in danger!"),
            new Card(47,"Jade", "My apologies. I'd forgotten the young master here...", "hasn't a clue about the world around him."),
            new Card(48,"Natalia", "Asch! Stop!", "You're making no sense!"),
            new Card(49,"Emil", "Courage is the magic that turns dreams into reality.", "Courage is what helped me make true friends."),
            new Card(50,"Marta", "Are you gonna fight with me or run?", "C'mon. You're a man. Speak up!"),
            new Card(51,"Reid", "There are people I love that live in this world.", "I'm not letting them die!"),
            new Card(52,"Farah", "Maybe we can become heroes!", "Yeah, no problem!"),
            new Card(53,"Chat", "Will you become my deck hands?", "Not subordinates, but deckhands. And you will address me as captain!"),
            new Card(54,"Max", "She doesn't like pickles on her burgers.", "She's kind of...well, you know."),
            new Card(55,"Keele", "What kind of technology does Celestia possess?", "What exactly is that stone attached to your forehead?"),
            new Card(56,"Meredy", "You saved Meredy and Quickie.", "Thank you."),
            new Card(57,"Ruca", "It was probably just luck.", "I've...never been in a fight before."),
            new Card(58,"Illia", "What the heck are you doing?!", "Are you an enemy or a friend?!"),
            new Card(59,"Spada", "What's up with you?", "You start talking to me and then just go quiet?"),
            new Card(60,"Ange", "Oh my! Then this meeting must be fate.", "I myself am Ange Serena."),
            new Card(61,"Ricardo", "Kids again?", "Since when did battlefields turn into playgrounds?"),
            new Card(62,"Hermana", "Don't worry 'bout it. From now on, eat yer fill, 'kay?", "Imma work hard to keep ya fed."),
            new Card(63,"Shing", "A guy who can't even make the girl in front of him smile?", "Someone like that could never save the world."),
            new Card(64,"Amber", "Yes. I believe you.", "A bad person couldn't use a Soma anyway."),
            new Card(65,"Jadeite", "Listen up. You lay one finger on my sister...", "and you're dead."),
            new Card(66,"Beryl", "Shut up! Just shut up!", "I don't trust anyone!"),
            new Card(67,"Innes", "Oh? A well-endowed beauty, you say?", "I get that all the time~!"),
            new Card(68,"Kunzite", "The unique identification code my master gave me is...", "Kunzite."),
            new Card(69,"Veigue", "All that matters is that Claire is safe.", "I don't care if I die myself."),
            new Card(70,"Mao", "The past isn't important.", "The future is!... Heh heh."),
            new Card(71,"Eugene", "I have seen many Force adepts in my day...", "You have potential."),
            new Card(72,"Annie", "Life doesn't have any one color.", "That's the truth that I've learned."),
            new Card(73,"Tytree", "Wow, sweet! That's five stars right there!", "Did you know about the mushrooms?"),
            new Card(74,"Hilda", "I will overcome...", "I will overcome myself!"),
            new Card(75,"Senel", "If she wants to hurt something we care about...", "Then we won't show her any mercy."),
            new Card(76,"Will", "My real occupation is that of a natural historian.", "I never claimed to be a sheriff."),
            new Card(77,"Chloe", "It is my duty to help those in need.", "I shall accompany you."),
            new Card(78,"Norma", "I have no doubt anymore, master!", "I won't run from the truth!"),
            new Card(79,"Moses", "This here's a job for a real man!", "Y'all leave it to me."),
            new Card(80,"Jay", "I need to know. I can't stand it when there's something I don't know.", "You know?"),
            new Card(81,"Grune", "Oh, come now. Let's not worry about little things, okay?", "They're...little."),
            new Card(82,"Shirley", "They have offered their hand..", "And I intend to accept it."),
            new Card(83,"Caius", "Let's see...", "Well, it's edible...I think."),
            new Card(84,"Rubia", "Come onnnn, flavor!", "Dinner's ready!"),
            new Card(85,"Asch", "f you're scared of killing...", "then throw away your sword!"),
            new Card(86,"Flynn", "We need just laws laid down by the government...", "to ensure that people can live lives of stability and peace."),
            new Card(87,"Repede", "Grrrr...", "Woof! WOOF! WOOF!"),
            new Card(88,"Patty", "A villain like him? Pah!", "He deserves at least this much!"),
            new Card(89,"Dio", "Come on, it'll be fine!", "It'll work out somehow!"),
            new Card(90,"Mell", "How many times have we told you?", "It's dangerous to leave the house"),
        };

        public static Card GetCardByNumber(int val)
        {
            foreach (Card crd in cards)
            {
                if (crd.Number == val)
                    return crd;
            }
            
            return null;
        }
    }
}
