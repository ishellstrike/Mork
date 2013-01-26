using System;

namespace Mork
{
#if WINDOWS || XBOX
    public class Program
    {
        public static Main game;
        [MTAThread] 
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (game = new Main())
            {
                game.Run();
            }
        }
    }
#endif
}

