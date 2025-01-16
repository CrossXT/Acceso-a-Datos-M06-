using SFML.Graphics;
using SFML.Audio;
using SFML.Window;
using SFML.System;

using System.IO;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Reflection.PortableExecutable;

namespace AdventureEngine
{
    internal struct Command
    {
        public string id;
        public string param1;
        public string param2;
        public string param3;
        public string param4;
        public string param5;
        public string param6;
    }

    internal static class Program
    {
        // Constants

        static int screenWidth = 1280;
        static int screenHeight = 720;

        static string storyTitle = "El secreto de la señora Dorotea";

        static int characterX = 720;
        static int characterY = 580;
        static int dialogBaseX = 30;
        static int dialogBaseY = 526;
        static int dialogNameX = 90;
        static int dialogNameY = 544;
        static int dialogContentX = 90;
        static int dialogContentY = 600;

        // Characters

        static Texture basilioTexture;
        static Texture carmeloTexture;
        static Texture doroteaTexture;
        static Texture nicolasTexture;

        static Sprite characterSprite;

        // Background

        static Texture armeriaTexture;
        static Texture castilloTexture;
        static Texture comedorTexture;
        static Texture dormitorioTexture;
        static Texture mazmorraTexture;
        static Texture vestibuloTexture;

        static Sprite backgroundSprite;

        // Music

        static Music animadaMusic;
        static Music normalMusic;
        static Music tensaMusic;

        // Dialog

        static Texture dialogBaseTexture;
        static Sprite dialogBaseSprite;

        static Font dialogFont;

        static Text dialogNameText;
        static Text dialogContentText;


        static bool dialogVisible;

        // Commands

        static bool waitingForCommand;
        static int commandIndex;

        static Clock clock;
        static float waitTime;

        static bool continuePressed;

        static List<Command> commandList;
        
        
        //static Command[] commands =
        //            {
        //                new Command() { id = CommandId.setBackground, param1 = "castillo" },
        //                new Command() { id = CommandId.playMusic, param1 = "normal" },
        //                new Command() { id = CommandId.wait, param1 = "1.0" },
        //                new Command() { id = CommandId.showDialog, param1 = "nicolas", param2 = "Buenos días" },
        //                new Command() { id = CommandId.showDialog, param1 = "nicolas", param2 = "Soy Nicolás, el mayordomo de la señora Dorotea" },
        //                new Command() { id = CommandId.showDialog, param1 = "nicolas", param2 = "Permítame llevar su equipaje" },
        //                new Command() { id = CommandId.wait, param1 = "1" },
        //                new Command() { id = CommandId.setBackground, param1 = "vestibulo" },
        //                new Command() { id = CommandId.wait, param1 = "1" },
        //                new Command() { id = CommandId.showDialog, param1 = "nicolas", param2 = "Éste es el vestíbulo del castillo" },
        //                new Command() { id = CommandId.showDialog, param1 = "nicolas", param2 = "Espere aquí. La señora Dorotea vendrá en cualquier momento" },
        //                new Command() { id = CommandId.showDialog, param1 = "nicolas", param2 = "Si me disculpa, llevaré su equipaje a su dormitorio" },
        //                new Command() { id = CommandId.wait, param1 = "3" },
        //                new Command() { id = CommandId.playMusic, param1 = "tensa" },
        //                new Command() { id = CommandId.showDialog, param1 = "basilio", param2 = "¿Quién es usted?" },
        //                new Command() { id = CommandId.showDialog, param1 = "basilio", param2 = "Bueno, en realidad me da igual" },
        //                new Command() { id = CommandId.showDialog, param1 = "basilio", param2 = "Vaya a buscar a ese inútil de Nicolás porque tenemos un problema" },
        //                new Command() { id = CommandId.showDialog, param1 = "basilio", param2 = "LA SEÑORA DOROTEA HA DESAPARECIDO" },
        //                new Command() { id = CommandId.showDialog, param1 = "basilio", param2 = "BASILIO" },
        //                new Command() { id = CommandId.showDialog, param1 = "basilio", param2 = "Madre mia estoy rompiendo la Cuarta pared" },
        //                new Command() { id = CommandId.showDialog, param1 = "basilio", param2 = "Que mastodontico de mi parte verdad?" },
        //                new Command() { id = CommandId.showDialog, param1 = "nicolas", param2 = "Pues la verdad es que no pedazo de clasista con peluca" },
        //                new Command() { id = CommandId.showDialog, param1 = "basilio", param2 = "Que te madreo eh" },


        //};


        static void LoadConfig()
        {
            StreamReader reader;

            reader = new StreamReader("configuracion.ini", Encoding.UTF8);

            while(!reader.EndOfStream)
            {
                string line = reader.ReadLine();

                string[] parts = line.Split('=');

                if (parts[0] == "screenWidth")
                {
                    screenWidth = Int32.Parse(parts[1]);
                }
                else if (parts[0] == "screenHeight")
                {
                    screenHeight = Int32.Parse(parts[1]);
                }
                else if (parts[0] == "storyTitle")
                {
                    storyTitle = parts[1];
                }
                else if (parts[0] == "characterX")
                {
                    characterX = Int32.Parse(parts[1]);
                }
                else if (parts[0] == "characterY")
                {
                    characterY = Int32.Parse(parts[1]);
                }
                else if (parts[0] == "dialogBaseX")
                {
                    dialogBaseX = Int32.Parse(parts[1]);
                }
                else if (parts[0] == "dialogBaseY")
                {
                    dialogBaseY = Int32.Parse(parts[1]);
                }
                else if (parts[0] == "dialogNameX")
                {
                    dialogNameX = Int32.Parse(parts[1]);
                }
                else if (parts[0] == "dialogNameY")
                {
                    dialogNameY = Int32.Parse(parts[1]);
                }
                else if (parts[0] == "dialogContentX")
                {
                    dialogContentX = Int32.Parse(parts[1]);
                }
                else if (parts[0] == "dialogContentY")
                {
                    dialogContentY = Int32.Parse(parts[1]);
                }
            }

            reader.Close();
        }

        static void Main()
        {

            // Configuration Initializing
            LoadConfig();
            LoadCommands("Main.script");


            // Window initialization

            var mode = new VideoMode((uint)screenWidth, (uint)screenHeight);
            var window = new RenderWindow(mode, storyTitle);
            window.KeyPressed += OnKeyPressed;
            window.MouseButtonPressed += OnMousePressed;

            // Characters initialization




            //Primero hacemos que busque el directorio de sprites


            Dictionary<string, Texture> DiccionarioCharacters;

            DiccionarioCharacters = new Dictionary<string, Texture>();

            string RutaDirectorio = Directory.GetCurrentDirectory();

            string DirectorioCharacters;

            DirectorioCharacters = RutaDirectorio + "\\" + "characters";

            string[] Sprites;

            Sprites = Directory.GetFiles(DirectorioCharacters);

            for(int i = 0; i < Sprites.Length; i++) 
            {
                string NombreSprite;
                int posicion;

                posicion = Sprites[i].LastIndexOf('\\');

                NombreSprite = Sprites[i].Substring(posicion + 1);


                string[] Characters = NombreSprite.Split('.');

                DiccionarioCharacters[Characters[0]] = new Texture(Sprites[i]);

            }




            Texture t = DiccionarioCharacters.First().Value;
            characterSprite = new Sprite();
            characterSprite.Texture = t;
            characterSprite.Origin = new Vector2f(t.Size.X / 2, t.Size.Y);
            characterSprite.Position = new Vector2f(characterX, characterY);


            // Background initialization

            Dictionary<string, Texture> DiccionarioBackgrounds;

            DiccionarioBackgrounds = new Dictionary<string, Texture>();




            string DirectorioBackgrounds;



            DirectorioBackgrounds = RutaDirectorio + "\\" + "backgrounds";

            string[] BackgroundFiles;



            BackgroundFiles = Directory.GetFiles(DirectorioBackgrounds);

            for (int i = 0; i < BackgroundFiles.Length; i++)
            {
                string NombreBackground;
                int posicion;

                posicion = BackgroundFiles[i].LastIndexOf('\\');

                NombreBackground = BackgroundFiles[i].Substring(posicion + 1);


                string[] BackgroundsParts = NombreBackground.Split('.');

                DiccionarioBackgrounds[BackgroundsParts[0]] = new Texture(BackgroundFiles[i]);

            }




            Texture B = DiccionarioBackgrounds.First().Value;
            backgroundSprite = new Sprite();
            backgroundSprite.Texture = B;
            backgroundSprite.Position = new Vector2f(0, 0);

            // Music initialization


            Dictionary<string, Music> DiccionarioMusics;

            DiccionarioMusics = new Dictionary<string, Music>();




            string DirectorioMusics;



            DirectorioMusics = RutaDirectorio + "\\" + "musics";

            string[] MusicFiles;



            MusicFiles = Directory.GetFiles(DirectorioMusics);

            for (int i = 0; i < MusicFiles.Length; i++)
            {
                string NombreMusic;
                int posicion;

                posicion = MusicFiles[i].LastIndexOf('\\');

                NombreMusic = MusicFiles[i].Substring(posicion + 1);


                string[] MusicParts = NombreMusic.Split('.');

                DiccionarioMusics[MusicParts[0]] = new Music(MusicFiles[i]);

            }




            // Dialog initialization

            dialogBaseTexture = new Texture("dialog\\base.png");

            dialogBaseSprite = new Sprite();
            dialogBaseSprite.Texture = dialogBaseTexture;
            dialogBaseSprite.Position = new Vector2f(dialogBaseX, dialogBaseY);

            dialogFont = new Font("fonts\\default.ttf");

            dialogNameText = new Text("", dialogFont);
            dialogNameText.Position = new Vector2f(dialogNameX, dialogNameY);

            dialogContentText = new Text("", dialogFont);
            dialogContentText.Position = new Vector2f(dialogNameX, dialogNameY);

            // Commands initialization

            commandList = new List<Command>();

            commandIndex = 0;

            clock = new Clock();

            // Start the game loop
            while (window.IsOpen)
            {
                // Process events
                window.DispatchEvents();

                // Game logic goes here

                if (waitingForCommand)
                {
                    Command command = commandList[commandIndex];

                    if(command.id == "wait")
                    {
                        if(clock.ElapsedTime.AsSeconds() >= waitTime || continuePressed)
                        {
                            waitingForCommand = false;
                            commandIndex ++;
                        }
                    }
                    else if(command.id == "showdialog")
                    {
                        if(continuePressed)
                        {
                            dialogVisible = false;
                            waitingForCommand = false;
                            commandIndex ++;
                        }
                    }
                }
                else if(commandIndex < commandList.Count)
                {
                    Command command = commandList[commandIndex];

                    if(command.id == "setBackground")
                    {
                        backgroundSprite.Texture = DiccionarioBackgrounds[command.param1];

                        commandIndex++;
                    }
                    else if(command.id == "playMusic")
                    {
                        // Bucle que pase por todas las músicas del diccionario
                        // y las pare

                        foreach(Music MusicList in DiccionarioMusics.Values)
                        {
                            MusicList.Stop();
                        }
                        // Obtener la música
                        Music m = DiccionarioMusics[command.param1];
                        m.Loop = true;
                        m.Play();
                        commandIndex++;
                    }
                    else if(command.id == "stopMusic")
                    {
                        normalMusic.Stop();
                        animadaMusic.Stop();
                        tensaMusic.Stop();

                        commandIndex++;
                    }
                    else if (command.id == "showDialog")
                    {

                        characterSprite.Texture = DiccionarioCharacters[command.param1];

                        dialogNameText = new Text(command.param1, dialogFont);
                        dialogNameText.Position = new Vector2f(100, 540);

                        dialogContentText = new Text(command.param2, dialogFont);
                        dialogContentText.Position = new Vector2f(100, 600);

                        dialogVisible = true;

                        waitingForCommand = true;
                    }
                    else if(command.id == "wait")
                    {
                        waitTime = Single.Parse(command.param1);
                        clock.Restart();
                        waitingForCommand = true;
                    }

                }

                window.Draw(backgroundSprite);

                if(dialogVisible)
                {
                    window.Draw(characterSprite);
                    window.Draw(dialogBaseSprite);
                    window.Draw(dialogNameText);
                    window.Draw(dialogContentText);
                }

                 // Finally, display the rendered frame on screen
                window.Display();

                continuePressed = false;

            }
        }

        static void LoadCommands(string filename)
        {
            //Borramos los comandos que hubiesen antes
            commandList.Clear();

            //Abrimos el fitchero filename con un lector de texto
            string RutaDirectorio = Directory.GetCurrentDirectory();

            string DirectorioComando;

            DirectorioComando = RutaDirectorio + "\\" + "scripts";

            StreamReader readerCommand;

            readerCommand = new StreamReader(DirectorioComando, Encoding.UTF8);


            while (!readerCommand.EndOfStream)
            {
                string commandLine = readerCommand.ReadLine();

                string[] commandParts = commandLine.Split("->");

                Command command = new Command();
                    
                    
                command.id = commandParts[0];
                command.param1 = commandParts[1];
                command.param2 = commandParts[2];
                command.param3 = commandParts[3];
                command.param4 = commandParts[4];
                command.param5 = commandParts[5];
                command.param6 = commandParts[6];

                //Lo añadimos a la lista

                commandList.Add(command);

            }

            //Cerramos el fitchero
            readerCommand.Close();  

        }

        static void OnMousePressed(object sender, MouseButtonEventArgs e)
        {
            if(e.Button == Mouse.Button.Left)
            {
                continuePressed = true;
            }
        }

        static void OnKeyPressed(object sender, KeyEventArgs e)
        {
            var window = (Window)sender;
            if (e.Code == Keyboard.Key.Escape)
            {
                window.Close();
            }
            else if(e.Code == Keyboard.Key.Space)
            {
                continuePressed = true;
            }
        }
    }


}
